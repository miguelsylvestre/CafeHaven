using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    Rigidbody2D rb;
    Vector2 moveInput;
    public Animator animator;
    public SpriteRenderer rend;

    public static PlayerMovement Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        moveInput = Vector2.zero;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveInput.x += 1;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveInput.x -= 1;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveInput.y += 1;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveInput.y -= 1;
        moveInput = moveInput.normalized;

        if (animator != null)
        {
            if (moveInput != Vector2.zero)
            {
                animator.SetBool("Moving", true);
                animator.SetFloat("MoveX", moveInput.x);
                animator.SetFloat("MoveY", moveInput.y);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}