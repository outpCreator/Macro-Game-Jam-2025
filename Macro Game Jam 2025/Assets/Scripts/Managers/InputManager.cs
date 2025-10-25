using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    // Movement
    [HideInInspector] public Vector2 moveInput;

    // Jump
    [HideInInspector] public bool jumpStarted;
    [HideInInspector] public bool jumpCanceled;
    [HideInInspector] public bool jumpHeld;

    // Interaction
    [HideInInspector] public bool interact;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            jumpStarted = true;
            jumpHeld = true;
            jumpCanceled = false;
        }
        else if (ctx.canceled)
        {
            jumpCanceled = true;
            jumpHeld = false;
        }
        else if (ctx.performed)
        {
            jumpHeld = true;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        interact = ctx.performed;
        Debug.Log("Interact input: " + interact);
    }

    public void ConsumeFrameInputs()
    {
        jumpStarted = false;
        jumpCanceled = false;
    }
}
