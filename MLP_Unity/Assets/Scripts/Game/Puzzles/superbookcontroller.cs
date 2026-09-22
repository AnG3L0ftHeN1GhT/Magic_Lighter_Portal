using UnityEngine;
using UnityEngine.InputSystem;


public class superbookcontroller : MonoBehaviour
{
    public int canalTV = 1;

    public GameObject slam;
    public GameObject cuneiforme;
    public GameObject buxavideo;
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

        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            Debug.Log(spinlock);
            if (spinlock==true)
            {
                spinlock = false;
            }else if (spinlock == false)
            {
                spinlock = true;
                
            }
        }
        Girapajinas();
    }

    void OnTriggerStay(Collider bgl)
    {
        if(Input.GetMouseButtonDown(0)||Input.GetMouseButton(0)){
        if (bgl.CompareTag("xbox"))
        {
            canalTV = canalTV + 1;
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

    void Girapajinas()
    {
        animator.SetBool("girar",spinlock);
    }

}