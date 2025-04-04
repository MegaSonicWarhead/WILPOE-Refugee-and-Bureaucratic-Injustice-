using UnityEngine;

public class ActionSlot : MonoBehaviour
{
    [Range(1, 6)] public int difficulty = 3;

    public void AttemptAction(Dice dice)
    {
        if (dice == null || dice.isUsed) return;

        if (dice.value >= difficulty)
        {
            Debug.Log("Action succeeded!");
            // Add success effects (e.g. progress, resource, approval)
        }
        else
        {
            Debug.Log("Action failed.");
            // Add failure effects (e.g. stress, loss, delay)
        }

        dice.Use();
        FindObjectOfType<EventManager>().UseDice(dice);
    }
}
