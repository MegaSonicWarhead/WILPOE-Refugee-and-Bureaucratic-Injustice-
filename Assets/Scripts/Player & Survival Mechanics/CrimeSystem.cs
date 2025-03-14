using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimeSystem : MonoBehaviour
{
    public float theftSuccessRate = 0.5f;
    public int penaltyDays = 3;
    public int reputationLoss = 10;

    public void AttemptSteal()
    {
        float chance = UnityEngine.Random.value;
        if (chance < theftSuccessRate)
        {
            Debug.Log("Theft successful! You gained resources but lost reputation.");
            // Add resources and decrease reputation
        }
        else
        {
            Debug.Log("Caught stealing! You lost " + penaltyDays + " days and reputation.");
            TimeManager timeManager = FindObjectOfType<TimeManager>();
            timeManager.currentDay += penaltyDays;
        }
    }
}
