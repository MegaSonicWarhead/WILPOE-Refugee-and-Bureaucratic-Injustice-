using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobButtonHandler : MonoBehaviour
{
    [Header("Job Settings")]
    public string jobName;
    public int minHours = 4;
    public int maxHours = 10;
    public int moneyPerHour = 2;
    public int sanityLossPerHour = 5;

    [Header("UI References")]
    public GameObject jobPanel;
    public GameObject notificationPanel;
    public TMP_Text notificationText;
    public Slider sanitySlider;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleJobClick);
    }

    void HandleJobClick()
    {
        int hoursWorked = Random.Range(minHours, maxHours + 1);
        int totalMoney = hoursWorked * moneyPerHour;
        int totalSanityLoss = hoursWorked * sanityLossPerHour;

        // Apply time skip
        if (GameTime.Instance != null)
        {
            GameTime.Instance.AdvanceHours(hoursWorked);
        }

        // Apply money
        //if (GameCurrency.Instance != null)
        //{
        //    GameCurrency.Instance.Earn(totalMoney);
        //}

        // Reduce sanity
        if (sanitySlider != null)
        {
            sanitySlider.value = Mathf.Max(sanitySlider.value - totalSanityLoss, 0);
        }

        // Show notification
        jobPanel.SetActive(false);
        notificationPanel.SetActive(true);
        notificationText.text = $"You {jobName} for {hoursWorked} hours. +${totalMoney}, -{totalSanityLoss} Sanity.";
    }
}
