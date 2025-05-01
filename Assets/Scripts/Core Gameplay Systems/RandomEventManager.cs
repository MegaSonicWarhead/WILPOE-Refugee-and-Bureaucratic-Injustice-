// Scripts/RandomEventManager.cs
using UnityEngine;
using TMPro;

public class RandomEventManager : MonoBehaviour
{
    public EventData[] allDailyEvents;

    [Header("UI Elements")]
    public TextMeshProUGUI outcomeTitleText;
    public TextMeshProUGUI outcomeDescriptionText;

    void Start()
    {
        if (GameTime.Instance != null)
            GameTime.Instance.OnNewDay += TriggerRandomDailyEvent;
    }

    void TriggerRandomDailyEvent()
    {
        if (allDailyEvents.Length == 0) return;

        int index = Random.Range(0, allDailyEvents.Length);
        EventData chosenEvent = allDailyEvents[index];

        EventOutcome outcome = GetRandomOutcome(chosenEvent.possibleOutcomes);

        // Log outcome
        Debug.Log($"[Day Event Triggered] {chosenEvent.eventName}");
        Debug.Log($"Outcome: {outcome.outcomeName} - {outcome.outcomeDescription}");

        // Update UI
        if (outcomeTitleText != null)
            outcomeTitleText.text = outcome.outcomeName;

        if (outcomeDescriptionText != null)
            outcomeDescriptionText.text = outcome.outcomeDescription;
    }

    private EventOutcome GetRandomOutcome(EventOutcome[] outcomes)
    {
        int totalWeight = 0;
        foreach (var o in outcomes)
            totalWeight += o.weight;

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var o in outcomes)
        {
            cumulative += o.weight;
            if (roll < cumulative)
                return o;
        }

        return outcomes[0];
    }

    void OnDestroy()
    {
        if (GameTime.Instance != null)
            GameTime.Instance.OnNewDay -= TriggerRandomDailyEvent;
    }
}
