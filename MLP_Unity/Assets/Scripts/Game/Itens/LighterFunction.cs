using UnityEngine;
using UnityEngine.InputSystem;

public class LighterFunction : MonoBehaviour
{
    public GameObject goldenFlame;
    public GameObject purpleFlame;
    public GameObject greenFlame;
    public int activeFlame;



    public static LighterFunction instance;
    private bool holdingLight;
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
            MudarCorChama(1);
        } else if(fogo2.action.WasPressedThisFrame())
        {
            MudarCorChama(2);
        } else if(fogo3.action.WasPressedThisFrame())
        {
            MudarCorChama(3);
        }
    } 

    private void handleLighter()
    {
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
            }
        } else if(corDesejada == 2)
        {
            if(playerCollections.fluidoRoxo)
            {
                goldenFlame.SetActive(false);
                purpleFlame.SetActive(true);
                greenFlame.SetActive(false);
            }
        } else if(corDesejada == 3)
        {
            if(playerCollections.fluidoGreen)
            {
                goldenFlame.SetActive(false);
                purpleFlame.SetActive(false);
                greenFlame.SetActive(true);
            }
        }

    }
    
    /*
    if(currentInteract.item.papelDeOuro)
                    {
                        activeFlame = 1;
                        MudarCorChama(activeFlame);
                        return;
                    }
                    */
}
