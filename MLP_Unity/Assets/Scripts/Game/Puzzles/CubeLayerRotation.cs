using UnityEngine;

public class CubeLayerRotation : MonoBehaviour
{
    public float rotationSpeed = 300f;

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
        if (!Input.GetMouseButtonDown(0))
            return;

        if (isRotating)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform clickedObject = hit.transform;

            while (clickedObject != null)
            {
                if (clickedObject.name == "Cube" ||
                    clickedObject.name == "Cube.001" ||
                    clickedObject.name == "Cube.002")
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

        // Rotação horizontal no eixo Y GLOBAL
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