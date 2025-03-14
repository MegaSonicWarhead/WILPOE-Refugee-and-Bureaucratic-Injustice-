using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<string> possibleEvents = new List<string> { "Theft", "Loadshedding", "Illness", "Bureaucratic Delay" };

    public void TriggerRandomEvent()
    {
        string selectedEvent = possibleEvents[UnityEngine.Random.Range(0, possibleEvents.Count)];
        Debug.Log("Event Triggered: " + selectedEvent);
    }
}
