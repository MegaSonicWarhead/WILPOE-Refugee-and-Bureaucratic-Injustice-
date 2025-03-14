using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int currentDay = 1;
    public int totalDays = 90;

    public void AdvanceDay()
    {
        if (currentDay < totalDays)
        {
            currentDay++;
            Debug.Log("Day " + currentDay + " has started.");
        }
        else
        {
            GameManager.Instance.EndGame();
        }
    }
}
