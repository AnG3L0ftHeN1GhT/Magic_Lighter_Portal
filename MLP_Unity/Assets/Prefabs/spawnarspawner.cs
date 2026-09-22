using UnityEngine;

public class spawnarspawner : MonoBehaviour
{
public GameObject ospawnado;
    void Start()
    {
        Instantiate(ospawnado);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
