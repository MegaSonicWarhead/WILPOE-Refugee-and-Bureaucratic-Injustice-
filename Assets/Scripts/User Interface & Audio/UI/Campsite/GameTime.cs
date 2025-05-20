using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static GameTime Instance;

    public int currentWeek = 0;
    public int currentDay = 0;
    public int currentHour = 8;
    public int currentMinute = 0;

    public float timeSpeed = 60f; // 1 real second = 1 in-game minute
    private float timeAccumulator = 0f;

    // Events for other systems to hook into
    public event Action OnNewDay;
    public event Action OnNewWeek;
    public event Action OnTimeChanged;

    private void Awake()
    {
        // Make sure there's only one GameTime instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    private void Update()
    {
        // Add delta time and convert to in-game time
        timeAccumulator += Time.deltaTime * timeSpeed;

        while (timeAccumulator >= 60f)
        {
            timeAccumulator -= 60f;
            AdvanceMinute();
        }
    }

    private void AdvanceMinute()
    {
        currentMinute++;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;

            if (currentHour >= 21)
            {
                EndDay();
            }
        }

        OnTimeChanged?.Invoke(); // Notify subscribers
    }

    private void EndDay()
    {
        currentHour = 8;
        currentMinute = 0;

        currentDay++;

        if (currentDay >= 4)
        {
            currentDay = 0;
            currentWeek++;

            if (currentWeek > 12)
            {
                Debug.Log("All game weeks finished!");
                return;
            }

            OnNewWeek?.Invoke(); // Notify new week
        }

        OnNewDay?.Invoke(); // Notify new day
    }

    // Returns time in HH:MM format
    public string GetFormattedTime()
    {
        return $"{currentHour:D2}:{currentMinute:D2}";
    }

    // Returns readable day info
    public string GetDayInfo()
    {
        return $"Day {currentDay + 1}";
    }

    // Returns readable week info (e.g., "Tutorial Week" or "Week 1")
    public string GetWeekInfo()
    {
        return currentWeek == 0 ? "Tutorial Week" : $"Week {currentWeek}";
    }

    // Skips time (for testing or gameplay systems)
    public void AdvanceHours(int hours)
    {
        for (int i = 0; i < hours * 60; i++)
            AdvanceMinute();
    }
}
