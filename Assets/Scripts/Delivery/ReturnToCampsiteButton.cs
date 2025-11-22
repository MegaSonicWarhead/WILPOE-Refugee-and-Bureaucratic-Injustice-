using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCampsiteButton : MonoBehaviour
{
    [Header("Scene Settings")]
    public string Campsite = "Campsite";
    public string DeliveryGame = "DeliveryGame";

    public void ReturnToCampsite()
    {
        Debug.Log("Returning to campsite scene: " + Campsite);
        SceneManager.LoadScene(Campsite);
    }
    public void ToJob()
    {
        Debug.Log("Returning to campsite scene: " + DeliveryGame);
        SceneManager.LoadScene(DeliveryGame);
    }
}
