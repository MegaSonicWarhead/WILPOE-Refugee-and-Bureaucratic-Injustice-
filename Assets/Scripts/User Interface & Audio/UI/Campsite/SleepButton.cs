using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepButton : MonoBehaviour
{
    public void OnSleepButtonPressed()
    {
        if (GameTime.Instance != null)
        {
            GameTime.Instance.SleepUntilMorning();
        }

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.Sleep(); // Keep this if you also want to recover sanity/etc.
        }
    }
}
