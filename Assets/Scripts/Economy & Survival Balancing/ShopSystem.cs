using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public Dictionary<string, int> shopItems = new Dictionary<string, int>
    {
        { "Food", 10 },
        { "Water", 5 },
        { "Blanket", 20 }
    };

    public void BuyItem(string itemName, int playerMoney)
    {
        if (shopItems.ContainsKey(itemName))
        {
            int price = shopItems[itemName];
            if (playerMoney >= price)
            {
                Debug.Log("Bought " + itemName + " for " + price + " currency.");
            }
            else
            {
                Debug.Log("Not enough money to buy " + itemName);
            }
        }
    }
}
