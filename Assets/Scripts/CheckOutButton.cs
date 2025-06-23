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
        iconImage.sprite = itemData.icon;
        priceText.text = $"R {itemData.price}";
    }

    public void OnBuyButtonClicked()
    {
        if (MoneySystem.Instance.SpendMoney(itemData.price))
        {
            InventoryManager.Instance.AddItem(itemData);
        }
        else
        {
            Debug.LogWarning("Not enough money to buy " + itemData.itemName);
        }
    }
}
