using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public List<Recipe> allRecipes = new List<Recipe>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryCraft(Recipe recipe)
    {
        var items = InventoryManager.Instance.items;

        if (recipe.Matches(items))
        {
            // ✅ Remove required items
            foreach (var req in recipe.requiredItems)
            {
                InventoryItem invItem = items.Find(i => i.data == req);
                if (invItem != null)
                {
                    invItem.quantity--;
                    if (invItem.quantity <= 0) items.Remove(invItem);
                }
            }

            // ✅ Add result
            InventoryManager.Instance.AddItem(recipe.resultItem);

            Debug.Log($"[CraftingManager] Crafted {recipe.resultItem.itemName}");
            return true;
        }

        Debug.LogWarning("[CraftingManager] Recipe requirements not met!");
        return false;
    }
}
