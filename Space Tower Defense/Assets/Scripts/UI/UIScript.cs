using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{
    Label healthLabel;
    Label moneyLabel;
    Label waveLabel;

    [SerializeField] Tower[] towers;

    List<Button> towerSlots = new List<Button>();

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
        VisualElement root = _document.rootVisualElement;
        healthLabel = root.Q<Label>("Health");
        moneyLabel = root.Q<Label>("Money");
        waveLabel = root.Q<Label>("Wave");
        mainHotbar = root.Q<VisualElement>("Basic");
        specialHotbar = root.Q<VisualElement>("Specials");
        Button swapButton = root.Q<Button>("Swap");

        specialHotbar.style.display = DisplayStyle.None;

        for (int i = 1; i <= 14; i++)
        {
            Button currentSlot;
            VisualElement towerIcon;
            if (i > 7)
            {
                currentSlot = specialHotbar.Q<Button>("Tower" + (i - 7));
                towerIcon = specialHotbar.Q<VisualElement>("TowerVisual" + (i - 7));
            }
            else
            {
                currentSlot = mainHotbar.Q<Button>("Tower" + i);
                towerIcon = mainHotbar.Q<VisualElement>("TowerVisual" + i);
            }

            if (currentSlot == null) { Debug.LogWarning($"Tower{i} not found"); continue; }

            
            towerIcon.style.backgroundImage = towers[i - 1].towerIcon;

            towerSlots.Add(currentSlot);
            
        }


        swapButton.RegisterCallback<ClickEvent>(evt => SwapTowers());

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
        }
    }

    public void AddMoney(int amount)
    {
        if (moneyLabel != null)
        {
            int currentMoney = int.Parse(moneyLabel.text);
            currentMoney += amount;
            moneyLabel.text = currentMoney.ToString();
        }
    }

    public int GetMoney()
    {
        if (moneyLabel != null)
        {
            int currentMoney = int.Parse(moneyLabel.text);
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

}
