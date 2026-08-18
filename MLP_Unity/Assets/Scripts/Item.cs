using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public bool grabbable;
    public bool stashable;
    public bool vela;
    public bool papelDeOuro;
    public AudioClip audioClip;
    public string text;
}
