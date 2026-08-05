using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
    }

    // Called at fixed intervals. Used for physics updates.
    void FixedUpdate()
    {
        float movementSpeed = 4.0f;
        Vector2 position = (Vector2)rigidbody2d.position + movementSpeed * Time.deltaTime * move;
        rigidbody2d.MovePosition(position);
    }
}
