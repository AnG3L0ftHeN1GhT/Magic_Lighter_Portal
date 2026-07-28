using UnityEngine;
using UnityEngine.InputSystem;

public class LighterFunction : MonoBehaviour
{
    public static LighterFunction instance;
    private bool holdingLight;
    public int hasLighter;
    public GameObject lighter;
    public InputActionReference lightBttn;
    public InputActionAsset inputActions;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        if ((hasLighter > 0) && lightBttn.action.WasPressedThisFrame())
        {
            holdingLight = !holdingLight;
            handleLighter();
        }
    }

    private void handleLighter()
    {
        lighter.SetActive(holdingLight);
    }

    public void SetLighter(int l)
    {
        hasLighter = l;
    }
}
