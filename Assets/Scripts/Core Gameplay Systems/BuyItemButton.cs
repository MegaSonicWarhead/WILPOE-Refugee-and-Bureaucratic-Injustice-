// Scripts/BuyItemButton.cs
using UnityEngine;
using UnityEngine.UI;

public class BuyItemButton : MonoBehaviour
{
    public int itemCost = 50; // Set this in Inspector

    public void TryBuyItem()
    {
        if (MoneySystem.Instance != null)
        {
            bool success = MoneySystem.Instance.SpendMoney(itemCost);

            if (success)
            {
                Debug.Log("Purchase successful!");
                // Optional: trigger effects like giving item or feedback
            }
            else
            {
                Debug.Log("Not enough money to buy this.");
            }
        }
        else
        {
            Debug.LogError("MoneySystem not found!");
        }
    }
}
