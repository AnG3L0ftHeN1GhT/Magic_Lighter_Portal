using UnityEngine;

public class GerenteDeVela : MonoBehaviour
{

    public GameObject fogo;
    //public PlayerInteraction velaAcesa;
    public LighterFunction isqueiroAceso;
    public GameProcess progress;
    public Interactables tipoDeVela;

    void Start()
    {
        fogo.SetActive(false);
    }

    public void AcenderVela()
    {
        if (isqueiroAceso.holdingLight)
        {
            if (tipoDeVela.item.velaDourada && isqueiroAceso.activeFlame == 1)
            {
                fogo.SetActive(true);
                Destroy(tipoDeVela);
                Destroy(this);
                progress.LightVelaG();
            }
            else if (tipoDeVela.item.velaRoxa && isqueiroAceso.activeFlame == 2)
            {
                fogo.SetActive(true);
                Destroy(tipoDeVela);
                Destroy(this);
                progress.LightVelaR();
            }
            else if (tipoDeVela.item.velaVerde && isqueiroAceso.activeFlame == 3)
            {
                fogo.SetActive(true);
                Destroy(tipoDeVela);
                Destroy(this);
                progress.LightVelaV();
            }
        }
        
    }
}
