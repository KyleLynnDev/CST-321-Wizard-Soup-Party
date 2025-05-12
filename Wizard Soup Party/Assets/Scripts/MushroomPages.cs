using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class MushroomBookPages : MonoBehaviour
{
    public Image pageDisplay;
    public List<Sprite> pageImages;

    private int currentPage = 0;
    private bool isActive = false;
    private PlayerInputActions inputActions;

    public InputActionReference flipLeftAction;
    public InputActionReference flipRightAction;        

    private void OnEnable()
    {
        flipLeftAction.action.Enable();
        flipRightAction.action.Enable();

        flipLeftAction.action.performed += OnFlipLeft;
        flipRightAction.action.performed += OnFlipRight;

        currentPage = 0;
        UpdatePage();
    }

    private void OnDisable()
    {
        flipLeftAction.action.performed -= OnFlipLeft;
        flipRightAction.action.performed -= OnFlipRight;

        flipLeftAction.action.Disable();
        flipRightAction.action.Disable();
    }

    private void OnFlipLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Flip left triggered");
        if (!UIManager.Instance.IsBookOpen()) return;
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }

        UIManager.Instance.FlagDashSuppression();
    }

    private void OnFlipRight(InputAction.CallbackContext context)
    {
        Debug.Log("Flip right triggered");
        if (!UIManager.Instance.IsBookOpen()) return;
        if (currentPage < pageImages.Count - 1)
        {
            currentPage++;
            UpdatePage();
        }

        UIManager.Instance.FlagDashSuppression();
    }

    private void UpdatePage()
    {
                    Debug.Log("Current Page: " + currentPage);
                    Debug.Log("Sprite: " + pageImages[currentPage]?.name);
        pageDisplay.sprite = pageImages[currentPage];
    }
}
