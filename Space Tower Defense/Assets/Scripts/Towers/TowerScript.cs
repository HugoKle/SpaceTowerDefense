using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [SerializeField] public TowerLevel[] towerLevels;
    [SerializeField] LayerMask collLayers;
    public int towerIndex = 0;
    public int currentLevel = 0;
    public int value = 0;

    Rigidbody2D rb;
    bool isTouchingPath = false;
    bool isPlaced = false;
    UIScript gameUI;
    GameObject currentTower;
    SpriteRenderer sr;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    public bool PlaceTower()
    {
        
        if (gameUI == null)
        {
            gameUI = FindFirstObjectByType<UIScript>();
        }

        

        int wealth = gameUI.GetMoney();
        if (isTouchingPath || wealth < towerLevels[0].Price || gameUI.IsPointerOverUIToolkit()) { return false; }

        isPlaced = true;
        gameUI.AddMoney(-towerLevels[0].Price);
        value += towerLevels[0].Price;
        sr.enabled = false;
        SpawnTower(0);

        rangeScript range = GetComponentInChildren<rangeScript>();
        if (range != null)
        {
            range.HideVisual();
        }


        return true;


    }

   public void UpgradeTower()
    {
        if (currentLevel >= towerLevels.Length - 1) { return; }
        if (gameUI == null)
        {
            gameUI = FindFirstObjectByType<UIScript>();
        }
        int wealth = gameUI.GetMoney();
        if (wealth < towerLevels[currentLevel + 1].Price) { return; }
        currentLevel++;
        value += towerLevels[currentLevel].Price;
        gameUI.AddMoney(-towerLevels[currentLevel].Price);
        SpawnTower(currentLevel);

        GetComponentInChildren<rangeScript>().gameObject.transform.localScale = Vector3.one * towerLevels[currentLevel].towerRange * 2f;
    }





    private void Update()
    {
        isTouchingPath = rb.IsTouchingLayers(collLayers);

        if (!isPlaced)
        {
            if (sr == null) { sr = GetComponent<SpriteRenderer>(); }


            if (gameUI == null) { gameUI = FindFirstObjectByType<UIScript>(); }
            int wealth = gameUI.GetMoney();

            if (isTouchingPath || wealth < towerLevels[0].Price)
            {
                sr.material.color = Color.red;
            }
            else
            {
                sr.material.color = Color.green;
            }
        }
    }

    void SpawnTower(int towerIndex)
    {
        if (currentTower != null) { Destroy(currentTower); }
        currentTower = Instantiate(towerLevels[towerIndex].towerPrefab, transform.position, Quaternion.identity);
    }

    public float GetRange()
    {
        return towerLevels[currentLevel].towerRange;
    }

    private void OnDestroy()
    {
        Destroy(currentTower);
    }

}
