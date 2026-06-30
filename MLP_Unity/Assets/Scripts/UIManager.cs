using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject interactionCursor;
    public GameObject backImage;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInteractionCursor(bool state)
    {
        interactionCursor.SetActive(state);
    }

    public void SetBackImage(bool state)
    {
        backImage.SetActive(state);
    }
}
