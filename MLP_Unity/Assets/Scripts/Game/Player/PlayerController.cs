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

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference lookAction;
    public InputActionReference clickAction;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.1f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float pitch = 0f;
    private bool cursorLocked = false;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        if (SceneManager.GetActiveScene().name == "Pyramid Screen")
        {
            cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        lookAction.action.Enable();
        clickAction.action.Enable();

        clickAction.action.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        lookAction.action.Disable();
        clickAction.action.Disable();

        clickAction.action.performed -= OnClickPerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        if (SceneManager.GetActiveScene().name == "Pyramid Screen")
        {
            return;
        }

        if (!cursorLocked)
        {
            SetCursorLocked(true);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Pyramid Screen")
        {
            if (cursorLocked && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                SetCursorLocked(false);
            }

            if (cursorLocked)
            {
                HandleMouseLook();
            }
        }

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer)
        {
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

        Vector2 input = moveAction.action.ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, 0, input.y);

        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;

        // Impede a câmera de fazer o jogador andar para cima/baixo
        move.y = 0f;

        if (groundedPlayer && jumpAction.action.WasPressedThisFrame())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;

        controller.Move(finalMove * Time.deltaTime);
    }

    private void SetCursorLocked(bool locked)
    {
        cursorLocked = locked;

        Cursor.lockState = locked
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !locked;
    }

    private void HandleMouseLook()
    {
        Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();

        float mouseX = lookDelta.x * mouseSensitivity;
        float mouseY = lookDelta.y * mouseSensitivity;

        // Rotação horizontal do jogador
        transform.Rotate(Vector3.up * mouseX);

        // Rotação vertical da câmera
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void GrabbedBox()
    {
        playerSpeed = 1.25f;
        jumpHeight = 0f;
    }

    public void ReleasedBox()
    {
        playerSpeed = 5f;
        jumpHeight = 1.5f;
    }
}