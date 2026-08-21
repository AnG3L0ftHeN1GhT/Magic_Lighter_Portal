using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivatePyramid : MonoBehaviour
{
    public static ActivatePyramid instance;

    private bool pyrActiv = false;
    public int allDone;

    public GameObject pyramid;

    private UIManager interactionCursor;

    private void Awake()
    {
        instance = this;
        interactionCursor = FindFirstObjectByType<UIManager>();
    }

    private void Update()
    {
        ActivatedPyramid();
    }

    private void ActivatedPyramid()
    {
        if (allDone > 1 && !pyrActiv)
        {
            pyrActiv = true;

            GameObject newPyramid = Instantiate(pyramid, transform.position, Quaternion.identity);

            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == "gluh")
            {
                newPyramid.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (sceneName == "Pyramid Screen")
            {
                newPyramid.transform.localScale = new Vector3(80f, 80f, 80f);

                if (interactionCursor != null)
                {
                    interactionCursor.SetInteractionCursor(true);
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}