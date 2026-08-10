using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

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

    // Variables related to the player character's animator
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Projectile launching
    [Header("Projectile Launching")]
    public InputAction launchAction;
    public GameObject projectilePrefab;
    public float launchForce = 300.0f;
    [Header("Shoot Time")]
    public float shootTime = 0.5f;
    float shootTimer;
    bool isShooting;

    [Header("Talk")]
    public InputAction talkAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        talkAction.Enable();
        launchAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

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

        if (isShooting)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer < 0)
            {
                isShooting = false;
            }
        }

        if (launchAction.triggered)
        {
            Launch();
        }


        if (talkAction.triggered)
        {
            FindFriend();
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
            animator.SetTrigger("Hit");
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
        UIHandler.Instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        if (isShooting)
        {
            return;
        }
        isShooting = true;
        shootTimer = shootTime;
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, launchForce);
        animator.SetTrigger("Launch");
    }
    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        Debug.Log("Look direction: " + moveDirection);

        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
                UIHandler.Instance.DisplayDialogue(character.dialogue);
            }
        }
    }
}
