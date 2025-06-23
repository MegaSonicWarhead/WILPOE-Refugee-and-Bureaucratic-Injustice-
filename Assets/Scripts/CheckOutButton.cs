using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CheckOutButton : MonoBehaviour
{
    [Header("Item Setup")]
    public InventoryItemData itemData;

    [Header("UI References")]
    public Image iconImage;
    public TMP_Text priceText;

    private void Start()
    {
        if (itemData != null)
        {
            if (iconImage != null) iconImage.sprite = itemData.icon;
            if (priceText != null) priceText.text = $"R {itemData.price}";
        }
        else
        {
            Debug.LogError($"itemData not assigned on {gameObject.name}");
        }
    }

    public void OnBuyButtonClicked()
    {
        if (itemData == null)
        {
            Debug.LogError("Item data is missing!");
            return;
        }

        if (MoneySystem.Instance.SpendMoney(itemData.price))
        {
            InventoryManager.Instance.AddItem(itemData);
        }
        else
        {
            Debug.Log("Not enough money to buy " + itemData.itemName);
        }
    }
}
