using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LoseScript : MonoBehaviour
{
    VisualElement panel;
    UIDocument uiDocument;
    VisualElement root;

    Label stats;
    

    void Start()
    {
        LevelExit exit = FindFirstObjectByType<LevelExit>();
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        panel = root.Q<VisualElement>("Bg");

        Button retryButton = root.Q<Button>("Restart");
        retryButton.RegisterCallback<ClickEvent>(evt => Time.timeScale = 1f);
        retryButton.RegisterCallback<ClickEvent>(evt => exit.StartLevelCoroutine((Level)currentLevel));

        Button mainMenuButton = root.Q<Button>("Return");
        mainMenuButton.RegisterCallback<ClickEvent>(evt => Time.timeScale = 1f);
        mainMenuButton.RegisterCallback<ClickEvent>(evt => exit.StartLevelCoroutine(Level.Start));

        stats = root.Q<Label>("Stats");

        panel.style.display = DisplayStyle.None;
    }

    IEnumerator showScreen()
    {
        panel.style.display = DisplayStyle.Flex;
        panel.style.opacity = 0f;
        while (panel.style.opacity.value < 0.99f)
        {
            panel.style.opacity = Mathf.Lerp(panel.style.opacity.value, 1f, Time.deltaTime * 2f);
            yield return null;
        }
        Time.timeScale = 0f;
    }

    public void ShowLoseScreen()
    {
        StartCoroutine(showScreen());
        UpdateStats();
    }

    void UpdateStats()
    {
        UIScript ui = FindFirstObjectByType<UIScript>();
        stats.text = $"wave: {ui.GetWave()}\n" +
            $"money: ${ui.GetMoney()}";
        ui.Hide();
    }


}
