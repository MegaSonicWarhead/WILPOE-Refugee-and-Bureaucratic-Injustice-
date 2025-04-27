using UnityEngine;

[CreateAssetMenu(fileName = "NewEvent", menuName = "RefugeeClicker/Event")]
public class EventData : ScriptableObject
{
    public string eventName;
    public EventOutcome[] possibleOutcomes;
}
