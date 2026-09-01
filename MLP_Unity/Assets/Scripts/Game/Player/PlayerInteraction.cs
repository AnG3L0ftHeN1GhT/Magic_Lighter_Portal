using System.Collections;
using NUnit.Framework;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] GameProcess processo;

    public bool fluidoDourado;
    public bool fluidoRoxo;
    public bool fluidoGreen;
    public bool temIsqueiro;
    public bool kanji1;
    public bool kanji2;
    public bool kanji3;
    public bool kanji4;
    public bool statua1;
    public bool statua2;
    public bool statua3;

    public bool clickIsPressed;
    public PlayerController playerMovements;

    public float rayDistance;
    public float pickupSpeed;
    public float rotateSpeed;
    public int hasKanjis;

    public Transform objectViewer;
    public UnityEvent OnView;
    public UnityEvent OnFinishView;

    public Item lighter;
    private Camera cam;

    private bool isViewing;
    private bool canFinish;

    private Interactables currentInteract;
    private Vector3 originPosition;
    private Quaternion originRotation;

    public InputActionReference leftClick;
    public InputActionReference rightClick;
    public InputActionReference interactBttn;
    public InputActionReference look;
    public InputActionAsset inputActions;
    
    public GerenteDeVela candleManager;

    void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    void Start()
    {
        cam = Camera.main;

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
            if (currentInteract.item.grabbable)
            {
                RotateObject();
            }

            if (canFinish && currentInteract.item.stashable && interactBttn.action.WasPressedThisFrame())
            {
                FinishView();
                inputActions.FindActionMap("Player").Enable();

                if (currentInteract.item == lighter)
                {
                    LighterFunction.instance.SetLighter(1);
                }
            }
            else if (canFinish && rightClick.action.WasPressedThisFrame())
            {
                FinishView();
                inputActions.FindActionMap("Player").Enable();
            }

            return;
        }

        if (clickIsPressed)
        {
            if (PressedClickCheck())
            {
                if (currentInteract.item.pesado)
                {
                    currentInteract.transform.SetParent(transform);

                }
                playerMovements.GrabbedBox();
                return;
            }
            else
            {
                clickIsPressed = false;
                currentInteract.transform.SetParent(null);
                playerMovements.ReleasedBox();
            }
        }

        RaycastHit hit;
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, rayDistance))
        {
            Interactables interactable = hit.collider.GetComponent<Interactables>();

            if (interactable != null)
            {
                UIManager.instance.SetInteractionCursor(true);

                if (leftClick.action.WasPressedThisFrame())
                {
                    if (interactable.isMoving)
                    {
                        return;
                    }

                    currentInteract = interactable;

                if (currentInteract.item.inPiramide)
                {
                    // Se a pirâmide já foi resolvida, não abre novamente
                    if (processo != null && processo.IsPyramidSolved())
                    {
                        Debug.Log("A pirâmide já foi resolvida.");

                        return;
                    }

                 UnityEngine.Cursor.lockState = CursorLockMode.None;
                 UnityEngine.Cursor.visible = true;

                 SceneManager.LoadScene("Pyramid Screen");

             return;
                }

                    if (currentInteract.item.falsoIsqueiro)
                    {
                        temIsqueiro = true;
                        processo.SetIsqueiro();

                        Destroy(currentInteract.gameObject);
                    }

                    if (currentInteract.item.ouro)
                    {
                        fluidoDourado = true;
                        processo.SetFluidoDourado();

                        Destroy(currentInteract.gameObject);
                    }

                    if (currentInteract.item.verde)
                    {
                        fluidoGreen = true;
                        processo.SetFluidoGreen();

                        Destroy(currentInteract.gameObject);
                    }

                    if (currentInteract.item.roxo)
                    {
                        fluidoRoxo = true;
                        processo.SetFluidoRoxo();

                        Destroy(currentInteract.gameObject);
                    }

                    if (currentInteract.item.velaDourada || currentInteract.item.velaRoxa || currentInteract.item.velaVerde)
                    {
                        candleManager = hit.collider.GetComponent<GerenteDeVela>();
                        candleManager.AcenderVela();
                    }

                    if (currentInteract.item.kanji1)
                    {
                        kanji1 = true;
                        processo.SetKanji1();

                        Destroy(currentInteract.gameObject);
                    }

                    if (currentInteract.item.kanji2)
                    {
                        kanji2 = true;
                        processo.SetKanji2();

                        Destroy(currentInteract.gameObject);
                    }
                    
                    if (currentInteract.item.kanji3)
                    {
                        kanji3 = true;
                        processo.SetKanji3();

                        Destroy(currentInteract.gameObject);
                    }
                    
                    if (currentInteract.item.kanji4)
                    {
                        kanji4 = true;
                        processo.SetKanji4();

                        Destroy(currentInteract.gameObject);
                    }

                    if (currentInteract.item.statua)
                    {
                        Debug.Log(currentInteract.gameObject.ToSafeString());
                        switch (currentInteract.gameObject.ToSafeString())
                        {
                            case "Feliz":
                                statua1 = true;
                                processo.SetStatua1();

                                Destroy(currentInteract.gameObject);
                                break;
                            case "Neutra":
                                statua2 = true;
                                processo.SetStatua2();

                                Destroy(currentInteract.gameObject);
                                break;
                            case "Triste":
                                statua3 = true;
                                processo.SetStatua3();

                                Destroy(currentInteract.gameObject);
                                break;
                        }
                    }


                    /*
                    inputActions.FindActionMap("Player").Disable();
                    OnView.Invoke();
                    currentInteract = interactable;
                    isViewing = true;

                    Invoke("CanFinish", 1f);

                    if (currentInteract.item.grabbable)
                    {
                        originPosition = currentInteract.transform.position;
                        originRotation = currentInteract.transform.rotation;
                        StartCoroutine(MovingObject(currentInteract, objectViewer.position));
                    } inutilizado e podre; não é nescessário por enquanto. Fica aqui só caso a mecância volte, o que é improvável
                    */
                    clickIsPressed = PressedClickCheck();
                }
                
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

    void CanFinish()
    {
        canFinish = true;
        UIManager.instance.SetBackImage(true);
    }

    void FinishView()
    {
        canFinish = false;
        isViewing = false;
        UIManager.instance.SetBackImage(false);

        if (currentInteract.item.grabbable)
        {
            currentInteract.transform.rotation = originRotation;
            StartCoroutine(MovingObject(currentInteract, originPosition));
        }

        OnFinishView.Invoke();
    }



    IEnumerator MovingObject(Interactables obj, Vector3 position)
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

    void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        currentInteract.transform.Rotate(cam.transform.up, -Mathf.Deg2Rad * x * rotateSpeed, Space.World);
        currentInteract.transform.Rotate(cam.transform.right, -Mathf.Deg2Rad * y * rotateSpeed, Space.World);
    }

    bool PressedClickCheck()
    {
        if (leftClick.action.IsPressed())
        {
            if(currentInteract.item.pesado)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    void LoadProgress()
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