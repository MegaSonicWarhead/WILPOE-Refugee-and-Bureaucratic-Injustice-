using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OfficerType { Nice, Difficult, Lazy, Corrupt }

[CreateAssetMenu(fileName = "OfficerData", menuName = "Officers/New Officer")]
public class OfficerData : ScriptableObject
{
    [Header("Officer Info")]
    public OfficerType officerType;
    public string officerName;
    public Sprite officerImage;

    [Header("Dialogue")]
    [TextArea(3, 10)] public string initialGreeting;

    [Header("Response to Option 1")]
    [TextArea(3, 10)] public string[] responsesOption1;

    [Header("Response to Option 2")]
    [TextArea(3, 10)] public string[] responsesOption2;
}
