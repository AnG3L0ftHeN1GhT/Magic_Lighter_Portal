using UnityEngine;

public class CandleManager : MonoBehaviour
{
    public GameObject portal;
    public GameObject bgldorado;
    public GameObject bglroxo;
    public GameObject bglverde;
    public velas vr1;
    public velas vr2;
    public velas vv1;
    public velas vv2;
    public velas vg1;
    public velas vg2;
    public bool raioRoxoChecker = false;
    public bool raioVerdeChecker = false;
    public bool raioDoradoChecker = false;
    private bool roxoInvocado = false;
    private bool verdeInvocado = false;
    private bool doradoInvocado = false;
    private bool portalInvocado = false;

    void Update()
    {
        //super raios legais
        if (vr1.roxo1==true&&vr2.roxo2==true&&roxoInvocado==false)
        {
            Instantiate(bglroxo);
            raioRoxoChecker = true;
            roxoInvocado = true;
        }
        if (vv1.verde1==true&&vv2.verde2==true&&verdeInvocado==false)
        {
            Instantiate(bglverde);
            raioVerdeChecker = true;
            verdeInvocado = true;
        }
        if (vg1.gold1==true&&vg2.gold2==true&&doradoInvocado==false)
        {
            Instantiate(bgldorado);
            raioDoradoChecker = true;
            doradoInvocado = true;
        }
        //prtal legal
        if (vr1.roxo1==true&&vr2.roxo2==true&&vv1.verde1==true&&vv2.verde2==true&&vg1.gold1==true&&vg2.gold2==true&&portalInvocado)
        {
            Instantiate(portal);
            portalInvocado = true;
        }    
    }

}