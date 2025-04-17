using UnityEngine;

[System.Serializable]
public class EventOutcome
{
    public string outcomeName;
    [TextArea]
    public string outcomeDescription;
    public int weight; 
}
