using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkSystem : MonoBehaviour
{
    public int dailyEarnings = 15;

    public void WorkForDay()
    {
        Debug.Log("Worked for the day and earned " + dailyEarnings + " currency.");
    }
}
