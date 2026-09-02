using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private String jogoPrincipal;
    [SerializeField] private String creditos;
    [SerializeField] private String menuPrincipal;

    public void Jogar()
    {
        SceneManager.LoadScene(jogoPrincipal);
    }

    public void AbrirCreditios()
    {
        SceneManager.LoadScene(creditos);
    }

    public void AbrirMenuPrincipal()
    {
        SceneManager.LoadScene(menuPrincipal);
    }

    public void Sair()
    {
        Debug.Log("Saindo dessa joça");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;        
        #else
        Application.Quit();
        #endif   
    }
}
