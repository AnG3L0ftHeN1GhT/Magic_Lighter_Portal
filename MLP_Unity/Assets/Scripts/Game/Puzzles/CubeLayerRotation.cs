using UnityEngine;
using UnityEngine.InputSystem;

public class CubeLayerRotation : MonoBehaviour
{
    public float rotationSpeed = 300f;

    public GameObject down;
    public GameObject mid;
    public GameObject top;

    private Transform currentCube;
    private Quaternion targetRotation;
    private bool isRotating = false;

    void Update()
    {
        DetectClick();
        AnimateRotation();
    }

    void DetectClick()
    {
        // Clique esquerdo do mouse
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        // Não permite outra rotação enquanto uma estiver acontecendo
        if (isRotating)
            return;

        // Garante que existe uma câmera principal
        if (Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform clickedObject = hit.transform;

            while (clickedObject != null)
            {
                if (clickedObject.name == "Japa" ||
                    clickedObject.name == "Japa-2" ||
                    clickedObject.name == "Japa-3")
                {
                    StartRotation(clickedObject);
                    return;
                }

                clickedObject = clickedObject.parent;
            }
        }
    }

    void StartRotation(Transform cube)
    {
        currentCube = cube;

        // Rotação horizontal de 90 graus no eixo Y global
        targetRotation = Quaternion.AngleAxis(
            90f,
            Vector3.up
        ) * currentCube.rotation;

        isRotating = true;
    }

    void AnimateRotation()
    {
        if (!isRotating || currentCube == null)
            return;

        currentCube.rotation = Quaternion.RotateTowards(
            currentCube.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(
            currentCube.rotation,
            targetRotation) < 0.01f)
        {
            currentCube.rotation = targetRotation;

            isRotating = false;
            currentCube = null;
        }
    }
}