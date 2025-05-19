using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLocationData", menuName = "Map/Location Data")]
public class LocationData : ScriptableObject
{
    public string locationName;
    public int travelCost;
    public string sceneName;
}
