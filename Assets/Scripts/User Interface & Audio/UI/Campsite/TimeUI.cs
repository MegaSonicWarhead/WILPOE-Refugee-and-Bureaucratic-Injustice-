using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI weekText;

    private bool isSubscribed = false;

    void Start()
    {
        TrySubscribeToGameTime();
    }

    void Update()
    {
        // If GameTime loads late (e.g. scene async), try again
        if (!isSubscribed && GameTime.Instance != null)
        {
            TrySubscribeToGameTime();
        }
    }

    void TrySubscribeToGameTime()
    {
        if (GameTime.Instance == null) return;

        GameTime.Instance.OnTimeChanged += UpdateUI;
        GameTime.Instance.OnNewDay += UpdateUI;
        GameTime.Instance.OnNewWeek += UpdateUI;

        isSubscribed = true;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (GameTime.Instance == null) return;

        timeText.text = GameTime.Instance.GetFormattedTime();
        dayText.text = GameTime.Instance.GetDayInfo();
        weekText.text = GameTime.Instance.GetWeekInfo();
    }
}
