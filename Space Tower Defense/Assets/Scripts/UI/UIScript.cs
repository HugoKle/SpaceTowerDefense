using UnityEngine;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{
    Label healthLabel;
    Label moneyLabel;
    Label waveLabel;

    private UIDocument _document;
    DamageVignette damageVignette;

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




    }



    public void ReduceHealth(float amount)
    {
        if (healthLabel != null)
        {
            float currentHealth = float.Parse(healthLabel.text);
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

    public void AddMoney(float amount)
    {
        if (moneyLabel != null)
        {
            float currentMoney = float.Parse(moneyLabel.text);
            currentMoney += amount;
            moneyLabel.text = currentMoney.ToString();
        }
    }

    public float GetMoney()
    {
        if (moneyLabel != null)
        {
            float currentMoney = float.Parse(moneyLabel.text);
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
