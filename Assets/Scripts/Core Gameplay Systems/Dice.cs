using UnityEngine;

public class Dice
{
    public int value { get; private set; }
    public bool isUsed { get; private set; }

    public void Roll()
    {
        value = Random.Range(1, 7);
        isUsed = false;
    }

    public void Use()
    {
        isUsed = true;
    }
}
