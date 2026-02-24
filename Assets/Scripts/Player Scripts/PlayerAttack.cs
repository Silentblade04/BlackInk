using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI log;
    public void attack(GameObject target)
    {
        if (target == null)
        {
            log.text = "No target selected";
        }
        TestDummy dummy = target.GetComponent<TestDummy>();

        int attackRoll = Random.Range(0, 20);
        if (attackRoll < 10)
        {
            log.text = attackRoll + " hits the target!";
        }
    }
}
