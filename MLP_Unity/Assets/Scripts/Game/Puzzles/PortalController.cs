using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Header("Objetos do Portal")]
    public GameObject portalVisual;
    public Collider portalCollider;

    private void Start()
    {
        AtualizarPortal();
    }

    private void Update()
    {
        // Caso o estado seja alterado durante a cena
        AtualizarPortal();
    }

    private void AtualizarPortal()
    {
        if (GameProcess.Instance == null)
            return;

        bool portalAtivo = GameProcess.Instance.IsPortalActive();

        if (portalVisual != null)
            portalVisual.SetActive(portalAtivo);

        if (portalCollider != null)
            portalCollider.enabled = portalAtivo;
    }
}