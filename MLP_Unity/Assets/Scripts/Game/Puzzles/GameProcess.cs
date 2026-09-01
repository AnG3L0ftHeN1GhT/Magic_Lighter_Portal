using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public static GameProcess Instance;

    public bool pyramidSolved;

    public bool fluidoDourado;
    public bool fluidoRoxo;
    public bool fluidoGreen;
    public bool temIsqueiro;

    public bool kanji1;
    public bool kanji2;
    public bool kanji3;
    public bool kanji4;
    public int velaV;
    public int velaR;
    public int velaG;

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

    public void SolvePyramid()
    {
        pyramidSolved = true;
        Debug.Log("Pirâmide resolvida!");
    }

    public bool IsPyramidSolved()
    {
        return pyramidSolved;
    }



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

    public void SetIsqueiro()
    {
        temIsqueiro = true;
    }

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