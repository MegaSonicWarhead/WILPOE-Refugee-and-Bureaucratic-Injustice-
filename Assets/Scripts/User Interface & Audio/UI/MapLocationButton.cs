using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapLocationButton : MonoBehaviour
{
    [Header("Location Info")]
    public LocationData locationData;

    [Header("UI References")]
    public TMP_Text costText;
    public TMP_Text popupText;
    public GameObject popupPanel;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (locationData == null)
        {
            Debug.LogError("LocationData not assigned on MapLocationButton.");
            return;
        }

        if (costText != null)
        {
            // Display like "HomeAffairs - $100"
            costText.text = $"{locationData.locationName} - ${locationData.travelCost}";
        }

        popupPanel.SetActive(false);
        button.onClick.AddListener(OnLocationClicked);
    }

    void OnLocationClicked()
    {
        //if (GameCurrency.Instance == null)
        //{
        //    Debug.LogError("GameCurrency instance not found.");
        //    return;
        //}

        //if (GameCurrency.Instance.CanAfford(locationData.travelCost))
        //{
        //    GameCurrency.Instance.Spend(locationData.travelCost);
        //    SceneManager.LoadScene(locationData.sceneName);
        //}
        //else
        //{
        //    ShowNotEnoughMoneyPopup();
        //}
    }

    void ShowNotEnoughMoneyPopup()
    {
        if (popupPanel != null && popupText != null)
        {
            popupPanel.SetActive(true);
            popupText.text = $"You don’t have enough money to travel to {locationData.locationName}.";
            CancelInvoke(nameof(HidePopup));
            Invoke(nameof(HidePopup), 2.5f);
        }
    }

    void HidePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}
