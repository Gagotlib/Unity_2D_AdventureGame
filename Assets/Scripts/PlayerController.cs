using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    [Header("Movement")]
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float movementSpeed = 4.0f;

    // Variables related to player health
    [Header("Health")]
    public int Health { get { return currentHealth; } }
    public int maxHealth = 5;
    int currentHealth;
    [Header("Healing")]
    public float healDuration = 2.0f;
    bool isHealing = false;
    float healCooldown;


    // Variables related to temporary invincibility
    [Header("Invincibility")]
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
        if (isHealing)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown < 0)
            {
                isHealing = false;
            }
        }
    }

    // Called at fixed intervals. Used for physics updates.
    void FixedUpdate()
    {

        Vector2 position = (Vector2)rigidbody2d.position + movementSpeed * Time.deltaTime * move;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
        }

        if (amount > 0)
        {
            if (currentHealth == maxHealth)
            {
                return;
            }
            if (isHealing)
            {
                return;
            }
            isHealing = true;
            healCooldown = healDuration;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
