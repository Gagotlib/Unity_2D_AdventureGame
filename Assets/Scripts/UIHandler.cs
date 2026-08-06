using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIHandler : MonoBehaviour
{
    VisualElement m_HealthBar;

    public static UIHandler Instance { get; private set; }


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
    }

    public void SetHealthValue(float percentage)
    {
        m_HealthBar.style.width = Length.Percent(percentage * 100);
    }
}
