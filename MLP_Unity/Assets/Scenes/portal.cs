using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string fimdejogo;

    void Start()
    {
        
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            SceneManager.LoadScene(fimdejogo);
        }
    }
}