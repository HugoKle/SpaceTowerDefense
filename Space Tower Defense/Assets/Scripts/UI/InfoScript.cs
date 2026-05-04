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

        upgradeStats.text =  $"Damage: {tower.towerLevels[towerLevel + 1].towerDamage}\n" +
            $"Range: {tower.towerLevels[towerLevel + 1].towerRange}\n" +
            $"Fire Rate: {tower.towerLevels[towerLevel + 1].towerAttackSpeed}";

        upgradeButton.text = $"Upgrade (${tower.towerLevels[towerLevel + 1].Price})";
    }

    public void HideInfo()
    {
        panel.style.display = DisplayStyle.None;
    }

    public bool IsPointerOverUIToolkit()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 uiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
        var picked = GetComponent<UIDocument>().rootVisualElement.panel.Pick(uiPos);
        return picked != null;
    }
}
