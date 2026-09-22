using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ActivatePyramid : MonoBehaviour
{
    public static ActivatePyramid instance;
    public GameObject portal;

    private bool pyrActiv = false;
    public bool allDone;

    public GameProcess score;

    public GameObject inPyramid;
    public GameObject cmPyramid;

    private UIManager interactionCursor;

    public ParticleSystem fogo;

    public InputActionReference kanji1;
    public InputActionReference kanji2;
    public InputActionReference kanji3;
    public InputActionReference kanji4;

    public InputActionAsset inputActions;

    [SerializeField] private InputActionReference leftClick;

    private GameObject currentPyramid;

    // Referência ao KanjiFunction
    private KanjiFunction kanjiFunction;

    private void Awake()
    {
        instance = this;

        interactionCursor = FindFirstObjectByType<UIManager>();

        // Procura o KanjiFunction na cena
        kanjiFunction = FindFirstObjectByType<KanjiFunction>();

        if (kanjiFunction == null)
        {
            Debug.LogError("ActivatePyramid: KanjiFunction não foi encontrado na cena.");
        }

        if (leftClick == null)
        {
            Debug.LogWarning(
                "ActivatePyramid: leftClick não foi configurado no Inspector."
            );
        }
    }

    private void Update()
    {
        StepsCheck();
        ActivatedPyramid();
        CheckLeftClick();
    }

    private void StepsCheck()
    {
        if (score.velaG == 2 && score.velaR == 2 && score.velaV == 2 && score.portal)
        {
            allDone = true;
        }
    }

    private void ActivatedPyramid()
    {
        if (allDone == true && !pyrActiv)
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
        yield return new WaitForSeconds(1f);

        // Instancia a pirâmide inicial
        if (inPyramid != null)
        {
            portal.SetActive(false);
            currentPyramid = Instantiate(
                inPyramid,
                transform.position,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogError(
                "ActivatePyramid: inPyramid não foi configurada no Inspector."
            );

            yield break;
        }

        // Verifica a cena atual
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "gluh")
        {
            currentPyramid.transform.localScale =
                new Vector3(1f, 1f, 1f);
        }
        else if (sceneName == "Pyramid Screen")
        {
            currentPyramid.transform.localScale =
                new Vector3(80f, 80f, 80f);

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
        if (leftClick == null)
            return;

        if (leftClick.action == null)
            return;

        if (!leftClick.action.WasPressedThisFrame())
            return;

        // Verifica se o KanjiFunction existe
        if (kanjiFunction == null)
        {
            Debug.LogError(
                "ActivatePyramid: KanjiFunction não foi encontrado."
            );

            return;
        }

        // Verifica se a pirâmide inicial existe
        if (currentPyramid == null)
            return;

        // Verifica se o objeto é realmente uma pirâmide
        if (!currentPyramid.CompareTag("Pirâmide"))
            return;

        // Verifica os Kanjis
        if (kanjiFunction.HasKanjis > 0)
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

                string sceneName =
                    SceneManager.GetActiveScene().name;

                if (sceneName == "gluh")
                {
                    currentPyramid.transform.localScale =
                        new Vector3(1f, 1f, 1f);
                }
                else if (sceneName == "Pyramid Screen")
                {
                    currentPyramid.transform.localScale =
                        new Vector3(80f, 80f, 80f);
                }
            }
            else
            {
                Debug.LogError(
                    "ActivatePyramid: cmPyramid não foi configurada no Inspector."
                );
            }
        }
        else
        {
            Debug.Log(
                "Você não achou nenhum Kanji. Continue procurando!"
            );
        }
    }
}