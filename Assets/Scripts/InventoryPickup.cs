using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InventoryPickup : MonoBehaviour
{
    [Header("Item Reference")]
    public InventoryItemData itemData; // Drag in corresponding ScriptableObject

    [Header("Pickup Settings")]
    public bool destroyOnPickup = true;

    private void OnMouseDown()
    {
        if (itemData == null) return;

        InventoryManager.Instance.AddItem(itemData);
        Debug.Log($"Picked up {itemData.itemName}");

        // Add a small scale pop or sound
        // Example: StartCoroutine(PickupEffect());

        if (destroyOnPickup)
            Destroy(gameObject);
    }
}
