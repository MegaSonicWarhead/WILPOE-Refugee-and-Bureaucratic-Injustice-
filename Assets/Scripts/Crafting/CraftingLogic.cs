using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingLogic : MonoBehaviour
{
    [Header("Reference to the bowl collector (trigger zone)")]
    public BowlCollector bowlCollector;

    [Header("Where the crafted meal will appear")]
    public Transform spawnPoint;

    [Header("All possible recipes")]
    public List<Recipe> recipes = new List<Recipe>();

    public void Craft()
    {
        // Get all ingredients currently in the bowl
        List<Ingredient> currentIngredients = bowlCollector.GetIngredientsInBowl();

        if (currentIngredients.Count < 2)
        {
            Debug.Log("Not enough ingredients to craft.");
            return;
        }

        // Try every recipe
        foreach (var recipe in recipes)
        {
            if (currentIngredients.Contains(recipe.ingredientA) &&
                currentIngredients.Contains(recipe.ingredientB))
            {
                // Found a match
                Instantiate(recipe.resultMealPrefab, spawnPoint.position, Quaternion.identity);
                Debug.Log($"Crafted: {recipe.resultMealPrefab.name}");
                bowlCollector.ClearBowl();
                return;
            }
        }

        Debug.Log("No matching recipe found.");
    }
}
