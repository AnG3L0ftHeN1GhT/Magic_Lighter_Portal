using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject standardFlame;
    public GameObject goldenFlame;

    public float rayDistance = 3f;
    public float pickupSpeed = 8f;
    public float rotateSpeed = 100f;

    public Transform objectViewer;

    public UnityEvent OnView;
    public UnityEvent OnFinishView;

    public Item lighter;

    private Camera cam;

    private bool isViewing;
    private bool canFinish;

    private Interactables currentInteract;

    private Vector3 originPosition;
    private Quaternion originRotation;

    public InputActionReference leftClick;
    public InputActionReference rightClick;
    public InputActionReference interactBttn;
    public InputActionReference look;
    public InputActionAsset inputActions;

    // =========================================================
    // GRAB TO HAND
    // =========================================================

    [Header("Grab To Hand")]

    [Tooltip("Transform que representa a mão do player.")]
    public Transform handPosition;

    [Tooltip("Tecla usada para pegar/largar o objeto.")]
    public InputActionReference grabKey;

    [Tooltip("Velocidade de encaixe do objeto na mão.")]
    public float handMoveSpeed = 8f;

    private Interactables carriedItem;
    private Rigidbody carriedRigidbody;
    private Collider carriedCollider;

    private Coroutine handRoutine;
    private Coroutine movingRoutine;

    // =========================================================

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.FindActionMap("Player").Enable();
        }

        // Garante que a ação de grab está habilitada.
        if (grabKey != null && grabKey.action != null)
        {
            grabKey.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (grabKey != null && grabKey.action != null)
        {
            grabKey.action.Disable();
        }
    }

    private void Start()
    {
        cam = Camera.main;

        if (handPosition == null)
        {
            Debug.LogError(
                "PlayerInteraction: handPosition não foi configurado no Inspector!"
            );
        }

        if (grabKey == null)
        {
            Debug.LogError(
                "PlayerInteraction: grabKey não foi configurado no Inspector!"
            );
        }
    }

    private void Update()
    {
        CheckInteractables();
        CheckGrabInput();
    }

    // =========================================================
    // INTERACTION
    // =========================================================

    private void CheckInteractables()
    {
        if (isViewing)
        {
            if (currentInteract != null &&
                currentInteract.item != null &&
                currentInteract.item.grabbable)
            {
                RotateObject();
            }

            if (currentInteract != null &&
                canFinish &&
                currentInteract.item.stashable &&
                interactBttn.action.WasPressedThisFrame())
            {
                FinishView();

                inputActions.FindActionMap("Player").Enable();

                if (currentInteract.item == lighter)
                {
                    LighterFunction.instance.SetLighter(1);
                }
            }
            else if (canFinish &&
                     rightClick.action.WasPressedThisFrame())
            {
                FinishView();

                inputActions.FindActionMap("Player").Enable();
            }

            return;
        }

        if (cam == null)
            return;

        RaycastHit hit;

        Vector3 rayOrigin = cam.ViewportToWorldPoint(
            new Vector3(0.5f, 0.5f, 0.5f)
        );

        if (Physics.Raycast(
            rayOrigin,
            cam.transform.forward,
            out hit,
            rayDistance))
        {
            // IMPORTANTE:
            // Se o Collider estiver em um filho do objeto,
            // GetComponent<Interactables>() pode retornar null.
            Interactables interactable =
                hit.collider.GetComponentInParent<Interactables>();

            if (interactable != null)
            {
                UIManager.instance.SetInteractionCursor(true);

                if (leftClick.action.WasPressedThisFrame())
                {
                    if (interactable.isMoving)
                        return;

                    // Não permite visualizar um objeto que está na mão.
                    if (interactable == carriedItem)
                        return;

                    currentInteract = interactable;

                    if (currentInteract.item.papel)
                    {
                        MudarCorChama(goldenFlame);
                        return;
                    }

                    inputActions.FindActionMap("Player").Disable();

                    OnView.Invoke();

                    isViewing = true;
                    canFinish = false;

                    CancelInvoke(nameof(CanFinish));
                    Invoke(nameof(CanFinish), 1f);

                    if (currentInteract.item.grabbable)
                    {
                        originPosition =
                            currentInteract.transform.position;

                        originRotation =
                            currentInteract.transform.rotation;

                        StartMovingObject(
                            currentInteract,
                            objectViewer.position
                        );
                    }
                }
            }
            else
            {
                UIManager.instance.SetInteractionCursor(false);
            }
        }
        else
        {
            UIManager.instance.SetInteractionCursor(false);
        }
    }

    // =========================================================
    // GRAB INPUT
    // =========================================================

    private void CheckGrabInput()
    {
        // Não permite pegar/largar durante a visualização.
        if (isViewing)
            return;

        if (grabKey == null || grabKey.action == null)
            return;

        // -----------------------------------------------------
        // Já está segurando algo
        // -----------------------------------------------------

        if (carriedItem != null)
        {
            if (grabKey.action.WasPressedThisFrame())
            {
                DropItem();
            }

            return;
        }

        // -----------------------------------------------------
        // Procurar objeto para pegar
        // -----------------------------------------------------

        if (cam == null)
            return;

        RaycastHit hit;

        Vector3 rayOrigin = cam.ViewportToWorldPoint(
            new Vector3(0.5f, 0.5f, 0.5f)
        );

        if (!Physics.Raycast(
            rayOrigin,
            cam.transform.forward,
            out hit,
            rayDistance))
        {
            return;
        }

        // IMPORTANTE:
        // GetComponentInParent permite que o Collider esteja
        // em um objeto filho.
        Interactables interactable =
            hit.collider.GetComponentInParent<Interactables>();

        if (interactable == null)
            return;

        if (interactable.isMoving)
            return;

        if (interactable.item == null)
            return;

        if (!interactable.item.grabbable)
            return;

        if (grabKey.action.WasPressedThisFrame())
        {
            GrabItem(interactable);
        }
    }

    // =========================================================
    // PEGAR
    // =========================================================

    private void GrabItem(Interactables interactable)
    {
        if (interactable == null)
            return;

        if (handPosition == null)
        {
            Debug.LogError(
                "Não é possível pegar o item: handPosition não está configurado."
            );

            return;
        }

        if (carriedItem != null)
            return;

        // Cancela uma movimentação anterior desse objeto.
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
            movingRoutine = null;
        }

        if (handRoutine != null)
        {
            StopCoroutine(handRoutine);
            handRoutine = null;
        }

        carriedItem = interactable;

        // Procura o Rigidbody no objeto ou nos pais.
        carriedRigidbody =
            interactable.GetComponent<Rigidbody>();

        if (carriedRigidbody == null)
        {
            carriedRigidbody =
                interactable.GetComponentInParent<Rigidbody>();
        }

        // Procura o Collider no objeto ou nos filhos.
        carriedCollider =
            interactable.GetComponent<Collider>();

        if (carriedCollider == null)
        {
            carriedCollider =
                interactable.GetComponentInChildren<Collider>();
        }

        // Desliga física.
        if (carriedRigidbody != null)
        {
            carriedRigidbody.linearVelocity = Vector3.zero;
            carriedRigidbody.angularVelocity = Vector3.zero;

            carriedRigidbody.isKinematic = true;
            carriedRigidbody.useGravity = false;
        }

        // Desliga colisão para não bater no player.
        if (carriedCollider != null)
        {
            carriedCollider.enabled = false;
        }

        // Guarda a posição mundial antes de mudar o parent.
        Transform itemTransform = interactable.transform;

        Vector3 startPosition = itemTransform.position;
        Quaternion startRotation = itemTransform.rotation;

        // Agora o objeto passa a acompanhar a mão.
        itemTransform.SetParent(handPosition, true);

        // Move suavemente até a mão.
        handRoutine = StartCoroutine(
            MoveToHand(
                itemTransform,
                startPosition,
                startRotation
            )
        );

        Debug.Log(
            "Grabbed item: " + interactable.gameObject.name
        );
    }

    // =========================================================
    // MOVIMENTO ATÉ A MÃO
    // =========================================================

    private IEnumerator MoveToHand(
        Transform obj,
        Vector3 startPosition,
        Quaternion startRotation)
    {
        if (obj == null)
            yield break;

        float t = 0f;

        while (t < 1f)
        {
            if (obj == null)
                yield break;

            t += Time.deltaTime * handMoveSpeed;

            obj.position = Vector3.Lerp(
                startPosition,
                handPosition.position,
                t
            );

            obj.rotation = Quaternion.Slerp(
                startRotation,
                handPosition.rotation,
                t
            );

            yield return null;
        }

        if (obj != null)
        {
            obj.position = handPosition.position;
            obj.rotation = handPosition.rotation;
        }

        handRoutine = null;
    }

    // =========================================================
    // LARGAR
    // =========================================================

    private void DropItem()
    {
        if (carriedItem == null)
            return;

        if (handRoutine != null)
        {
            StopCoroutine(handRoutine);
            handRoutine = null;
        }

        Transform itemTransform = carriedItem.transform;

        // Remove da mão.
        itemTransform.SetParent(null, true);

        // Reativa collider.
        if (carriedCollider != null)
        {
            carriedCollider.enabled = true;
        }

        // Reativa física.
        if (carriedRigidbody != null)
        {
            carriedRigidbody.isKinematic = false;
            carriedRigidbody.useGravity = true;

            carriedRigidbody.linearVelocity = Vector3.zero;
            carriedRigidbody.angularVelocity = Vector3.zero;
        }

        Debug.Log(
            "Dropped item: " + carriedItem.gameObject.name
        );

        carriedItem = null;
        carriedRigidbody = null;
        carriedCollider = null;
    }

    // =========================================================
    // VISUALIZAÇÃO
    // =========================================================

    private void CanFinish()
    {
        canFinish = true;

        UIManager.instance.SetBackImage(true);
    }

    private void FinishView()
    {
        canFinish = false;
        isViewing = false;

        UIManager.instance.SetBackImage(false);

        if (currentInteract != null &&
            currentInteract.item != null &&
            currentInteract.item.grabbable)
        {
            currentInteract.transform.rotation = originRotation;

            StartMovingObject(
                currentInteract,
                originPosition
            );
        }

        OnFinishView.Invoke();
    }

    // =========================================================
    // MOVIMENTAÇÃO
    // =========================================================

    private void StartMovingObject(
        Interactables obj,
        Vector3 position)
    {
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }

        movingRoutine = StartCoroutine(
            MovingObject(obj, position)
        );
    }

    private IEnumerator MovingObject(
        Interactables obj,
        Vector3 position)
    {
        if (obj == null)
            yield break;

        obj.isMoving = true;

        float timer = 0f;

        while (timer < 1f)
        {
            if (obj == null)
                yield break;

            obj.transform.position = Vector3.Lerp(
                obj.transform.position,
                position,
                Time.deltaTime * pickupSpeed
            );

            timer += Time.deltaTime;

            yield return null;
        }

        if (obj != null)
        {
            obj.transform.position = position;
            obj.isMoving = false;
        }

        movingRoutine = null;
    }

    // =========================================================
    // ROTAÇÃO
    // =========================================================

    private void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        currentInteract.transform.Rotate(
            cam.transform.up,
            -Mathf.Deg2Rad * x * rotateSpeed,
            Space.World
        );

        currentInteract.transform.Rotate(
            cam.transform.right,
            -Mathf.Deg2Rad * y * rotateSpeed,
            Space.World
        );
    }

    // =========================================================
    // CHAMA
    // =========================================================

    public void MudarCorChama(GameObject corDesejada)
    {
        standardFlame.SetActive(false);
        goldenFlame.SetActive(false);

        corDesejada.SetActive(true);
    }
}