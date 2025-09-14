using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneLoader : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log($"[MapSceneLoader] Awake in scene: {SceneManager.GetActiveScene().name}, object: {gameObject.name}");
    }

    private void OnEnable()
    {
        Debug.Log($"[MapSceneLoader] OnEnable called for {gameObject.name}");
    }

    private void Start()
    {
        Debug.Log($"[MapSceneLoader] Start called in scene: {SceneManager.GetActiveScene().name}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log($"[MapSceneLoader] 'M' pressed in scene {SceneManager.GetActiveScene().name}. Attempting to load 'Map'");
            SceneManager.LoadScene("Map"); // Make sure your map scene is actually called "Map"
        }
    }

    private void OnDisable()
    {
        Debug.Log($"[MapSceneLoader] OnDisable called for {gameObject.name}");
    }

    private void OnDestroy()
    {
        //Debug.LogError($"[MapSceneLoader] OnDestroy called for {gameObject.name} in scene: {SceneManager.GetActiveScene().name}");
    }
}
