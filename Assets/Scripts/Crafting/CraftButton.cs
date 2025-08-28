using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    public Recipe recipe;
    public Button craftButton;

    private void Start()
    {
        craftButton.onClick.AddListener(() =>
        {
            CraftingManager.Instance.TryCraft(recipe);
            InventoryManager.Instance.inventoryUI.RefreshInventory();
        });
    }
}
