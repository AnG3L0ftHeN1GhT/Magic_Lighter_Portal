using UnityEngine;
using FMODUnity;

public class GerenteDeVela : MonoBehaviour
{
    public GameObject fogo;
    //public PlayerInteraction velaAcesa;
    public LighterFunction isqueiroAceso;
    public GameProcess progress;
    public Interactables tipoDeVela;

    [SerializeField] EventReference fire;
    [SerializeField] EventReference snap;
    [SerializeField] EventReference scree;

    private FMOD.Studio.EventInstance instanciaOuro;
    private FMOD.Studio.EventInstance instanciaRoxa;
    private FMOD.Studio.EventInstance instanciaVerde;

    private void Awake()
    {
        instanciaOuro = RuntimeManager.CreateInstance(fire);
        instanciaVerde = RuntimeManager.CreateInstance(snap);
        instanciaRoxa = RuntimeManager.CreateInstance(scree);
    }
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
                instanciaOuro.start();
                Destroy(tipoDeVela);
                Destroy(this);
                progress.LightVelaG();
            }
            else if (tipoDeVela.item.velaRoxa && isqueiroAceso.activeFlame == 2)
            {
                fogo.SetActive(true);
                instanciaRoxa.start();
                Destroy(tipoDeVela);
                Destroy(this);
                progress.LightVelaR();
            }
            else if (tipoDeVela.item.velaVerde && isqueiroAceso.activeFlame == 3)
            {
                fogo.SetActive(true);
                instanciaVerde.start();
                Destroy(tipoDeVela);
                Destroy(this);
                progress.LightVelaV();
            }
        }
        
    }
}
