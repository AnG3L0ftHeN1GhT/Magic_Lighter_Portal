using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject standardFlame;
    public GameObject goldenFlame;
    //public GameObject StandardFlame;

    public float rayDistance;
    public float pickupSpeed;
    public float rotateSpeed;
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

    [Header("Grab To Hand")]
    [Tooltip("Transform que representa a mão do player. O item carregado será filho deste transform.")]
    public Transform handPosition;

    [Tooltip("Ação do Input System vinculada à tecla de pegar/largar (configurada no Input Actions Asset, ex: Z).")]
    public InputActionReference grabKey;

    [Tooltip("Velocidade com que o objeto se ajusta até a mão ao ser pego.")]
    public float handMoveSpeed = 8f;

    private Interactables carriedItem;
    private Rigidbody carriedRigidbody;
    private Collider carriedCollider;
    private Coroutine handRoutine;

    void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        CheckInteractables();
    }

    void CheckInteractables()
    {
        if (isViewing)
        {
            if (currentInteract.item.grabbable)
            {
                RotateObject();
            }
            if (canFinish && currentInteract.item.stashable && interactBttn.action.WasPressedThisFrame())
            {
                FinishView();
                inputActions.FindActionMap("Player").Enable();
                if (currentInteract.item == lighter)
                {
                    LighterFunction.instance.SetLighter(1);
                }
            }
            else if (canFinish && rightClick.action.WasPressedThisFrame())
            {
                FinishView();
                inputActions.FindActionMap("Player").Enable();
            }
            return;
        }
        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, rayDistance))
        {
            Interactables interactable = hit.collider.GetComponent<Interactables>();
            if (interactable != null)
            {
                UIManager.instance.SetInteractionCursor(true);
                if (leftClick.action.WasPressedThisFrame())
                {
                    if (interactable.isMoving)
                    {
                        return;
                    }

                    // Não permite abrir o modo de visualização em um item que já está na mão
                    if (interactable == carriedItem)
                    {
                        return;
                    }

                    currentInteract = interactable;

                    if(currentInteract.item.papel)
                    {
                        MudarCorChama(goldenFlame);

                        return;
                    }

                    inputActions.FindActionMap("Player").Disable();
                    OnView.Invoke();
                    currentInteract = interactable;
                    isViewing = true;

                    Invoke("CanFinish", 1f);

                    if (currentInteract.item.grabbable)
                    {
                        originPosition = currentInteract.transform.position;
                        originRotation = currentInteract.transform.rotation;
                        StartCoroutine(MovingObject(currentInteract, objectViewer.position));
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

    // Controla o pegar/largar de itens na mão, de forma independente do modo de visualização.
    void CheckGrabInput()
    {
        // Enquanto estiver no modo de visualização (segurando o objeto na frente da câmera), não mexe na mão.
        if (isViewing)
        {
            return;
        }

        // Já está carregando algo: a mesma tecla larga o item.
        if (carriedItem != null)
        {
            if (grabKey.action.WasPressedThisFrame())
            {
                DropItem();
            }
            return;
        }

        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, rayDistance))
        {
            Interactables interactable = hit.collider.GetComponent<Interactables>();

            if (interactable != null && !interactable.isMoving && interactable.item.grabbable)
            {
                if (grabKey.action.WasPressedThisFrame())
                {
                    GrabItem(interactable);
                }
            }
        }
    }

    void GrabItem(Interactables interactable)
    {
        carriedItem = interactable;
        carriedRigidbody = interactable.GetComponent<Rigidbody>();
        carriedCollider = interactable.GetComponent<Collider>();

        if (carriedRigidbody != null)
        {
            carriedRigidbody.isKinematic = true;
            carriedRigidbody.useGravity = false;
        }

        // Desativa o collider enquanto está na mão para não colidir com o próprio player.
        if (carriedCollider != null)
        {
            carriedCollider.enabled = false;
        }

        interactable.transform.SetParent(handPosition);

        if (handRoutine != null)
        {
            StopCoroutine(handRoutine);
        }
        handRoutine = StartCoroutine(MoveToHand(interactable.transform));
    }

    void DropItem()
    {
        if (carriedItem == null)
        {
            return;
        }

        if (handRoutine != null)
        {
            StopCoroutine(handRoutine);
            handRoutine = null;
        }

        carriedItem.transform.SetParent(null);

        if (carriedCollider != null)
        {
            carriedCollider.enabled = true;
        }

        if (carriedRigidbody != null)
        {
            carriedRigidbody.isKinematic = false;
            carriedRigidbody.useGravity = true;
        }

        carriedItem = null;
        carriedRigidbody = null;
        carriedCollider = null;
    }

    // Suaviza o encaixe do objeto na posição/rotação da mão, mas mantém o parent
    // já definido em GrabItem, então o item acompanha o player normalmente após o encaixe.
    IEnumerator MoveToHand(Transform obj)
    {
        Vector3 startPos = obj.localPosition;
        Quaternion startRot = obj.localRotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * handMoveSpeed;
            obj.localPosition = Vector3.Lerp(startPos, Vector3.zero, t);
            obj.localRotation = Quaternion.Slerp(startRot, Quaternion.identity, t);
            yield return null;
        }

        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
    }

    void CanFinish()
    {
        canFinish = true;
        UIManager.instance.SetBackImage(true);
    }

    void FinishView()
    {
        canFinish = false;
        isViewing = false;
        UIManager.instance.SetBackImage(false);
        if (currentInteract.item.grabbable)
        {
            currentInteract.transform.rotation = originRotation;
            StartCoroutine(MovingObject(currentInteract, originPosition));
        }
        OnFinishView.Invoke();
    }

    IEnumerator MovingObject(Interactables obj, Vector3 position)
    {
        obj.isMoving = true;
        float timer = 0;
        while (timer < 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, position, Time.deltaTime * pickupSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = position;
        obj.isMoving = false;
    }

    void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        currentInteract.transform.Rotate(cam.transform.up, -Mathf.Deg2Rad * x * rotateSpeed, Space.World);
        currentInteract.transform.Rotate(cam.transform.right, -Mathf.Deg2Rad * y * rotateSpeed, Space.World);
    }

    public void MudarCorChama(GameObject corDesejada)
    {
        standardFlame.SetActive(false);
        goldenFlame.SetActive(false);

        corDesejada.SetActive(true);
    }
}