[System.Serializable]
public class InventoryItem 
{
    public InventoryItemData data;
    public int quantity;

    public InventoryItem(InventoryItemData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;
    }
}
