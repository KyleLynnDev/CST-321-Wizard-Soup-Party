using UnityEngine;
using TMPro;
using UnityEngine.UI; 

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
    private bool dashSuppressedThisFrame = false;

        [Header("Mana UI")]
    public Transform manaBarContainer;
    public GameObject manaStarPrefab;
    private Image[] manaStars;

        [Header("Upgrade Stats")]
    public TextMeshProUGUI glideText;
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI dashText;


        [Header("Upgrade Popup")]
    public TextMeshProUGUI upgradePopupText;

   




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


    public void SetupManaUI(int maxMana)
    {
        foreach (Transform child in manaBarContainer)
        {
            Destroy(child.gameObject);
        }

        manaStars = new Image[maxMana];

        for (int i = 0; i < maxMana; i++)
        {
            GameObject star = Instantiate(manaStarPrefab, manaBarContainer);
            manaStars[i] = star.GetComponent<Image>();
        }
    }

    public void UpdateManaUI(int currentMana)
    {
        if (manaStars == null) return;

        for (int i = 0; i < manaStars.Length; i++)
        {
            manaStars[i].enabled = i < currentMana;
        }
    }


    public void ShowUpgradePopup(string message)
    {
        if (upgradePopupText == null) return;

        upgradePopupText.text = message;
        upgradePopupText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideUpgradePopup));
        Invoke(nameof(HideUpgradePopup), 2f);
    }
    

    private void HideUpgradePopup()
    {
        if (upgradePopupText != null)
            upgradePopupText.gameObject.SetActive(false);
    }




}
