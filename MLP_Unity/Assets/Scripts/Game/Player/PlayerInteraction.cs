using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Processo de Jogo")]
    public GameProcess processo;

    [Header("Estados do Jogador & Inventário")]
    public bool fluidoDourado;
    public bool fluidoRoxo;
    public bool fluidoGreen;
    public bool temIsqueiro;
    public bool setBox;
    public bool kanji1;
    public bool kanji2;
    public bool kanji3;
    public bool kanji4;
    public bool statua1;
    public bool statua2;
    public bool statua3;

    [Header("Configurações de Raycast e Interação")]
    public float rayDistance = 3.0f;
    public float pickupSpeed = 5.0f;
    public float rotateSpeed = 100.0f;
    public int hasKanjis;

    [Header("Referências da Cena")]
    public PlayerController playerMovements;
    public Transform objectViewer;
    public Item lighter;
    public Camera cam;
    public GerenteDeVela candleManager;

    [Header("Eventos")]
    public UnityEvent OnView;
    public UnityEvent OnFinishView;

    [Header("Input Actions")]
    public InputActionReference leftClick;
    public InputActionReference rightClick;
    public InputActionReference interactBttn;
    public InputActionReference look;
    public InputActionAsset inputActions;

    [Header("Estados Internos (Visualização)")]
    public bool clickIsPressed;
    public bool isViewing;
    public bool canFinish;
    public Interactables currentInteract;
    public Vector3 originPosition;
    public Quaternion originRotation;
    public Vector2 lookInput;

    void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.FindActionMap("Player").Enable();
        }

        if (leftClick != null) leftClick.action.Enable();
        if (rightClick != null) rightClick.action.Enable();
        if (interactBttn != null) interactBttn.action.Enable();
        if (look != null) look.action.Enable();

        if (leftClick != null)
        {
            leftClick.action.performed += OnLeftClickPerformed;
            leftClick.action.canceled += OnLeftClickCanceled;
        }

        if (rightClick != null) rightClick.action.performed += OnRightClickPerformed;
        if (interactBttn != null) interactBttn.action.performed += OnInteractPerformed;

        if (look != null)
        {
            look.action.performed += OnLookPerformed;
            look.action.canceled += OnLookCanceled;
        }
    }

    void OnDisable()
    {
        if (leftClick != null)
        {
            leftClick.action.performed -= OnLeftClickPerformed;
            leftClick.action.canceled -= OnLeftClickCanceled;
        }

        if (rightClick != null) rightClick.action.performed -= OnRightClickPerformed;
        if (interactBttn != null) interactBttn.action.performed -= OnInteractPerformed;

        if (look != null)
        {
            look.action.performed -= OnLookPerformed;
            look.action.canceled -= OnLookCanceled;
        }

        if (leftClick != null) leftClick.action.Disable();
        if (rightClick != null) rightClick.action.Disable();
        if (interactBttn != null) interactBttn.action.Disable();
        if (look != null) look.action.Disable();
    }

    #region Input Callbacks

    public void OnLeftClickPerformed(InputAction.CallbackContext ctx) => HandleLeftClick();

    public void OnLeftClickCanceled(InputAction.CallbackContext ctx)
    {
        if (clickIsPressed)
        {
            clickIsPressed = false;
            if (currentInteract != null)
            {
                currentInteract.transform.SetParent(null);
            }
            if (playerMovements != null)
            {
                playerMovements.ReleasedBox();
            }
        }
    }

    public void OnRightClickPerformed(InputAction.CallbackContext ctx)
    {
        if (isViewing && canFinish)
        {
            FinishView();
            if (inputActions != null)
            {
                inputActions.FindActionMap("Player").Enable();
            }
        }
    }

    public void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (isViewing && canFinish && currentInteract != null && currentInteract.item.stashable)
        {
            FinishView();
            if (inputActions != null)
            {
                inputActions.FindActionMap("Player").Enable();
            }

            if (currentInteract.item == lighter)
            {
                LighterFunction.instance.SetLighter(1);
            }
        }
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>();
    private void OnLookCanceled(InputAction.CallbackContext ctx) => lookInput = Vector2.zero;

    #endregion

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        LoadProgress();
    }

    void Update()
    {
        CheckInteractables();
    }

    void CheckInteractables()
    {
        if (isViewing)
        {
            if (currentInteract != null && currentInteract.item.grabbable)
            {
                RotateObject();
            }
            return;
        }

        if (clickIsPressed)
        {
            if (currentInteract != null && currentInteract.item.pesado)
            {
                currentInteract.transform.SetParent(transform);
                if (playerMovements != null)
                {
                    playerMovements.GrabbedBox();
                }
            }
            return;
        }

        if (cam == null) return;

        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, rayDistance))
        {
            Interactables interactable = hit.collider.GetComponent<Interactables>();
            UIManager.instance.SetInteractionCursor(interactable != null);
        }
        else
        {
            UIManager.instance.SetInteractionCursor(false);
        }
    }

    private void HandleLeftClick()
    {
        if (isViewing || cam == null) return;

        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, rayDistance))
        {
            Interactables interactable = hit.collider.GetComponent<Interactables>();

            if (interactable == null || interactable.isMoving)
            {
                return;
            }

            currentInteract = interactable;

            if (currentInteract.item.piramide)
            {
                if (processo != null && processo.IsPyramidSolved())
                {
                    Debug.Log("A pirâmide já foi resolvida.");
                    return;
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("Pyramid Screen");
                return;
            }

            if (currentInteract.item.falsoIsqueiro)
            {
                temIsqueiro = true;
                if (processo != null) processo.SetIsqueiro();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.ouro)
            {
                fluidoDourado = true;
                if (processo != null) processo.SetFluidoDourado();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.verde)
            {
                fluidoGreen = true;
                if (processo != null) processo.SetFluidoGreen();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.roxo)
            {
                fluidoRoxo = true;
                if (processo != null) processo.SetFluidoRoxo();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.velaDourada || currentInteract.item.velaRoxa || currentInteract.item.velaVerde)
            {
                candleManager = hit.collider.GetComponent<GerenteDeVela>();
                if (candleManager != null)
                {
                    candleManager.AcenderVela();
                }
            }

            if (currentInteract.item.kanji1)
            {
                kanji1 = true;
                if (processo != null) processo.SetKanji1();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.kanji2)
            {
                kanji2 = true;
                if (processo != null) processo.SetKanji2();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.kanji3)
            {
                kanji3 = true;
                if (processo != null) processo.SetKanji3();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.kanji4)
            {
                kanji4 = true;
                if (processo != null) processo.SetKanji4();
                Destroy(currentInteract.gameObject);
            }

            if (currentInteract.item.statua)
            {
                switch (currentInteract.gameObject.name)
                {
                    case "Feliz":
                        statua1 = true;
                        if (processo != null) processo.SetStatua1();
                        Destroy(currentInteract.gameObject);
                        break;
                    case "Neutra":
                        statua2 = true;
                        if (processo != null) processo.SetStatua2();
                        Destroy(currentInteract.gameObject);
                        break;
                    case "Triste":
                        statua3 = true;
                        if (processo != null) processo.SetStatua3();
                        Destroy(currentInteract.gameObject);
                        break;
                }
            }

            if (currentInteract != null && currentInteract.item.pesado)
            {
                clickIsPressed = true;
            }
        }
    }

    public void CanFinish()
    {
        canFinish = true;
        UIManager.instance.SetBackImage(true);
    }

    public void FinishView()
    {
        canFinish = false;
        isViewing = false;
        UIManager.instance.SetBackImage(false);

        if (currentInteract != null && currentInteract.item.grabbable)
        {
            currentInteract.transform.rotation = originRotation;
            StartCoroutine(MovingObject(currentInteract, originPosition));
        }

        if (OnFinishView != null) OnFinishView.Invoke();
    }

    public IEnumerator MovingObject(Interactables obj, Vector3 position)
    {
        obj.isMoving = true;
        float timer = 0;

        while (timer < 1)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, position, Time.deltaTime * pickupSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = position;
        obj.isMoving = false;
    }

    public void RotateObject()
    {
        if (currentInteract == null || cam == null) return;

        currentInteract.transform.Rotate(cam.transform.up, -Mathf.Deg2Rad * lookInput.x * rotateSpeed, Space.World);
        currentInteract.transform.Rotate(cam.transform.right, -Mathf.Deg2Rad * lookInput.y * rotateSpeed, Space.World);

        lookInput = Vector2.zero;
    }

    public void LoadProgress()
    {
        if (processo == null)
        {
            Debug.LogWarning("GameProgress não encontrado!");
            return;
        }

        fluidoDourado = processo.fluidoDourado;
        fluidoRoxo = processo.fluidoRoxo;
        fluidoGreen = processo.fluidoGreen;
        temIsqueiro = processo.temIsqueiro;

        kanji1 = processo.kanji1;
        kanji2 = processo.kanji2;
        kanji3 = processo.kanji3;
        kanji4 = processo.kanji4;

        statua1 = processo.statua1;
        statua2 = processo.statua2;
        statua3 = processo.statua3;
    }
}