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
        button.onClick.AddListener(HandleJobClick);
    }

    void HandleJobClick()
    {
        int hoursWorked = Random.Range(minHours, maxHours + 1);
        int totalMoney = hoursWorked * moneyPerHour;
        int totalSanityLoss = hoursWorked * sanityLossPerHour;

        // Advance game time
        if (GameTime.Instance != null)
        {
            GameTime.Instance.AdvanceHours(hoursWorked);
        }

        // Add money using MoneySystem
        if (MoneySystem.Instance != null)
        {
            MoneySystem.Instance.AddMoney(totalMoney);
        }

        // Reduce sanity using PlayerStats
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ModifySanity(-totalSanityLoss);
        }

        // Update UI
        if (jobPanel != null) jobPanel.SetActive(false);
        if (MainPanel != null) MainPanel.SetActive(true);
        if (notificationPanel != null) notificationPanel.SetActive(true);
        if (notificationText != null)
        {
            notificationText.text = $"You {jobName} for {hoursWorked} hours.\n+R{totalMoney}, -{totalSanityLoss} Sanity.";
        }
    }
}
