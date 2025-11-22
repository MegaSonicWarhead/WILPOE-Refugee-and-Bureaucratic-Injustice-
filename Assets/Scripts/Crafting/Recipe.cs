using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cooking/Recipe", fileName = "NewRecipe")]
public class Recipe : ScriptableObject
{
    [Header("The two required ingredient prefabs")]
    public Ingredient ingredientA;
    public Ingredient ingredientB;

    [Header("The meal prefab to spawn when this recipe matches")]
    public GameObject resultMealPrefab;
}
