using UnityEngine;

public class RotatePyramid : MonoBehaviour
{
    Vector2 firstPressPos;
    Vector2 secondPressPos;

    public float rotationSpeed = 300f;

    private Quaternion targetRotation;
    private bool isRotating = false;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        Swipe();
        AnimateRotation();
    }

    void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            firstPressPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            secondPressPos = Input.mousePosition;

            Vector2 swipe = secondPressPos - firstPressPos;

            // Ignora swipes muito pequenos
            if (swipe.magnitude < 50f)
                return;

            swipe.Normalize();

            if (swipe.x < -0.5f)
            {
                RotateLeft();
            }
            else if (swipe.x > 0.5f)
            {
                RotateRight();
            }
        }
    }

    void RotateLeft()
    {
        targetRotation *= Quaternion.Euler(0f, 90f, 0f);
        isRotating = true;
    }

    void RotateRight()
    {
        targetRotation *= Quaternion.Euler(0f, -90f, 0f);
        isRotating = true;
    }

    void AnimateRotation()
    {
        if (!isRotating)
            return;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }
}