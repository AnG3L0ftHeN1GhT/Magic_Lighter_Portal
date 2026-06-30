using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float rayDistance;
    public float rotateSpeed;
    public Transform objectViwer;
    private Camera cam;
    private bool isViewing;
    private Interactables currentInteract;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public InputActionReference leftClick;
    public InputActionReference look;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            if(currentInteract.item.grabbable && leftClick.action.IsPressed())
            {
                RotateObject();
            }
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
                    currentInteract = interactable;
                    isViewing = true;

                    if(currentInteract.item.grabbable)
                    {
                        originPosition = currentInteract.transform.position;
                        originRotation = currentInteract.transform.rotation;
                        StartCoroutine(Movingobject(currentInteract, objectViwer.position));
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

    IEnumerator Movingobject(Interactables obj, Vector3 position)
    {
        float timer = 0;
        while(timer < 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, position, Time.deltaTime * rotateSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = position;
    }

    void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        currentInteract.transform.Rotate(cam.transform.up, -Mathf.Deg2Rad * x * rotateSpeed, Space.World);
        currentInteract.transform.Rotate(cam.transform.right, -Mathf.Deg2Rad * y * rotateSpeed, Space.World);
    }
}
