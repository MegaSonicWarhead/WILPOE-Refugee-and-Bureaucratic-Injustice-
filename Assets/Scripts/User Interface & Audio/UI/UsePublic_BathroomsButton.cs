using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UsePublic_BathroomsButton : MonoBehaviour
{
    public Button useBathroomButton;

    private void Start()
    {
        if (useBathroomButton != null)
            useBathroomButton.onClick.AddListener(UseBathroom);
        else
            Debug.LogWarning("[UsePublic_BathroomsButton] Button reference is missing!");
    }

    private void UseBathroom()
    {
        // Spend R10
        if (!MoneySystem.Instance.SpendMoney(10))
        {
            Debug.LogWarning("[PublicBathroom] Not enough money to use the bathroom!");
            return;
        }

        PlayerStats stats = PlayerStats.Instance;

        // Restore sanity to max
        stats.sanity = 100f;

        // Restore health +10
        stats.ModifyHealth(+10f);

        // Update UI manually
      //  stats.UpdateUI();

        Debug.Log("[PublicBathroom] Player used public bathroom. Sanity restored & +10 health.");
    }
}
