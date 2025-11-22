using UnityEngine;
public enum ItemType
{
    Ingredient,
    Meal
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxStackSize = 99;
    public ItemType itemType;


    [Header("Shop")]
    public int price;

    [Header("World Prefab (for Ingredients or Meals)")]
    public GameObject worldPrefab;
}
