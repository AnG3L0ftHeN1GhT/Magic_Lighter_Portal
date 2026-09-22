using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public static GameProcess Instance;

    [Header("Puzzle da Pirâmide")]
    public bool pyramidSolved;

    [Header("Itens")]
    public bool fluidoDourado;
    public bool fluidoRoxo;
    public bool fluidoGreen;
    public bool temIsqueiro;

    [Header("Kanjis")]
    public bool kanji1;
    public bool kanji2;
    public bool kanji3;
    public bool kanji4;

    [Header("Velas")]
    public int velaV;
    public int velaR;
    public int velaG;
    public bool velasProntas;

    [Header("Portal")]
    public bool portal;

    [Header("Estátuas")]
    public bool statua1;
    public bool statua2;
    public bool statua3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (velaG == 2 && velaR == 2 && velaV == 2)
        {
            velasProntas = true;
        }
    }

    // =========================
    // PIRÂMIDE
    // =========================

    public void SolvePyramid()
    {
        pyramidSolved = true;

        // Ativa o portal assim que a pirâmide for concluída
        SetPortal();

        Debug.Log("Pirâmide resolvida!");
        Debug.Log("Portal ativado!");
    }

    public bool IsPyramidSolved()
    {
        return pyramidSolved;
    }

    // =========================
    // PORTAL
    // =========================

    public void SetPortal()
    {
        portal = true;
        Debug.Log("Portal = TRUE");
    }

    public bool IsPortalActive()
    {
        return portal;
    }

    // =========================
    // FLUIDOS
    // =========================

    public void SetFluidoDourado()
    {
        fluidoDourado = true;
    }

    public void SetFluidoRoxo()
    {
        fluidoRoxo = true;
    }

    public void SetFluidoGreen()
    {
        fluidoGreen = true;
    }

    // =========================
    // ISQUEIRO
    // =========================

    public void SetIsqueiro()
    {
        temIsqueiro = true;
    }

    // =========================
    // KANJIS
    // =========================

    public void SetKanji1()
    {
        kanji1 = true;
    }

    public void SetKanji2()
    {
        kanji2 = true;
    }

    public void SetKanji3()
    {
        kanji3 = true;
    }

    public void SetKanji4()
    {
        kanji4 = true;
    }

    // =========================
    // ESTÁTUAS
    // =========================

    public void SetStatua1()
    {
        statua1 = true;
    }

    public void SetStatua2()
    {
        statua2 = true;
    }

    public void SetStatua3()
    {
        statua3 = true;
    }

    // =========================
    // VELAS
    // =========================

    public void LightVelaV()
    {
        velaV += 1;
    }

    public void LightVelaR()
    {
        velaR += 1;
    }

    public void LightVelaG()
    {
        velaG += 1;
    }
}