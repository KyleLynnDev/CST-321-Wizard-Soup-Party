using UnityEngine;

public enum MushroomType { JumpHeight, JumpNumber, Dash, Glide, Mana }

public class MushroomPickup : MonoBehaviour
{
    public MushroomType type;
    public bool isSafe = true;
    public float powerAmount = 0.5f;

    public void Collect(playerController player)
    {
        switch (type)
        {
            case MushroomType.JumpHeight:
                player.ModifyJumpHeight(isSafe ? powerAmount : -powerAmount);
                break;
            case MushroomType.JumpNumber:
                //add double jump stuff here 
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
