using UnityEngine;

public class EventManager : MonoBehaviour
{
    public EventData currentEvent;
    //meow
    public EventOutcome TriggerEvent()
    {
        if (currentEvent == null || currentEvent.possibleOutcomes.Length == 0)
        {
            Debug.LogWarning("No event data assigned or no outcomes.");
            return null;
        }

        EventOutcome selected = GetRandomOutcome(currentEvent.possibleOutcomes);
        Debug.Log($"Event Triggered: {currentEvent.eventName}");
        Debug.Log($"Outcome: {selected.outcomeName} - {selected.outcomeDescription}");
        return selected;
    }

    private EventOutcome GetRandomOutcome(EventOutcome[] outcomes)
    {
        int totalWeight = 0;
        foreach (var o in outcomes)
        {
            totalWeight += o.weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var o in outcomes)
        {
            cumulative += o.weight;
            if (randomValue < cumulative)
            {
                return o;
            }
        }

        return outcomes[0];
    }
}
