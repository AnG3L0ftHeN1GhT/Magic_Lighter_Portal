using UnityEngine;
using FMODUnity;
using System;

public class DraggingSound : MonoBehaviour
{
    public EventReference arrastando;
    private FMOD.Studio.EventInstance instancia;
    public PlayerInteraction playerReference;
    private Vector3 pastMovement;

    void Start()
    {
        instancia = RuntimeManager.CreateInstance(arrastando);
    }

    void Update()
    {
        if (playerReference.clickIsPressed)
        {
            if (playerReference.transform.position == pastMovement)
            {
                instancia.start();
            }
        }
        else
        {
            instancia.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        pastMovement = new Vector3(playerReference.transform.position.x, playerReference.transform.position.y, playerReference.transform.position.z);
    }
}
