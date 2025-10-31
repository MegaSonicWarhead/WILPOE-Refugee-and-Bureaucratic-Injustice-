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
        // Clear existing slots
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        // Populate inventory
        foreach (var item in InventoryManager.Instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.transform.GetChild(0).GetComponent<Image>().sprite = item.data.icon;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = item.quantity.ToString();

            // Add click listener for consuming food
            Button btn = slot.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => UseFoodItem(item));
            }
        }
    }

    private void UseFoodItem(InventoryItem item)
    {
        if (item == null) return;

        // Increase hunger and sanity by 20
        PlayerStats.Instance.ModifyHunger(20f);
        PlayerStats.Instance.ModifySanity(20f);

        Debug.Log($"Consumed {item.data.itemName}: +20 Hunger, +20 Sanity");

        // Remove one quantity from inventory
        InventoryManager.Instance.RemoveItem(item.data);

        // Refresh UI
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
