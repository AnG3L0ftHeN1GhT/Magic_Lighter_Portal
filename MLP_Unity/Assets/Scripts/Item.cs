using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public bool grabbable;
    public bool stashable;
    public bool vela;
    public bool papelDeOuro;

    //esse cara só não vem pro inventario, esse bool existe p pegar o isqueiro
    public bool ItemAdquirivel;
    public AudioClip audioClip;
    public string text;
}
