using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotParent;
    public GameObject slotPrefab;

    private void Update()
    {
        // Close inventory if Escape is pressed and panel is active
        if (inventoryPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryPanel.SetActive(false);
        }
    }

    public void RefreshInventory()
    {
        // ✅ Clear existing slots first
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        // ✅ Safety: make sure we have a valid list
        if (InventoryManager.Instance == null || InventoryManager.Instance.items == null)
            return;

        // ✅ Populate slots
        foreach (var item in InventoryManager.Instance.items)
        {
            // Create a local copy to avoid closure issues
            InventoryItem currentItem = item;

            // Instantiate the slot prefab under the parent
            GameObject slot = Instantiate(slotPrefab, slotParent);

            // --- ICON ---
            Image icon = slot.transform.GetChild(0).GetComponent<Image>();
            if (icon != null)
                icon.sprite = currentItem.data.icon;

            // --- QUANTITY ---
            TMP_Text qtyText = slot.transform.GetChild(1).GetComponent<TMP_Text>();
            if (qtyText != null)
                qtyText.text = currentItem.quantity.ToString();

            // --- BUTTON ---
            Button btn = slot.GetComponent<Button>();
            if (btn == null)
            {
                // Add a Button automatically if missing
                btn = slot.AddComponent<Button>();
                var bg = slot.GetComponent<Image>();
                if (bg == null)
                    bg = slot.AddComponent<Image>(); // required for Button to detect clicks
                bg.raycastTarget = true;
            }

            // Clear any previous listeners
            btn.onClick.RemoveAllListeners();

            // Add the click action
            btn.onClick.AddListener(() =>
            {
                Debug.Log($"[INVENTORY] Clicked on {currentItem.data.itemName}");

                // Use the item
                UseFoodItem(currentItem);
            });
        }
    }

    private void UseFoodItem(InventoryItem item)
    {
        if (item == null || item.data == null)
        {
            Debug.LogWarning("[INVENTORY] Tried to use null item!");
            return;
        }

        // Example: Only apply effects if it's an edible item
        Debug.Log($"[INVENTORY] Using {item.data.itemName} (+20 Hunger, +20 Sanity)");

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ModifyHunger(+20f);
            PlayerStats.Instance.ModifySanity(+20f);
        }

        // Remove item after consuming
        InventoryManager.Instance.RemoveItem(item.data);

        // Refresh inventory UI after change
        RefreshInventory();
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);

        if (!isActive)
            RefreshInventory();
    }
}
