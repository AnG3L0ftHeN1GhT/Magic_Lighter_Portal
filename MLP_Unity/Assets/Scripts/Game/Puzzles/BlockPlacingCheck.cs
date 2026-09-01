using UnityEngine;

public class BlockPlacingCheck : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Portal")
        {
            print("abublé");
            //Destroy(other);
        }
    }

    
}