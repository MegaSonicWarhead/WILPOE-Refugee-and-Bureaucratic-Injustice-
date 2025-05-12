using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerProgression
{
    FirstVisit,
    AskedPermit,
    HasDocuments,
    CompletedApplication
}

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public PlayerProgression playerProgression = PlayerProgression.FirstVisit;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
