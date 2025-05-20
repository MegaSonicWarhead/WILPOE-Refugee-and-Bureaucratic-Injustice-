using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPollutedWaterButton : MonoBehaviour
{
    // Drink from river
    public void OnDrinkPollutedWater()
    {
        PlayerStats.Instance.DrinkPollutedWater();
    }
}
