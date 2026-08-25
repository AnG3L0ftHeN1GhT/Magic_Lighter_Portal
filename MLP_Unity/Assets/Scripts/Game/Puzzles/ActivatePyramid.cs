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
    private InputActionReference rightClick;

    private void Awake()
    {
        instance = this;
        interactionCursor = FindFirstObjectByType<UIManager>();
    }

    private void Update()
    {
        ActivatedPyramid();
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
        // Ativa o objeto do Particle System
        if (fogo != null)
        {
            fogo.gameObject.SetActive(true);

            // Garante que o sistema esteja parado antes de iniciar
            fogo.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // Inicia o Particle System
            fogo.Play(true);
        }

        // Espera 5 segundos
        yield return new WaitForSeconds(5f);

        // Instancia a pirâmide
        GameObject newPyramid = Instantiate(
            inPyramid,
            transform.position,
            Quaternion.identity
        );

        if ((hasKanjis > 0) && rightClick.action.WasPressedThisFrame())
        {
            Destroy(inPyramid);
            GameObject newPiramide = Instantiate(cmPyramid, transform.position, Quaternion.identity);
        }

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "gluh")
        {
            newPyramid.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (sceneName == "Pyramid Screen")
        {
            newPyramid.transform.localScale = new Vector3(80f, 80f, 80f);

            if (interactionCursor != null)
            {
                interactionCursor.SetInteractionCursor(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}