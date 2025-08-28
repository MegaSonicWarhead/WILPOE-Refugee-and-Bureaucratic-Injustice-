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
    public GameObject MainPanel;
    public GameObject notificationPanel;
    public TMP_Text notificationText;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(HandleJobClick);
    }

    private void HandleJobClick()
    {
        int hoursWorked = Random.Range(minHours, maxHours + 1);
        int totalMoney = hoursWorked * moneyPerHour;
        int totalSanityLoss = hoursWorked * sanityLossPerHour;

        Debug.Log($"[JOB] {jobName} for {hoursWorked}h | +R{totalMoney} | -{totalSanityLoss} Sanity");

        GameTime.Instance?.AdvanceHours(hoursWorked);
        MoneySystem.Instance?.AddMoney(totalMoney);
        PlayerStats.Instance?.ModifySanity(-totalSanityLoss);

        TogglePanels();
        ShowNotification($"You {jobName} for {hoursWorked} hours.\n+R{totalMoney}, -{totalSanityLoss} Sanity.");
    }

    private void ShowNotification(string message)
    {
        Debug.Log($"[NOTIFICATION] {message}");
        if (notificationPanel != null) notificationPanel.SetActive(true);
        if (notificationText != null) notificationText.text = message;
    }

    private void TogglePanels()
    {
        if (jobPanel != null) jobPanel.SetActive(false);
        if (MainPanel != null) MainPanel.SetActive(true);
    }
}
