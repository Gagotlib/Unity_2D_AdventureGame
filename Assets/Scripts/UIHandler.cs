using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIHandler : MonoBehaviour
{
    private VisualElement m_HealthBar;

    public static UIHandler Instance { get; private set; }

    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;
    private VisualElement m_GameOverScreen;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_HealthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;

        m_GameOverScreen = uiDocument.rootVisualElement.Q<VisualElement>("GameOverScreen");
        if (m_GameOverScreen != null)
        {
            m_GameOverScreen.style.display = DisplayStyle.None;
            Button restartButton = m_GameOverScreen.Q<Button>("RestartButton");
            if (restartButton != null)
            {
                restartButton.clicked += RestartGame;
            }
        }
    }

    public void SetHealthValue(float percentage)
    {
        m_HealthBar.style.width = Length.Percent(percentage * 100);
    }

    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }
    public void DisplayDialogue(string dialogue)
    {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;

        Label dialogueLabel = m_NonPlayerDialogue.Q<Label>();
        dialogueLabel.text = dialogue;
    }

    public void ShowGameOver(bool show)
    {
        if (m_GameOverScreen != null)
        {
            m_GameOverScreen.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}

