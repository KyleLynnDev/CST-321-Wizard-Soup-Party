using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SoundButton : MonoBehaviour,
                           IPointerEnterHandler,
                           IPointerExitHandler
{
    [Header("Button References")]
    public Button button;              // Reference to the Button component
    public Image buttonImage;          // The Image component that displays the sprite
    
    [Header("Button Sprites")]
    public Sprite soundOnSprite;       // Sprite to show when sound is on
    public Sprite soundOffSprite;      // Sprite to show when sound is off
    
    [Header("Audio Settings")]
    [Range(0f, 1f)]
    public float soundOnVolume = 1f;   // Volume when sound is on
    public float soundOffVolume = 0f;  // Volume when sound is off
    
    private bool isSoundOn = true;     // Track sound state
    
    void Start()
    {
        // Initialize references if not set
        if (button == null)
            button = GetComponent<Button>();
            
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
            
        // Set up the click handler
        button.onClick.AddListener(ToggleSound);
        
        // Initialize visual state
        UpdateButtonAppearance();
    }
    
    // Toggle sound on/off
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        
        // Update the master volume
        AudioListener.volume = isSoundOn ? soundOnVolume : soundOffVolume;
        
        // Update button appearance
        UpdateButtonAppearance();
        
        // Log the change
        Debug.Log("Sound is now " + (isSoundOn ? "ON" : "OFF"));

        Cursor.visible = true;
    }
    
    // Update the button's appearance based on sound state
    void UpdateButtonAppearance()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Slightly enlarge the button on hover
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // Return to normal size
        transform.localScale = Vector3.one;
    }
}