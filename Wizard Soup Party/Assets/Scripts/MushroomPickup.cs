using UnityEngine;

public enum MushroomType { Jump, Dash, Glide, Climb }

public class MushroomPickup : MonoBehaviour
{
    public MushroomType type;
    public bool isSafe = true;
    public float powerAmount = 0.5f;

    public void Collect(playerController player)
    {
        switch (type)
        {
            case MushroomType.Jump:
                player.ModifyJumpHeight(isSafe ? powerAmount : -powerAmount);
                break;
            case MushroomType.Dash:
                player.ModifyDashSpeed(isSafe ? powerAmount : -powerAmount);
                break;
            case MushroomType.Glide:
                player.ModifyGlideControl(isSafe ? powerAmount : -powerAmount);
                break;
            case MushroomType.Climb:
                player.ModifyClimbStrength(isSafe ? powerAmount : -powerAmount);
                break;
        }

        // Optionally: play sound, VFX
        Destroy(gameObject);
    }
}
