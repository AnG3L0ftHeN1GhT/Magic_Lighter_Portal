using UnityEngine;

public class CandleLight : MonoBehaviour
{
    public Light luz;

    public float intensidadeMin = 0.8f;
    public float intensidadeMax = 1.2f;
    public float velocidade = 2f;

    void Update()
    {
        float ruido = Mathf.PerlinNoise(Time.time * velocidade, 0f);

        luz.intensity = Mathf.Lerp(
            intensidadeMin,
            intensidadeMax,
            ruido
        );
    }
}