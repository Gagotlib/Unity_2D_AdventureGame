using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    Animator animator;

    [Header("Movement")]
    public float enemySpeed = 3.0f;
    Rigidbody2D rigidbody2d;

    [Header("Direction")]
    public bool vertical;
    int direction = 1;

    [Header("Random Movement")]
    public bool randomMovement;

    [Header("Patrol Time")]
    public float changeTime = 1.7f;
    float timer;

    bool broken = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (randomMovement)
            {
                // Randomly choose one of the four directions (Up, Down, Right, Left)
                int rand = Random.Range(0, 4);
                switch (rand)
                {
                    case 0: // Up
                        vertical = true;
                        direction = 1;
                        break;
                    case 1: // Down
                        vertical = true;
                        direction = -1;
                        break;
                    case 2: // Right
                        vertical = false;
                        direction = 1;
                        break;
                    case 3: // Left
                        vertical = false;
                        direction = -1;
                        break;
                }
            }
            else
            {
                direction = -direction;
            }
            timer = changeTime;
        }
    }


    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y += enemySpeed * Time.deltaTime * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x += enemySpeed * Time.deltaTime * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        rigidbody2d.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
    }
}
