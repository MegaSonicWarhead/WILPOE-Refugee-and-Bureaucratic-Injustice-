using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHandler : MonoBehaviour
{
    [Header("Item To Buy")]
    public InventoryItemData itemData;

    [Header("Optional UI Reference")]
    public Button buyButton;

    private void Start()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(Buy);
    }

    public void Buy()
    {
        if (itemData == null)
        {
            Debug.LogError("[BuyHandler] No item assigned to this button!");
            return;
        }

        if (MoneySystem.Instance == null)
        {
            Debug.LogError("[BuyHandler] No MoneySystem found!");
            return;
        }

        if (MoneySystem.Instance.SpendMoney(itemData.price))
        {
            InventoryManager.Instance.AddItem(itemData);
            Debug.Log($"[BuyHandler] Bought {itemData.itemName} for R{itemData.price}");
        }
        else
        {
            Debug.LogWarning($"[BuyHandler] Not enough money for {itemData.itemName}");
        }
    }
}
