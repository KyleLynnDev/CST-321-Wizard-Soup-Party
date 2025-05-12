using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; } 

    [Header("References")]
    public GameObject mushroomBookPanel;
    public GameObject inventoryPanel; 

    [Header("Interaction Prompt")]
    public GameObject interactionPromptRoot; // UI panel or object
    public TextMeshProUGUI interactionPromptText; // The TMP text component inside it

    public bool isBookOpen = true; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void Start(){
         if (mushroomBookPanel != null)
            mushroomBookPanel.SetActive(!mushroomBookPanel.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMushroomBook()
    {
        if (mushroomBookPanel == null) return;

        bool isActive = mushroomBookPanel.activeSelf;
        mushroomBookPanel.SetActive(!isActive);
        isBookOpen = !isActive; 
    }


     public void ShowInteractionPrompt(string message)
    {
        if (interactionPromptRoot != null && interactionPromptText != null)
        {
            interactionPromptText.text = message;
            interactionPromptRoot.SetActive(true);
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPromptRoot != null)
            interactionPromptRoot.SetActive(false);
    }


    public bool IsBookOpen(){
        return isBookOpen; 
    }

    
    public void FlagDashSuppression()
    {
        dashSuppressedThisFrame = true;
    }
    public bool ConsumeDashSuppression()
    {
        if (dashSuppressedThisFrame)
        {
            dashSuppressedThisFrame = false;
            return true;
        }
        return false;
    }

    private bool dashSuppressedThisFrame = false;




}
