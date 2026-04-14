using UnityEngine;

public class MultiStagePuzzleController : MonoBehaviour
{
    public bool leverAActivated = false;
    public bool leverBActivated = false;

    public bool IsPuzzleComplete()
    {
        return leverAActivated && leverBActivated;
    }

    public void ActivateLeverA()
    {
        leverAActivated = true;
    }

    public void ActivateLeverB()
    {
        leverBActivated = true;
    }
}