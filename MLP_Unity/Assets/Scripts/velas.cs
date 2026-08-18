using UnityEngine;

public class velas : MonoBehaviour
{
    
    public int corLocal;

    public int corInput;
    public string corChecker;
    public string velaid;
    public bool roxo1= false;
    public bool roxo2= false;
    public bool verde1= false;
    public bool verde2= false;
    public bool gold1= false;
    public bool gold2= false;
    private Animator animator;
     void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    
    void Update()
    {
     //metaif pra acender a particula e o funcionamento do portal.. . . ..  .. . ..
        if (corLocal==corInput)
        {//se a cor acionada pelo player for a cor certa da vela
            if (corChecker=="roxo")
            {//se for (insira cor da vela) torna a var dela true
                if (velaid=="roxo1")
                {
                    roxo1=true;
                }else if (velaid=="roxo2")
                {
                    roxo2=true;
                }
            }else if(corChecker=="dourado")
            {
                if (velaid=="gold1")
                {
                    gold1=true;
                }else if (velaid=="gold2")
                {
                    gold2=true;
                }
            }else if (corChecker == "verde")
            {
            if (velaid=="verde1")
                {
                    verde1=true;
                }else if (velaid=="verde2")
                {
                    verde2=true;
                }
            }
            //so liga as velas na cor q o player escolher
            animator.SetInteger("morte", corInput);
        }
       
    }
    
}
