using System.Collections;
using UnityEngine;

public class PyramidPuzzle : MonoBehaviour
{
    [Header("Partes da pirâmide")]
    public Transform basePart;
    public Transform middlePart;
    public Transform topPart;

    [Header("Configuração")]
    public int correctBaseRotation = 0;
    public int correctMiddleRotation = 0;
    public int correctTopRotation = 0;

    public float rotationStep = 72f; // 360 / 5 faces
    public float rotationSpeed = 8f;

    [Header("Interação")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;

    [Header("Centro do Pentagrama")]
    public Transform pentagramCenter;
    public float returnSpeed = 2f;

    [Header("Efeito ao resolver")]
    public ParticleSystem solvedEffect;

    private Transform player;

    private int baseRotation = 0;
    private int middleRotation = 0;
    private int topRotation = 0;

    private bool playerInteracting = false;
    private bool puzzleSolved = false;
    private bool rotating = false;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (puzzleSolved || player == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                playerInteracting = !playerInteracting;
            }
        }

        if (!playerInteracting)
            return;

        HandleRotationInput();

        CheckPuzzle();
    }

    private void HandleRotationInput()
    {
        if (rotating)
            return;

        // A / D ou setas esquerda/direita
        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateCurrentPart(-1);
        }

        if (Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateCurrentPart(1);
        }

        // 1 = base
        // 2 = meio
        // 3 = topo
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentPart = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentPart = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentPart = 2;
        }
    }

    private int currentPart = 2;

    private void RotateCurrentPart(int direction)
    {
        switch (currentPart)
        {
            case 0:
                baseRotation += direction;
                baseRotation = NormalizeRotation(baseRotation);

                StartCoroutine(RotatePart(
                    basePart,
                    direction * rotationStep
                ));
                break;

            case 1:
                middleRotation += direction;
                middleRotation = NormalizeRotation(middleRotation);

                StartCoroutine(RotatePart(
                    middlePart,
                    direction * rotationStep
                ));
                break;

            case 2:
                topRotation += direction;
                topRotation = NormalizeRotation(topRotation);

                StartCoroutine(RotatePart(
                    topPart,
                    direction * rotationStep
                ));
                break;
        }
    }

    private int NormalizeRotation(int value)
    {
        if (value < 0)
            value += 5;

        if (value >= 5)
            value -= 5;

        return value;
    }

    private IEnumerator RotatePart(
        Transform part,
        float amount
    )
    {
        rotating = true;

        Quaternion startRotation = part.localRotation;

        Quaternion targetRotation =
            startRotation *
            Quaternion.Euler(0f, amount, 0f);

        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;

            part.localRotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                time
            );

            yield return null;
        }

        part.localRotation = targetRotation;

        rotating = false;
    }

    private void CheckPuzzle()
    {
        if (baseRotation == correctBaseRotation &&
            middleRotation == correctMiddleRotation &&
            topRotation == correctTopRotation)
        {
            SolvePuzzle();
        }
    }

    private void SolvePuzzle()
    {
        if (puzzleSolved)
            return;

        puzzleSolved = true;
        playerInteracting = false;

        if (solvedEffect != null)
        {
            solvedEffect.Play();
        }

        StartCoroutine(ReturnToPentagram());
    }

    private IEnumerator ReturnToPentagram()
    {
        Vector3 startPosition = transform.position;

        Vector3 targetPosition = pentagramCenter.position;

        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * returnSpeed;

            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                time
            );

            yield return null;
        }

        transform.position = targetPosition;

        Debug.Log("PUZZLE RESOLVIDO!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            interactionDistance
        );
    }
}