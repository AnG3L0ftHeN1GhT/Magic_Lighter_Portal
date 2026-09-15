using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    private bool grabbing;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference lookAction;
    public InputActionReference clickAction;

    [Header("Look")]
    public float mouseSensitivity = 0.1f;
    public float controllerSensitivity = 120f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float pitch = 0f;
    private bool cursorLocked = false;

    // Variáveis para guardar os valores dos Callbacks
    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void OnEnable()
    {
        // Habilita as ações
        moveAction.action.Enable();
        jumpAction.action.Enable();
        lookAction.action.Enable();
        clickAction.action.Enable();

        // Inscreve os Callbacks de cada ação
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;

        lookAction.action.performed += OnLookPerformed;

        jumpAction.action.performed += OnJumpPerformed;

        clickAction.action.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        // Cancela a inscrição dos Callbacks
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;

        lookAction.action.performed -= OnLookPerformed;

        jumpAction.action.performed -= OnJumpPerformed;

        clickAction.action.performed -= OnClickPerformed;

        // Desabilita as ações
        moveAction.action.Disable();
        jumpAction.action.Disable();
        lookAction.action.Disable();
        clickAction.action.Disable();
    }

    #region Input Callbacks

    public void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

public void OnLookPerformed(InputAction.CallbackContext ctx)
{
    lookInput = ctx.ReadValue<Vector2>();
}

public void OnLookCanceled(InputAction.CallbackContext ctx)
{
    lookInput = Vector2.zero;
}
    public void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        // Realiza o pulo apenas se estiver no chão
        if (groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        if (SceneManager.GetActiveScene().name == "Pyramid Screen")
        {
            return;
        }

        if (!cursorLocked)
        {
            // SetCursorLocked(true);
        }
    }

    #endregion

private void Update()
{
    if (!grabbing)
    {
        HandleMouseLook();
    }

    groundedPlayer = controller.isGrounded;

    if (groundedPlayer && playerVelocity.y < -2f)
    {
        playerVelocity.y = -2f;
    }

    Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

    if (cameraTransform != null)
    {
        move = cameraTransform.forward * move.z
             + cameraTransform.right * move.x;

        move.y = 0f;
    }

    playerVelocity.y += gravityValue * Time.deltaTime;

    Vector3 finalMove =
        move * playerSpeed + Vector3.up * playerVelocity.y;

    controller.Move(finalMove * Time.deltaTime);
}

private void HandleMouseLook()
{
    transform.Rotate(
        Vector3.up * lookInput.x * 100f * Time.deltaTime
    );

    if (cameraTransform != null)
    {
        cameraTransform.Rotate(
            Vector3.right * -lookInput.y * 100f * Time.deltaTime
        );
    }
}
    public void GrabbedBox()
    {
        playerSpeed = 1.25f;
        jumpHeight = 0f;
        grabbing = true;
    }

    public void ReleasedBox()
    {
        playerSpeed = 5f;
        jumpHeight = 1.5f;
        grabbing = false;
    }

    public void DesativarCamera()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (SceneManager.GetActiveScene().name == "Pyramid Screen")
        {
            cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}