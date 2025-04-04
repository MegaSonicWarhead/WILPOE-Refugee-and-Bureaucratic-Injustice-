using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneIconManger : MonoBehaviour
{
    public void LoadByIndex(int SceneIndex)
    {


        SceneManager.LoadScene(SceneIndex);
        Time.timeScale = 1;
    }
}
