using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public int dicePerDay = 5;
    public List<Dice> currentDice = new List<Dice>();
    public List<string> possibleEvents = new List<string> { "Theft", "Loadshedding", "Illness", "Bureaucratic Delay" };
    public delegate void OnDiceRolled(List<Dice> dice);
    public static event OnDiceRolled OnDiceRolledEvent;

    private void Start()
    {
        RollNewDay();
    }
    public void TriggerRandomEvent()
    {
        string selectedEvent = possibleEvents[UnityEngine.Random.Range(0, possibleEvents.Count)];
        Debug.Log("Event Triggered: " + selectedEvent);
    }
    public void RollNewDay()
    {
        currentDice.Clear();
        for (int i = 0; i < dicePerDay; i++)
        {
            Dice dice = new Dice();
            dice.Roll();
            currentDice.Add(dice);
        }

        OnDiceRolledEvent?.Invoke(currentDice);
    }

    public void UseDice(Dice dice)
    {
        currentDice.Remove(dice);
        if (currentDice.Count == 0)
        {
            // Trigger end-of-day
            Debug.Log("No dice left. End of the day.");
        }
    }
}
