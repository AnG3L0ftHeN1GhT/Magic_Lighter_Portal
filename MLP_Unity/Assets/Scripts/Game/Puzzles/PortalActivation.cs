using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalActivation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameProcess verificacao;
    [SerializeField] private string fimdejogo;

    void Update()
    {
        if (verificacao.velasProntas && verificacao.pyramidSolved)
        {
            gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(fimdejogo);
        }
    }
}
