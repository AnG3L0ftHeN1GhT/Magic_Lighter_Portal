using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float rayDistance;
    private Camera cam;
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
        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if(Physics.Raycast(rayOrigin, cam.transform.forward, out hit, rayDistance))
        {
            Interactables interactable = hit.collider.GetComponent<Interactables>();
            if(interactable != null)
            {
                UIManager.instance.SetInteractionCursor(true);
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
}
