using UnityEngine;
using UnityEngine.InputSystem;

public class TowerController : MonoBehaviour
{
    bool isPlacingTower = false;
    GameObject towerToPlace;
    InfoScript info;
     private void Start()
    {
        info = FindFirstObjectByType<InfoScript>();
    }

    public void PlaceTower(GameObject towerPrefab, int towerIndex)
    {
        towerToPlace = Instantiate(towerPrefab);
        towerToPlace.GetComponent<TowerScript>().towerIndex = towerIndex;
        isPlacingTower = true;
      
    }

    public void CancelPlacingTower()
    {
        if (towerToPlace != null)
        {
            Destroy(towerToPlace);
            towerToPlace = null;
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
          RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null && hit.collider.CompareTag("Tower"))
            {
                info.ShowInfo(hit.collider.GetComponent<TowerScript>());
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
