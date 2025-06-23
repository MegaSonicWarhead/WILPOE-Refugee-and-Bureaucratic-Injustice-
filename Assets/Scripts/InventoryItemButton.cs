using UnityEngine;

public class InventoryItemButton : MonoBehaviour
{
    public InventoryItemData itemData;

    public void OnButtonClick()
    {
        InventoryManager.Instance.AddItem(itemData);
    }
}
