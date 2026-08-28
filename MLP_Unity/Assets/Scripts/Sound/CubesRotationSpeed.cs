using UnityEngine;
using FMODUnity;

public class CubeRotationSound : MonoBehaviour
{
    [Header("FMOD")]
    public EventReference whoosh;

    [Header("Configuração")]
    public float anguloMinimo = 10f;

    private Vector3 rotacaoAnterior;
    private float anguloAcumulado;

    void Start()
    {
        rotacaoAnterior = transform.eulerAngles;
    }

    void Update()
    {
        Vector3 rotacaoAtual = transform.eulerAngles;

        // Calcula quanto o cubo girou desde o último frame
        float diferencaX = Mathf.DeltaAngle(rotacaoAnterior.x, rotacaoAtual.x);
        float diferencaY = Mathf.DeltaAngle(rotacaoAnterior.y, rotacaoAtual.y);
        float diferencaZ = Mathf.DeltaAngle(rotacaoAnterior.z, rotacaoAtual.z);

        float movimento = Mathf.Abs(diferencaX) +
                          Mathf.Abs(diferencaY) +
                          Mathf.Abs(diferencaZ);

        if (movimento > 0.01f)
        {
            anguloAcumulado += movimento;
        }

        // Toca o Whoosh quando atingir o ângulo configurado
        if (anguloAcumulado >= anguloMinimo)
        {
            RuntimeManager.PlayOneShot(whoosh, transform.position);

            anguloAcumulado = 0f;
        }

        rotacaoAnterior = rotacaoAtual;
    }
}
