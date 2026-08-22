using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivatePyramid : MonoBehaviour
{
    public static ActivatePyramid instance;

    private bool pyrActiv = false;
    public int allDone;

    public GameObject pyramid;
    private UIManager interactionCursor;
    public ParticleSystem fogo;

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
            pyramid,
            transform.position,
            Quaternion.identity
        );

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