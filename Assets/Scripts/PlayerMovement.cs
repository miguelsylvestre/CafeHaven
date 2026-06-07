using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public static bool IsInMenu = false;
    Rigidbody2D rb;
    Vector2 moveInput;
    public Animator animator;
    public SpriteRenderer rend;
    private IInteractable currentInteractable;

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
        if (IsInMenu)
        {
            moveInput = Vector2.zero;
            if (animator != null)
            {
                animator.SetBool("Moving", false);
            }
            return;
        }


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
        if (currentInteractable != null)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                currentInteractable.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == null || !interactable.CanInteract()) return;

        currentInteractable = interactable;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}