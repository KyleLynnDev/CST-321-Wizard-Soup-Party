using UnityEngine;


public enum PickupType { Mushroom, Ingredient}

public enum MushroomType { JumpHeight, JumpNumber, Dash, Glide, Mana }



public class PickupItem : MonoBehaviour
{
    public MushroomType type;
    public bool isSafe = true;
    public float powerAmount = 0.5f;

    [Header("Mushroom Properties")]
    public MushroomType mushroomType;

    [Header("Ingredient Properties")]
    public string ingredientName;

    public PickupType pickupType = PickupType.Mushroom;


    public void CollectIngredient()
    {
        GameData.Instance.CollectIngredient(ingredientName);
        UIManager.Instance.ShowUpgradePopup($"{ingredientName} Acquired!");
        Destroy(gameObject);
    }


    public void CollectMushroom(playerController player)
    {
        switch (type)
        {
            case MushroomType.JumpHeight:
                player.ModifyJumpHeight(isSafe ? powerAmount : -powerAmount);
                break;
            case MushroomType.JumpNumber:
                player.ModifyJumpCount(isSafe ? 1 : -1);
                break;
            case MushroomType.Dash:
                player.ModifyDashSpeed(isSafe ? powerAmount : -powerAmount);
                break;
            case MushroomType.Glide:
                player.ModifyGlideControl(isSafe ? powerAmount : -powerAmount);
                break;
            case MushroomType.Mana:
                player.ModifyManaStrength(isSafe ? 1 : -1);
                break;
        }

        Debug.Log($"Collected {type} mushroom. Safe: {isSafe}");

        // TODO: play sound, VFX
        Destroy(gameObject);
    }


}
