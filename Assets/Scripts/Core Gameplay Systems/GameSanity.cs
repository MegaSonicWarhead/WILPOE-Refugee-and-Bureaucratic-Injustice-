using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSanity : MonoBehaviour
{
    public static GameSanity Instance;
    public int CurrentSanity = 100;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Reduce(int amount)
    {
        CurrentSanity = Mathf.Max(0, CurrentSanity - amount);
    }
}
