using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public static IngredientSpawner Instance;

    [Header("Spawn Points (Plates)")]
    public List<Transform> plateSpots = new List<Transform>();

    [Header("Settings")]
    public bool skipOccupiedSpots = false; // If true, won't spawn where something already exists
    public float yOffset = 0.05f; // Slight lift so it sits above the plate

    private int nextSpotIndex = 0; // Which plate to use next

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnIngredient(InventoryItemData data)
    {
        if (data == null || data.worldPrefab == null)
        {
            Debug.LogWarning("[IngredientSpawner] Missing prefab for " + (data != null ? data.itemName : "null item"));
            return;
        }

        if (plateSpots.Count == 0)
        {
            Debug.LogError("[IngredientSpawner] No plate spots assigned!");
            return;
        }

        // Find a valid spot
        Transform targetSpot = GetNextAvailableSpot();
        if (targetSpot == null)
        {
            Debug.LogWarning("[IngredientSpawner] No available plate spots!");
            return;
        }

        // Spawn ingredient slightly above the plate
        Vector3 spawnPos = targetSpot.position + Vector3.up * yOffset;
        Quaternion spawnRot = targetSpot.rotation;

        GameObject spawnedIngredient = Instantiate(data.worldPrefab, spawnPos, spawnRot, targetSpot);

        Debug.Log($"[IngredientSpawner] Spawned {data.itemName} on plate {targetSpot.name}");
    }

    private Transform GetNextAvailableSpot()
    {
        int attempts = 0;
        while (attempts < plateSpots.Count)
        {
            Transform candidate = plateSpots[nextSpotIndex];

            // Move to next for next time
            nextSpotIndex = (nextSpotIndex + 1) % plateSpots.Count;

            // Check if the spot is occupied
            if (skipOccupiedSpots)
            {
                if (candidate.childCount == 0)
                    return candidate; // Empty, valid spot
            }
            else
            {
                return candidate; // Always valid
            }

            attempts++;
        }

        // If all are full
        return null;
    }
}
