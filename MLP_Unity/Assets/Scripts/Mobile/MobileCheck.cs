using UnityEngine;
using UnityEngine.InputSystem;

public class MobileCheck : MonoBehaviour
{
    public GameObject mobileStuff;
    void Start()
    {
        if ((Application.platform != RuntimePlatform.Android))
        {
            Destroy(gameObject);
        }
        else
        {
            mobileStuff.SetActive(true);
        }
    }
}
