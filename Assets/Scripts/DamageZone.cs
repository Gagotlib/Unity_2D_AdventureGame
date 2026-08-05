using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Variables related to the damage zone
    [Header("Damage Zone")]
    public int damageAmount = 1;

    // This function is called when another collider stays in the trigger
    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-damageAmount);
        }
    }
}
