using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonManager : MonoBehaviour
{
    public static BackButtonManager Instance;

    private Stack<string> sceneHistory = new Stack<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneHistory.Count == 0 || scene.name != sceneHistory.Peek())
        {
            sceneHistory.Push(scene.name);
        }
    }

    public void GoBack()
    {
        if (sceneHistory.Count > 1)
        {
            // Remove current scene
            sceneHistory.Pop();

            // Load previous scene
            string previousScene = sceneHistory.Peek();
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.Log("No previous scene to go back to.");
        }
    }
}
