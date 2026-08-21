using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public bool grabbable;
    public bool stashable;
    public bool vela;
    public bool piramide;
    public bool ouro;
    public bool verde;
    public bool roxo;
    public AudioClip audioClip;
    public string text;
}
