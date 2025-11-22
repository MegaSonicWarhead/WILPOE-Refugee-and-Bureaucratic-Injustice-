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
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in InventoryManager.Instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.transform.GetChild(0).GetComponent<Image>().sprite = item.data.icon;
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = item.quantity.ToString();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool isActive = inventoryPanel.activeSelf;

        if (!isActive)
        {
            // Opening the inventory → refresh contents
            inventoryPanel.SetActive(true);
            RefreshInventory();
        }
        else
        {
            // Closing the inventory
            inventoryPanel.SetActive(false);
        }
    }
}
