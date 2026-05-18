using UnityEngine;
using UnityEngine.InputSystem;

public class TowerController : MonoBehaviour
{
    bool isPlacingTower = false;
    GameObject towerToPlace;
    GameObject currentVisual;
    InfoScript info;
    UIScript gameUI;
    [SerializeField] LayerMask towerLayerMask;
    [SerializeField] GameObject visualPrefab;  
    private void Start()
    {
        info = FindFirstObjectByType<InfoScript>();
        gameUI = FindFirstObjectByType<UIScript>();
    }

    public void PlaceTower(GameObject towerPrefab, int towerIndex)
    {
        if (isPlacingTower) { Destroy(towerToPlace); }

        towerToPlace = Instantiate(towerPrefab);
        towerToPlace.GetComponent<TowerScript>().towerIndex = towerIndex;

        currentVisual = Instantiate(visualPrefab, towerToPlace.transform.position, Quaternion.identity, towerToPlace.transform);
        currentVisual.transform.localScale = Vector3.one * towerToPlace.GetComponent<TowerScript>().GetRange() * 2f;
        
        isPlacingTower = true;
      
    }

    public void CancelPlacingTower()
    {
        if (towerToPlace != null)
        {
            Destroy(towerToPlace);
            Destroy(currentVisual);

            towerToPlace = null;
            currentVisual = null;
        }
        isPlacingTower = false;
    }

    public void ConfirmPlacingTower()
    {
        if (towerToPlace.GetComponent<TowerScript>().PlaceTower())
        {
            if (towerToPlace != null)
            {
                towerToPlace = null;
            }
            isPlacingTower = false;
        }
    }

    private void OnPlaceTower()
    {
        if (isPlacingTower)
        {
            ConfirmPlacingTower();
        }
        else
        {
          Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
          RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, towerLayerMask);
            if (hit.collider != null && hit.collider.CompareTag("Tower"))
            {
                info.ShowInfo(hit.collider.GetComponent<TowerScript>());
                hit.collider.gameObject.GetComponentInChildren<rangeScript>().ShowVisual();
            }
            else if (!info.IsPointerOverUIToolkit())
            {
               info.HideInfo();
            }
        }

    }

    private void OnCancelPlacement()
    {
        if (isPlacingTower)
        {
            CancelPlacingTower();
        }
    }

    private void Update()
    {
        if (isPlacingTower)
        {
           Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); ;
           towerToPlace.transform.position = mousePos; 
        }
    }
}
