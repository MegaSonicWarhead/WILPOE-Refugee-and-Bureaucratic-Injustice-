using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadRatButton : MonoBehaviour
{
    public Button deadRatButton;

    private void Start()
    {
        if (deadRatButton != null)
            deadRatButton.onClick.AddListener(EatDeadRat);
        else
            Debug.LogWarning("[DeadRatButton] Button reference missing!");
    }

    private void EatDeadRat()
    {
        PlayerStats stats = PlayerStats.Instance;

        // +10 Hunger
        stats.ModifyHunger(+10f);

        // -5 Health
        stats.ModifyHealth(-5f);

        // -5 Sanity
        stats.ModifySanity(-5f);

        // Update UI
        stats.UpdateUI();

        Debug.Log("[DeadRatButton] Player ate a dead rat: +10 Hunger, -5 Health, -5 Sanity.");
    }
}
