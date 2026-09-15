using System.IO.Compression;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockPlacingCheck : MonoBehaviour
{
    public PlayerInteraction soltaissodaimeufi;
    public PlayerController movement;
    public float positionX;
    public float positionY;
    public float positionZ;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Portal")
        {
            print("abublé");
            Vector3 destination = new Vector3 (positionX, positionY ,positionZ );
            other.transform.position = destination;
            movement.ReleasedBox();
            other.transform.SetParent(null);
            soltaissodaimeufi.setBox = true;
        }
    }

    
}