using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotParent;
    public GameObject slotPrefab;

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
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
            RefreshInventory();
    }
}
