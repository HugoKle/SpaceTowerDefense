using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [SerializeField] TowerLevel[] towerLevels;

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
    public void PlaceTower()
    {
        
        if (gameUI == null)
        {
            gameUI = FindFirstObjectByType<UIScript>();
        }

        isPlaced = true;
        int wealth = gameUI.GetMoney();
        if (isTouchingPath || wealth < towerLevels[0].Price) { return;}

        SpawnTower(0);

    }

    private void Update()
    {
        isTouchingPath = rb.IsTouchingLayers(3); // line 31

        if (!isPlaced)
        {
            if (sr == null) { GetComponent<SpriteRenderer>(); }

            if (isTouchingPath)
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


}
