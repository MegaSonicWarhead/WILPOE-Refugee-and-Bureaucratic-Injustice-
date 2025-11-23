using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CraftSnenLoad : MonoBehaviour
{
    public void LoadFoodCraftingScene()
    {
        string sceneName = "Food Crafting";

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"[SceneLoader] Scene '{sceneName}' not found! Make sure it’s added to Build Settings.");
        }
    }

    // Optional: quit button function
    public void QuitGame()
    {
        Debug.Log("[SceneLoader] Quitting game...");
        Application.Quit();
    }

}
