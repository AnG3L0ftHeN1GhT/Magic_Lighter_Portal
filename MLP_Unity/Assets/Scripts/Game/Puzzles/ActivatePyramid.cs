using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ActivatePyramid : MonoBehaviour
{
    public static ActivatePyramid instance;

    private bool pyrActiv = false;

    [SerializeField] private int hasKanjis;
    public int allDone;

    public GameObject inPyramid;
    public GameObject cmPyramid;

    private UIManager interactionCursor;

    public ParticleSystem fogo;

    public InputActionReference kanji1;
    public InputActionReference kanji2;
    public InputActionReference kanji3;
    public InputActionReference kanji4;

    public InputActionAsset inputActions;

    // Agora pode ser configurado pelo Inspector
    [SerializeField] private InputActionReference leftClick;

    // Guarda a pirâmide que foi realmente instanciada
    private GameObject currentPyramid;

    private void Awake()
    {
        instance = this;

        interactionCursor = FindFirstObjectByType<UIManager>();

        // Verifica se o botão direito foi configurado
        if (leftClick == null)
        {
            Debug.LogWarning("ActivatePyramid: leftClick não foi configurado no Inspector.");
        }
    }

    private void Update()
    {
        ActivatedPyramid();
        CheckLeftClick();
    }

    private void ActivatedPyramid()
    {
        if (allDone > 1 && !pyrActiv)
        {
            pyrActiv = true;

            StartCoroutine(ActivateFireAndPyramid());
        }
    }

    private IEnumerator ActivateFireAndPyramid()
    {
        // Ativa o fogo
        if (fogo != null)
        {
            fogo.gameObject.SetActive(true);

            fogo.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            fogo.Play(true);
        }

        // Espera 5 segundos
        yield return new WaitForSeconds(5f);

        // Instancia a pirâmide inicial
        if (inPyramid != null)
        {
            currentPyramid = Instantiate(
                inPyramid,
                transform.position,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogError("ActivatePyramid: inPyramid não foi configurada no Inspector.");
            yield break;
        }

        // Verifica a cena atual
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "gluh")
        {
            currentPyramid.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (sceneName == "Pyramid Screen")
        {
            currentPyramid.transform.localScale = new Vector3(80f, 80f, 80f);

            if (interactionCursor != null)
            {
                interactionCursor.SetInteractionCursor(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void CheckLeftClick()
    {
        // Evita NullReferenceException
        if (leftClick == null)
            return;

        if (leftClick.action == null)
            return;

        // Só permite trocar a pirâmide se:
        // 1. A pirâmide inicial existir
        // 2. hasKanjis for maior que 0
        // 3. O botão direito for pressionado
        if (currentPyramid != null &&
            hasKanjis > 0 &&
            leftClick.action.WasPressedThisFrame())
        {
            // Destrói a pirâmide inicial
            Destroy(currentPyramid);

            // Instancia a nova pirâmide
            if (cmPyramid != null)
            {
                currentPyramid = Instantiate(
                    cmPyramid,
                    transform.position,
                    Quaternion.identity
                );

                string sceneName = SceneManager.GetActiveScene().name;

                if (sceneName == "gluh")
                {
                    currentPyramid.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (sceneName == "Pyramid Screen")
                {
                    currentPyramid.transform.localScale = new Vector3(80f, 80f, 80f);
                }
            }
            else
            {
                Debug.LogError("ActivatePyramid: cmPyramid não foi configurada no Inspector.");
            }
        }
    }
}