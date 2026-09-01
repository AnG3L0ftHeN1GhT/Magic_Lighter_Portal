using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class LighterFunction : MonoBehaviour
{
    public GameObject goldenFlame;
    public GameObject purpleFlame;
    public GameObject greenFlame;
    public int activeFlame;

    [SerializeField] EventReference acendendo;
    [SerializeField] EventReference fire;
    [SerializeField] EventReference snap;
    [SerializeField] EventReference scree;
    private FMOD.Studio.EventInstance instanciaIsqueiro;
    private FMOD.Studio.EventInstance instanciaOuro;
    private FMOD.Studio.EventInstance instanciaRoxa;
    private FMOD.Studio.EventInstance instanciaVerde;

    public static LighterFunction instance;
    public bool holdingLight;
    public int hasLighter;
    public GameObject lighter;
    public InputActionReference lightBttn;
    public InputActionReference fogo1;
    public InputActionReference fogo2;
    public InputActionReference fogo3;
    public InputActionAsset inputActions;


    public PlayerInteraction playerCollections;



    private void Awake()
    {
        instanciaIsqueiro = RuntimeManager.CreateInstance(acendendo);
        instanciaOuro = RuntimeManager.CreateInstance(fire);
        instanciaVerde = RuntimeManager.CreateInstance(snap);
        instanciaRoxa = RuntimeManager.CreateInstance(scree);
        instance = this;
    }

    void Update()
    {
        if ((hasLighter > 0) && lightBttn.action.WasPressedThisFrame())
        {
            holdingLight = !holdingLight;
            handleLighter();
        }

        if(fogo1.action.WasPressedThisFrame())
        {
            activeFlame = 1;
            MudarCorChama(activeFlame);
        } else if(fogo2.action.WasPressedThisFrame())
        {
            activeFlame = 2;
            MudarCorChama(activeFlame);
        } else if(fogo3.action.WasPressedThisFrame())
        {
            activeFlame = 3;
            MudarCorChama(activeFlame);
        }

        if(playerCollections.temIsqueiro && hasLighter == 0)
        {
            hasLighter = 1;
        }
        else
        {
            return;
        }
    } 

    private void handleLighter()
    {
        if (holdingLight)
        {
            instanciaIsqueiro.start();
        }
        lighter.SetActive(holdingLight);
    }

    public void SetLighter(int l)
    {
        hasLighter = l;
    }

    
    public void MudarCorChama(int corDesejada)
    {
        if(corDesejada == 1)
        {
            if(playerCollections.fluidoDourado)
            {
                goldenFlame.SetActive(true);
                purpleFlame.SetActive(false);
                greenFlame.SetActive(false);
                instanciaOuro.start();
            }
        } else if(corDesejada == 2)
        {
            if(playerCollections.fluidoRoxo)
            {
                goldenFlame.SetActive(false);
                purpleFlame.SetActive(true);
                greenFlame.SetActive(false);
                instanciaRoxa.start();
            }
        } else if(corDesejada == 3)
        {
            if(playerCollections.fluidoGreen)
            {
                goldenFlame.SetActive(false);
                purpleFlame.SetActive(false);
                greenFlame.SetActive(true);
                instanciaVerde.start();
            }
        }

    }
}
