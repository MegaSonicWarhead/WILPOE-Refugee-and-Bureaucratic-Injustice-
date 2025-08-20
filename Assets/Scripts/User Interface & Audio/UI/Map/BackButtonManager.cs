using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonManager : MonoBehaviour
{
    public static BackButtonManager Instance;

    private Stack<string> sceneHistory = new Stack<string>();

    private void Awake()
    {
        Debug.Log($"[BackButtonManager] Awake in scene: {SceneManager.GetActiveScene().name}, object: {gameObject.name}");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log($"[BackButtonManager] Created and persisted. Instance set to: {gameObject.name}");
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"[BackButtonManager] Duplicate detected. Destroying object: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Debug.Log($"[BackButtonManager] OnEnable called for {gameObject.name}");
    }

    private void Start()
    {
        Debug.Log($"[BackButtonManager] Start called in scene: {SceneManager.GetActiveScene().name}");
    }

    private void OnDisable()
    {
        Debug.Log($"[BackButtonManager] OnDisable called for {gameObject.name}");
    }

    private void OnDestroy()
    {
        Debug.LogError($"[BackButtonManager] OnDestroy called for {gameObject.name} in scene: {SceneManager.GetActiveScene().name}");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneHistory.Count == 0 || scene.name != sceneHistory.Peek())
        {
            sceneHistory.Push(scene.name);
            Debug.Log($"[BackButtonManager] Pushed scene to history: {scene.name} (stack size: {sceneHistory.Count})");
        }
    }

    public void GoBack()
    {
        if (sceneHistory.Count > 1)
        {
            string current = sceneHistory.Pop();
            string previousScene = sceneHistory.Peek();
            Debug.Log($"[BackButtonManager] Going back from {current} to {previousScene}");
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.Log("[BackButtonManager] No previous scene to go back to.");
        }
    }
}
