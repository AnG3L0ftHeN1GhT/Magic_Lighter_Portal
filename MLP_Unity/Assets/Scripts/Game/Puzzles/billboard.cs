using UnityEngine;

public class billboard : MonoBehaviour
{
    private Camera playercam;
    public string trava;

    void Start()
    {
        playercam = Camera.main;
    }

    void Update()
    {
        Billboardeffect();
    }

    private void Billboardeffect()
    {
        Vector3 direcao = playercam.transform.position - transform.position;

        if (trava == "verticalobject")
        {
            direcao.y = 0;
            transform.rotation = Quaternion.LookRotation(direcao);
        }
        else if (trava == "horizontalobject")
        {
            direcao.x = 0;
            transform.rotation = Quaternion.LookRotation(direcao);
        }
    
        else if (trava == "desgracadojunior")
        {
            //NAO ACONTECE NADA É IMPOSSIVEL FAZER ESSE OBJECTO DESGRAÇADO RODAR DIREITO MORTE O BILLBOARD
        }
        
    }
}