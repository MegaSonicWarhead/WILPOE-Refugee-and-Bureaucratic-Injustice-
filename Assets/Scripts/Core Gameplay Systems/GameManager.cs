using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TimeManager timeManager;
    public EventManager eventManager;
   // public PlayerSurvival player;

    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        timeManager = GetComponent<TimeManager>();
        eventManager = GetComponent<EventManager>();
    }

    public void EndGame()
    {
        isGameOver = true;
        Debug.Log("Game Over. You ran out of time or died.");
    }
}
