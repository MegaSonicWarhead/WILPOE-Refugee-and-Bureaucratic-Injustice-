using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyHandler : MonoBehaviour
{
    [Header("Item to Sell")]
    public InventoryItemData itemData;

    public void Buy()
    {
        if (itemData == null)
        {
            Debug.LogError("[ShopItemSlot] No item assigned to shop slot.");
            return;
        }

        if (MoneySystem.Instance.SpendMoney(itemData.price))
        {
            InventoryManager.Instance.AddItem(itemData);
            Debug.Log($"[ShopItemSlot] Bought {itemData.itemName} for R{itemData.price}");
        }
        else
        {
            Debug.LogWarning($"[ShopItemSlot] Not enough money for {itemData.itemName}");
        }
    }
}
