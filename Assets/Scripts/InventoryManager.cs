using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> items = new List<InventoryItem>();
    public InventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(InventoryItemData newItemData)
    {
        InventoryItem existingItem = items.Find(i => i.data == newItemData);
        if (existingItem != null)
        {
            existingItem.quantity++;
        }
        else
        {
            items.Add(new InventoryItem(newItemData, 1));
        }

        inventoryUI.RefreshInventory();
    }

    // ✅ Check if inventory contains this item
    public bool HasItem(InventoryItemData itemData)
    {
        return items.Exists(i => i.data == itemData && i.quantity > 0);
    }

    // ✅ Remove an item (decrease quantity or remove completely)
    public bool RemoveItem(InventoryItemData itemData)
    {
        InventoryItem existingItem = items.Find(i => i.data == itemData);
        if (existingItem != null && existingItem.quantity > 0)
        {
            existingItem.quantity--;

            if (existingItem.quantity <= 0)
                items.Remove(existingItem);

            inventoryUI.RefreshInventory();
            return true;
        }
        return false;
    }
}
