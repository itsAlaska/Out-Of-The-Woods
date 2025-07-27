using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Transform visualTransform;

    private Rigidbody2D rb;
    private Vector2 moveInput = Vector2.zero;

    private PlayerInputActions inputActions;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = visualTransform.GetComponent<Animator>();
        inputActions = new PlayerInputActions();
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;

        inputActions.Disable();
    }

    private void Update()
    {
        // Use magnitude to determine if player is moving
        float speed = moveInput.sqrMagnitude;
        animator.SetFloat("Speed", speed);
        
        // Flip the sprite based on horizontal input
        if (moveInput.x != 0)
        {
            Vector3 scale = visualTransform.localScale;
            scale.x = -Mathf.Sign(moveInput.x) * Mathf.Abs(scale.x);
            visualTransform.localScale = scale;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * playerStats.moveSpeed;
    }
}