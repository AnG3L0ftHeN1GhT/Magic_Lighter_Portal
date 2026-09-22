using UnityEngine;

public class portalkeeperscript : MonoBehaviour
{
    public GameObject portal;
    private bool puzzleSolved;
    private GameObject portalInstance;

    public void SetPuzzleSolved(bool solved)
    {
        puzzleSolved = solved;
        UpdatePortal();
    }

    private void Start()
    {
        UpdatePortal();
    }

    private void UpdatePortal()
    {
        if (puzzleSolved && portal != null && portalInstance == null)
        {
            portalInstance = Instantiate(portal);
        }
    }
}
