using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SleepButton : MonoBehaviour
{
    public TextMeshProUGUI notificationText;
    public GameObject notificationPanel;
    private float robberyChance = 0.1f;
    private int robberyAmount = 50;
    public void OnSleepButtonPressed()
    {
        if (Random.value < robberyChance && MoneySystem.Instance != null)
        {
            MoneySystem.Instance.SpendMoney(robberyAmount);
            if (notificationText != null && notificationPanel != null)
            {
                notificationText.text = "You got Robbed they stole R50!";
                notificationPanel.SetActive(true);
                Invoke("HideNotification", 3f);
            }
        }

        if (GameTime.Instance != null)
        {
            GameTime.Instance.SleepUntilMorning();
        }

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.Sleep(); // Keep this if you also want to recover sanity/etc.
        }
    }

    private void HideNotification()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }
    }
}
