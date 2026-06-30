using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float rayDistance;
    public float pickupSpeed;
    public float rotateSpeed;
    public Transform objectViewer;
    public UnityEvent OnView;
    public UnityEvent OnFinishView;
    private Camera cam;
    private bool isViewing;
    private bool canFinish;
    private Interactables currentInteract;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public InputActionReference leftClick;
    public InputActionReference rightClick;
    public InputActionReference look;
    public InputActionAsset inputActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteractables();
    }

    void CheckInteractables()
    {
        if(isViewing)
        {
            if(currentInteract.item.grabbable)
            {
                RotateObject();
            }
            if(canFinish && rightClick.action.WasPressedThisFrame())
            {
                FinishView();
                inputActions.FindActionMap("Player").Enable();
            }
            return;
        }
        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if(Physics.Raycast(rayOrigin, cam.transform.forward, out hit, rayDistance))
        {
            Interactables interactable = hit.collider.GetComponent<Interactables>();
            if(interactable != null)
            {
                UIManager.instance.SetInteractionCursor(true);
                if(leftClick.action.WasPressedThisFrame())
                {
                    if(interactable.isMoving)
                    {
                        return;
                    }
                    
                    inputActions.FindActionMap("Player").Disable();
                    OnView.Invoke();
                    currentInteract = interactable;
                    isViewing = true;

                    Invoke("CanFinish", 1f);

                    if(currentInteract.item.grabbable)
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
        if(currentInteract.item.grabbable)
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
        while(timer < 1)
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
}
