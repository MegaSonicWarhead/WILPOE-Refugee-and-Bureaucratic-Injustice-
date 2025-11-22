using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlCollector : MonoBehaviour
{
    private List<Ingredient> ingredientsInBowl = new List<Ingredient>();

    private void OnTriggerEnter(Collider other)
    {
        IngredientHolder holder = other.GetComponent<IngredientHolder>();
        if (holder != null && !ingredientsInBowl.Contains(holder.ingredientData))
        {
            ingredientsInBowl.Add(holder.ingredientData);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IngredientHolder holder = other.GetComponent<IngredientHolder>();
        if (holder != null && ingredientsInBowl.Contains(holder.ingredientData))
        {
            ingredientsInBowl.Remove(holder.ingredientData);
        }
    }

    public List<Ingredient> GetIngredientsInBowl()
    {
        return new List<Ingredient>(ingredientsInBowl);
    }

    public void ClearBowl()
    {
        ingredientsInBowl.Clear();
    }
}
