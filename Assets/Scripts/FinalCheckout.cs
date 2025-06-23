using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheckout : MonoBehaviour
{
    public void BuyItem(InventoryItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("[BuyItemHandler] No itemData passed in OnClick.");
            return;
        }

        if (MoneySystem.Instance == null || InventoryManager.Instance == null)
        {
            Debug.LogError("[BuyItemHandler] MoneySystem or InventoryManager not found in scene.");
            return;
        }

        if (MoneySystem.Instance.SpendMoney(itemData.price))
        {
            InventoryManager.Instance.AddItem(itemData);
            Debug.Log($"[BuyItemHandler] Bought item: {itemData.itemName}");
        }
        else
        {
            Debug.Log("Not enough money to buy " + itemData.itemName);
        }
    }
}
