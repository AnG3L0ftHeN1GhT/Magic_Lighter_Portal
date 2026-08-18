using UnityEngine;
using UnityEngine.InputSystem;

public class CubeFunction : MonoBehaviour
{
    public static CubeFunction instance;
    private bool holdingCube;
    public int hasCube;
    public GameObject cube;
    public InputActionReference cubeBttn;
    public InputActionAsset inputActions;
    public bool CUBO;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        if ((hasCube > 0) && cubeBttn.action.WasPressedThisFrame())
        {
            holdingCube = !holdingCube;
            handleLighter();
        }
    }

    private void handleLighter()
    {
        if (hasCube == 0)
        cube.SetActive(holdingCube);
    }

    public void SetLighter(int l)
    {
        hasCube = l;
    }
}
