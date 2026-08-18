using UnityEngine;

public class BeamAnihilator : MonoBehaviour
{
    public CandleManager cmg;

    public void ObliterateGoldenBeam()
    {
        if (cmg.raioDoradoChecker==false)
        {
            Destroy(this);
        }
    }
}
