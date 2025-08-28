using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StealingJob : MonoBehaviour
{
    [Header("Steal Settings")]
    public Button stealButton;
    public int stealReward = 100;
    public int stealFine = 100;
    public int jailWeeks = 4; // Total jail time in weeks
    private int daysPerWeek = 4; // 4 days per week

    [Header("UI References")]
    public GameObject jobPanel;
    public GameObject MainPanel;
    public GameObject notificationPanel;
    public TMP_Text notificationText;

    private bool stealInProgress = false;

    private void Start()
    {
        if (stealButton != null)
            stealButton.onClick.AddListener(HandleStealClick);
    }

    private void HandleStealClick()
    {
        if (stealInProgress) return;
        stealInProgress = true;

        if (PlayerStats.Instance != null && PlayerStats.Instance.IsInJail())
        {
            ShowNotification($"You cannot steal while in jail! Fine: R{stealFine}");
            Debug.Log("[STEAL] Player is in jail, cannot steal.");
            stealInProgress = false;
            return;
        }

        Debug.Log("[STEAL] Attempting to steal...");

        bool success = Random.value > 0.5f;
        int hoursPerDay = 13;
        int totalJailDays = jailWeeks * daysPerWeek;

        if (success)
        {
            Debug.Log($"[STEAL] SUCCESS! Reward: +R{stealReward}");
            MoneySystem.Instance?.AddMoney(stealReward);

            // Advance 9 hours for successful steal
            if (GameTime.Instance != null)
                GameTime.Instance.AdvanceHours(9);

            ShowNotification($"You successfully stole R{stealReward}!");
        }
        else
        {
            Debug.Log($"[STEAL] FAILED! Fine: -R{stealFine}, Jail for {totalJailDays} days");
            MoneySystem.Instance?.SpendMoney(stealFine);

            // Send player to jail for multiple weeks
            PlayerStats.Instance?.GoToJail(totalJailDays);

            // Advance time according to jail
            if (GameTime.Instance != null)
                GameTime.Instance.AdvanceHours(totalJailDays * hoursPerDay);

            ShowNotification($"You were caught stealing!\nJail: {totalJailDays} days\nFine: R{stealFine}");
        }

        TogglePanels();
        stealInProgress = false;
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
