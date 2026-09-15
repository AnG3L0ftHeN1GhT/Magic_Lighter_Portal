using UnityEngine;

public class superbookcontroller : MonoBehaviour
{
    public int canalTV = 1;

    public GameObject slam;
    public GameObject cuneiforme;
    public GameObject buxavideo;
    private Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (canalTV == 1)
        {
            slam.SetActive(true);
            cuneiforme.SetActive(false);
            buxavideo.SetActive(false);
        }
        
        if (canalTV == 2)
        {
            slam.SetActive(false);
            cuneiforme.SetActive(true);
            buxavideo.SetActive(false);
        }
        
        if (canalTV == 3)
        {
            slam.SetActive(false);
            cuneiforme.SetActive(false);
            buxavideo.SetActive(true);
        }

        if (canalTV > 3)
        {
            canalTV = 1;
        }

        if (Input.GetKeyDown(Keycode.L))
        {

            if (spinlock==true)
            {
                spinlock = true;
            }else if (spinlock == false)
            {
                spinlock = false;
            }
        }
        Girapajinas();
    }

    void OnTriggerEnter(Collider bgl)
    {
        if (bgl.CompareTag("xbox"))
        {
            canalTV = canalTV + 1;
        }

        if (bgl.CompareTag("livro"))
        {
            livropick.SetActive(false);
            livrocam.SetActive(true);
        }
    }

    void Girapajinas()
    {
        animator.SetBool("Grounded",groundedState);
    }

}