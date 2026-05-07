using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InfoScript : MonoBehaviour
{
    TowerScript tower;
    UIDocument _infoDocument;
    UIScript gameUI;

    Label towerName;
    Label description;
    Label stats;
    Label upgradeStats;
    VisualElement towerIcon;
    Button upgradeButton;
    VisualElement panel;



    private void Start()
    {
        gameUI = FindFirstObjectByType<UIScript>();
        _infoDocument = GetComponent<UIDocument>();

        VisualElement root = _infoDocument.rootVisualElement;

        panel = root.Q<VisualElement>("Background");
        upgradeButton = root.Q<Button>("UpgradeButton");
        upgradeButton.RegisterCallback<ClickEvent>(evt => tower.UpgradeTower());
        upgradeButton.RegisterCallback<ClickEvent>(evt => UpdateInfo());

        Button sellButton = root.Q<Button>("SellButton");
        sellButton.RegisterCallback<ClickEvent>(evt => SellTower());

        towerName = root.Q<Label>("TowerName");
        description = root.Q<Label>("Description");
        stats = root.Q<Label>("Stats");
        upgradeStats = root.Q<Label>("UpgradeStats");
        towerIcon = root.Q<VisualElement>("Icon");

        panel.style.display = DisplayStyle.None;
    }

    public void ShowInfo(TowerScript selectedTower)
    {
        panel.style.display = DisplayStyle.Flex;

        if (tower != null)
        {
            rangeScript range = tower.GetComponentInChildren<rangeScript>();
            if (range != null)
            {
                range.HideVisual();
            }
        }

        tower = selectedTower;

        

        UpdateInfo();
    }

    void UpdateInfo()
    {
        int index = tower.towerIndex;
        int towerLevel = tower.currentLevel;

        towerName.text = gameUI.towers[index].towerName + $" ({towerLevel + 1})";
        description.text = gameUI.towers[index].towerDescription;
        towerIcon.style.backgroundImage = new StyleBackground(gameUI.towers[index].towerIcon);

        stats.text = $"Damage: {tower.towerLevels[towerLevel].towerDamage}\n" +
            $"Range: {tower.towerLevels[towerLevel].towerRange}\n" +
            $"Fire Rate: {tower.towerLevels[towerLevel].towerAttackSpeed}";

       

        if (towerLevel + 1 >= tower.towerLevels.Length)
        {
            upgradeButton.text = "Max Level";
            upgradeStats.text = "Max Level";
            upgradeButton.SetEnabled(false);
        }
        else
        {
            upgradeButton.SetEnabled(true);
            upgradeButton.text = $"Upgrade (${tower.towerLevels[towerLevel + 1].Price})";

            upgradeStats.text =  $"Damage: {tower.towerLevels[towerLevel + 1].towerDamage}\n" +
           $"Range: {tower.towerLevels[towerLevel + 1].towerRange}\n" +
           $"Fire Rate: {tower.towerLevels[towerLevel + 1].towerAttackSpeed}";
        }

       
       
    }

    public void HideInfo()
    {
        panel.style.display = DisplayStyle.None;
        if (tower != null)
        {
            rangeScript range = tower.GetComponentInChildren<rangeScript>();
            if (range != null)
            {
                range.HideVisual();
            }
        }

    }

    public bool IsPointerOverUIToolkit()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 uiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
        var picked = GetComponent<UIDocument>().rootVisualElement.panel.Pick(uiPos);
        return picked != null;
    }

    void SellTower()
    {
        if (tower == null) { return; }
        int sellValue = (int)(tower.value / 1.25f);
        gameUI.AddMoney(sellValue);
        Destroy(tower.gameObject);
        HideInfo();
    }
}
