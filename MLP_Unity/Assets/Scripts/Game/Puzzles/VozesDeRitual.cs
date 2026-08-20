using Unity.VisualScripting;
using UnityEngine;

public class VozesDeRitual : MonoBehaviour
{
    public PlayerInteraction playerController;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(playerController.activeFlame == 1)
            {
                print("sfjadshfiseki");
            }
        }
    }
}
