using UnityEngine;


public class HealthCollectible : MonoBehaviour
{
    //Variables
    public int healthAmount = 1;

    public AudioClip collectedClip;

    public ParticleSystem healthEffect;

    // This function is called when another collider enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.Health < player.maxHealth)
        {
            player.ChangeHealth(healthAmount);
            Destroy(gameObject);
            player.PlaySound(collectedClip);
            Instantiate(healthEffect, transform.position, Quaternion.identity);
        }

    }

}