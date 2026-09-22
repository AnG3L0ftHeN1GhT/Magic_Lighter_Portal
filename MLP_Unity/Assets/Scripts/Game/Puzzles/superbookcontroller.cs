using UnityEngine;
using UnityEngine.InputSystem;


public class superbookcontroller : MonoBehaviour
{
    public int canalTV = 0;

    public GameObject[] videos;
    public GameObject livropick;
    public GameObject livrocam;
    public GameObject pagina;
    public GameObject roxinhho;

    private Animator animator;
    public bool spinlock = true;





    void Start()
    {
        animator = pagina.GetComponent<Animator>();
        livrocam.SetActive(false);
        roxinhho.SetActive(false);

    }

    void Update()
    {
        for (int i = 0; i < videos.Length; i++)
        {
            if (canalTV == i)
            {
                videos[i].SetActive(true);
            }
            else
            {
                videos[i].SetActive(false);
            }

        }

        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            Debug.Log(spinlock);
            if (spinlock)
            {
                spinlock = false;
            }
            else
            {
                spinlock = true;

            }
            animator.SetBool("girar", spinlock);
        }

    }

    void OnTriggerStay(Collider bgl)
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            if (bgl.CompareTag("xbox"))
            {
                canalTV = canalTV + 1;
                if (canalTV >= videos.Length)
                {
                    canalTV = 0;
                }
            }

            if (bgl.CompareTag("livro"))
            {
                livropick.SetActive(false);
                livrocam.SetActive(true);
                roxinhho.SetActive(true);
            }
            if (bgl.CompareTag("cubolegal"))
            {
                if (GameProcess.Instance != null)
                {
                    GameProcess.Instance.SetPortal();
                }
            }
        }
    }



}