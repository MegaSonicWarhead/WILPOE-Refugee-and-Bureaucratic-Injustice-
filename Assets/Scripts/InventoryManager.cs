using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> items = new List<InventoryItem>();
    public InventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 🔥 Keep inventory across scenes

            // Reconnect UI when a new scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ✅ Try to find a new InventoryUI in the new scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InventoryUI foundUI = Object.FindFirstObjectByType<InventoryUI>();
        if (foundUI != null)
        {
            inventoryUI = foundUI;
            inventoryUI.RefreshInventory();
            Debug.Log($"[InventoryManager] Reconnected InventoryUI in scene: {scene.name}");
        }
        else
        {
            Debug.LogWarning($"[InventoryManager] No InventoryUI found in scene: {scene.name}");
        }
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

        if (inventoryUI != null)
            inventoryUI.RefreshInventory();
    }

    public bool HasItem(InventoryItemData itemData)
    {
        return items.Exists(i => i.data == itemData && i.quantity > 0);
    }

    public bool RemoveItem(InventoryItemData itemData)
    {
        InventoryItem existingItem = items.Find(i => i.data == itemData);
        if (existingItem != null && existingItem.quantity > 0)
        {
            existingItem.quantity--;

            if (existingItem.quantity <= 0)
                items.Remove(existingItem);

            if (inventoryUI != null)
                inventoryUI.RefreshInventory();

            return true;
        }
        return false;
    }

}
