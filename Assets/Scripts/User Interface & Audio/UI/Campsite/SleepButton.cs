using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepButton : MonoBehaviour
{
    // Sleep button
    public void OnSleepButtonPressed()
    {
        PlayerStats.Instance.Sleep();
    }
}
