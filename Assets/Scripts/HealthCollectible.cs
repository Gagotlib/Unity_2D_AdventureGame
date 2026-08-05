using UnityEngine;


public class HealthCollectible : MonoBehaviour
{
    //Variables
    public int healthAmount = 1;

    // This function is called when another collider enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.Health < player.maxHealth)
        {
            player.ChangeHealth(healthAmount);
            Destroy(gameObject);
        }
    }

}