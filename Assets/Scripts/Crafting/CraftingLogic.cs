using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingLogic : MonoBehaviour
{
    public BowlCollector bowl;
    public GameObject finalMealPrefab;
    public Transform spawnPoint; // Where final meal appears

    public void CraftMeal()
    {
        if (bowl.HasAllIngredients())
        {
            Instantiate(finalMealPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("Meal Crafted!");
        }
        else
        {
            Debug.Log("Missing ingredients!");
        }
    }
}
