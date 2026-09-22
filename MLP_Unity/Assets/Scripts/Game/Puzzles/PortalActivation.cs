using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalActivation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameProcess verificacao;
    public GameObject realPortal;

    void Update()
    {
        if (verificacao.velasProntas && verificacao.pyramidSolved)
        {
            realPortal.SetActive(true);
        }
    }
}
