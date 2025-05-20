using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashBodyInRiverButton : MonoBehaviour
{
    // Wash in polluted water
    public void OnWashBodyInRiver()
    {
        PlayerStats.Instance.WashInPollutedWater();
    }
}
