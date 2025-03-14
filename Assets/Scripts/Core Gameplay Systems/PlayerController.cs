using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public int hunger = 100;
    public int money = 50;
    public int sanity = 100;

    public void Eat(int foodValue)
    {
        hunger += foodValue;
        if (hunger > 100) hunger = 100;
        Debug.Log("Ate food. Hunger: " + hunger);
    }

    public void SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            Debug.Log("Spent " + amount + " money. Remaining: " + money);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void ReduceSanity(int amount)
    {
        sanity -= amount;
        if (sanity < 0) sanity = 0;
        Debug.Log("Sanity reduced! Current: " + sanity);
    }
}
