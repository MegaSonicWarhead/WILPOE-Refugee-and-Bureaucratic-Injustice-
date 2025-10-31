using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCampsiteButton : MonoBehaviour
{
    [Header("Scene Settings")]
    public string Campsite = "Campsite"; 

    public void ReturnToCampsite()
    {
        Debug.Log("Returning to campsite scene: " + Campsite);
        SceneManager.LoadScene(Campsite);
    }
}
