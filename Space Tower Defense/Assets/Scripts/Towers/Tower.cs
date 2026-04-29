using UnityEngine;
[System.Serializable]
public class Tower
{
    public Texture2D towerIcon;
    public GameObject towerPrefab;
    public string towerName;
    [TextArea(3,10)]
    public string towerDescription;
}
