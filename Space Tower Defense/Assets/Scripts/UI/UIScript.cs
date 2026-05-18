using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{
    Label healthLabel;
    Label moneyLabel;
    Label waveLabel;

    [SerializeField] public Tower[] towers;
    [SerializeField] int startingCash;

    List<Button> towerSlots = new List<Button>();
    TowerController towerController;
    InfoScript info;

    private UIDocument _document;
    DamageVignette damageVignette;

    VisualElement specialHotbar;
    VisualElement mainHotbar;
    

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        towerController = FindFirstObjectByType<TowerController>();
        info = FindFirstObjectByType<InfoScript>();

        VisualElement root = _document.rootVisualElement;
        healthLabel = root.Q<Label>("Health");
        moneyLabel = root.Q<Label>("Money");
        waveLabel = root.Q<Label>("Wave");
        mainHotbar = root.Q<VisualElement>("Basic");
        specialHotbar = root.Q<VisualElement>("Specials");
        Button swapButton = root.Q<Button>("Swap");
        Button speedButton = root.Q<Button>("SpeedButton");

        specialHotbar.style.display = DisplayStyle.None;


      
        swapButton.RegisterCallback<ClickEvent>(evt => SwapTowers());
        speedButton.RegisterCallback<ClickEvent>(evt => Time.timeScale = Time.timeScale < 2f ? 3f : 1f);

       

        AddMoney(startingCash);

        AssignSlots();
      
    }

    public void Hide()
    {
        _document.rootVisualElement.style.display = DisplayStyle.None;
    }
    void AssignSlots()
    {
        for (int i = 1; i <= 14; i++)
        {
            Button currentSlot;
            VisualElement towerIcon;
            Label towerText;
            if (i > 7)
            {
                currentSlot = specialHotbar.Q<Button>("Tower" + (i - 7));
                towerIcon = specialHotbar.Q<VisualElement>("TowerVisual" + (i - 7));
                towerText = specialHotbar.Q<Label>("TowerText" + (i - 7));
            }
            else
            {
                currentSlot = mainHotbar.Q<Button>("Tower" + i);
                towerIcon = mainHotbar.Q<VisualElement>("TowerVisual" + i);
                towerText = mainHotbar.Q<Label>("TowerText" + i);

            }

            if (currentSlot == null) { Debug.LogWarning($"Tower{i} not found"); continue; }

            if (towers[i - 1].towerPrefab == null)
            {
                Debug.LogWarning($"Tower {i} has no prefab assigned!");
                continue;
            }

            towerIcon.style.backgroundImage = towers[i - 1].towerIcon;

            towerSlots.Add(currentSlot);

            int index = i - 1;
            TowerScript towerScript = towers[index].towerPrefab.GetComponent<TowerScript>();

            currentSlot.RegisterCallback<ClickEvent>(evt => OnTowerSlotClicked(index));
            currentSlot.RegisterCallback<MouseEnterEvent>(evt => info.ShowInfo(towerScript));
            currentSlot.RegisterCallback<MouseLeaveEvent>(evt => info.HideInfo());

            
            if (towerScript != null)
            {
                towerText.text = "$" + towerScript.towerLevels[0].Price;
            }
        }
    }

    void OnTowerSlotClicked(int index)
    {
        Debug.Log($"Clicked slot {index}, tower: {towers[index].towerName}");
        if (towerController == null)
        {
            towerController = FindFirstObjectByType<TowerController>();
        }
        towerController.PlaceTower(towers[index].towerPrefab, index);
    }


    bool isShowingBasic = true;
    void SwapTowers()
    {
        if (isShowingBasic)
        {
            mainHotbar.style.display = DisplayStyle.None;
            specialHotbar.style.display= DisplayStyle.Flex;
        }
        else
        {
            specialHotbar.style.display = DisplayStyle.None;
            mainHotbar.style.display = DisplayStyle.Flex;
        }

        isShowingBasic = !isShowingBasic;

    }

    public void ReduceHealth(int amount)
    {
        if (healthLabel != null)
        {
            int currentHealth = int.Parse(healthLabel.text);
            currentHealth -= amount;
            healthLabel.text = currentHealth.ToString();
            if (damageVignette != null)
            {
                damageVignette.ShowDamageVignette();
            }
            else
            {
                damageVignette = FindFirstObjectByType<DamageVignette>();
                if (damageVignette != null)
                {
                    damageVignette.ShowDamageVignette();
                }
            }

            if (currentHealth <= 0)
            {
                LoseScript loseScript = FindFirstObjectByType<LoseScript>();
                if (loseScript != null)
                {
                    loseScript.ShowLoseScreen();
                }
            }
        }
    }

    public int GetHealth()
    {
        if (healthLabel != null)
        {
            int currentHealth = int.Parse(healthLabel.text);
            return currentHealth;
        }
        return 0;
    }

    public void AddMoney(int amount)
    {
        if (moneyLabel != null)
        {
            int currentMoney = int.Parse(moneyLabel.text.Replace("$", ""));
            currentMoney += amount;
            moneyLabel.text = "$" + currentMoney.ToString();
        }
    }

    public int GetMoney()
    {
        if (moneyLabel != null)
        {
            int currentMoney = int.Parse(moneyLabel.text.Replace("$", ""));
            return currentMoney;
        }
        return 0;
    }

    public void SetWave(int wave)
    {
        if (waveLabel != null)
        {
            waveLabel.text = "Wave: " + wave;
        }
    }

    public int GetWave()
    {
        if (waveLabel != null)
        {
            string waveText = waveLabel.text.Replace("Wave: ", "");
            if (int.TryParse(waveText, out int currentWave))
            {
                return currentWave;
            }
        }
        return 0;
    }

    public bool IsPointerOverUIToolkit()
    {
        // osäker
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 uiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);

        VisualElement picked = mainHotbar.panel.Pick(uiPos);

        if (picked == null) return false;

        VisualElement current = picked;
        while (current != null)
        {
            if (current == mainHotbar || current == specialHotbar)
                return true;
            current = current.parent;
        }

        return false;
        // ------
    }

}
