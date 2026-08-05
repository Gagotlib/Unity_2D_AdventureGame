using UnityEngine;

public class HealingZone : MonoBehaviour
{
    // Variables related to the healing zone
    [Header("Healing Zone")]
    public int healingAmount = 1;

    // This function is called when another collider enters the trigger
    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(healingAmount);
        }
    }
}
