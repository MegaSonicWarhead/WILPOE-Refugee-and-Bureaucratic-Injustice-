using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlCollector : MonoBehaviour
{
   public List<string> requiredIngredients; // Set in Inspector (ex: "Tomato", "Cheese", "Bread")
    private List<string> currentIngredients = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        Ingrediants ingredient = other.GetComponent<Ingrediants>();
        if (ingredient != null)
        {
            if (!currentIngredients.Contains(ingredient.ingredientName))
            {
                currentIngredients.Add(ingredient.ingredientName);
                Debug.Log(ingredient.ingredientName + " added to bowl.");
            }
        }
    }

    public bool HasAllIngredients()
    {
        foreach (string required in requiredIngredients)
        {
            if (!currentIngredients.Contains(required))
                return false;
        }
        return true;
    }
}
