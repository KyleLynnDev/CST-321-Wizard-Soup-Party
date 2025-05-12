using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public Sprite normalBackground;
    public Sprite selectedBackground;
    private Image buttonImage;
    public TextMeshProUGUI buttonText;
    [TextArea] public string buttonLabel;
    
    [Header("Font Sizes")]
    public float normalFontSize = 36f;
    public float selectedFontSize = 48f;
    
    private Button button;
    private bool isSelected = false;
    
    void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        buttonText.text = buttonLabel;
        normalBackground = buttonImage.sprite;
        AdjustButtonWidth();
    }
    
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        // Initialize with normal state
        buttonText.fontSize = normalFontSize;
        buttonImage.sprite = normalBackground;
    }
    
    void AdjustButtonWidth()
    {
        float textWidth = buttonText.preferredWidth;
        float padding = 40f;
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(textWidth + padding, rt.sizeDelta.y);
    }
    
    // These are called automatically by the EventSystem
    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        buttonText.fontSize = selectedFontSize;
        if (buttonImage != null && selectedBackground != null)
        {
            buttonImage.sprite = selectedBackground;
        }
        Debug.Log($"Button {buttonLabel} selected");
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        buttonText.fontSize = normalFontSize;
        if (buttonImage != null && normalBackground != null)
        {
            buttonImage.sprite = normalBackground;
        }
        Debug.Log($"Button {buttonLabel} deselected");
    }
    
    // Add these to handle mouse interactions
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Force select this button when mouse hovers
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // Only deselect if this button is the selected one
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    
    void OnButtonClick()
    {
        switch (buttonLabel.ToUpper())
        {
            case "CONTINUE":
                Debug.Log("Continue button clicked");
                break;
            case "RESTART":
                Debug.Log("Restart button clicked");
                break;
            case "OPTIONS":
                Debug.Log("Options button clicked");
                break;
            case "QUIT":
                Debug.Log("Quit button clicked");
    #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
    #else
                Application.Quit();
    #endif
                break;
        }
    }
}