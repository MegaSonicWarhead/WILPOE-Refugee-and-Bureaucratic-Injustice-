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
			Debug.Log($"[BackButtonManager] Created and persisted. Object: {gameObject.name}");
		}
		else
		{
			Debug.LogWarning($"[BackButtonManager] Duplicate detected. Destroying: {gameObject.name}");
			Destroy(gameObject);
		}
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
			// Remove current scene
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
