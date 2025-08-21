using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneLoader : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			Debug.Log($"[MapSceneLoader] 'M' pressed in scene {SceneManager.GetActiveScene().name}. Loading 'Map'");
			SceneManager.LoadScene("Map"); // Make sure your map scene is called this
		}
	}
}
