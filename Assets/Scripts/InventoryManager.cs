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
}
