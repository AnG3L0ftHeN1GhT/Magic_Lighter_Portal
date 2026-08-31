using UnityEngine;
using FMODUnity;

public class DraggingSound : MonoBehaviour
{
    public EventReference arrastando;
    private FMOD.Studio.EventInstance instancia;
    public PlayerInteraction playerReference;
    private Vector3 pastMovement;

    void Start()
    {
        instancia = RuntimeManager.CreateInstance(arrastando);
        instancia.start();
        instancia.setPaused(true);
    }

    void Update()
    {
        if (playerReference.clickIsPressed)
        {
            if (playerReference.transform.position != pastMovement)
            {
                instancia.setPaused(false);
            }
            else
            {
                instancia.setPaused(true);
            }
        }
        else
        {
            instancia.setPaused(true);
        }
        
        pastMovement = new Vector3(playerReference.transform.position.x, playerReference.transform.position.y, playerReference.transform.position.z);
    }
}
