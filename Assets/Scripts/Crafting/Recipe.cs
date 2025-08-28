using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe")]
public class Recipe : MonoBehaviour
{
    [Header("Ingredients Required")]
    public List<InventoryItemData> requiredItems;

    [Header("Result")]
    public InventoryItemData resultItem;

    public bool Matches(List<InventoryItem> playerItems)
    {
        // Copy inventory counts
        Dictionary<InventoryItemData, int> counts = new Dictionary<InventoryItemData, int>();
        foreach (var item in playerItems)
            counts[item.data] = item.quantity;

        // Check each requirement
        foreach (var req in requiredItems)
        {
            if (!counts.ContainsKey(req) || counts[req] <= 0)
                return false;
            counts[req]--;
        }
        return true;
    }
}
