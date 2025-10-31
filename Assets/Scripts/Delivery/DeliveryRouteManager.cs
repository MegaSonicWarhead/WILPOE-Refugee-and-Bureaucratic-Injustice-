using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeliveryRouteManager : MonoBehaviour
{
    [Header("Waypoints (Assign in Order)")]
    public List<GameObject> waypointPrefabs;
    private Queue<GameObject> waypointQueue = new Queue<GameObject>();

    [Header("UI")]
    public TextMeshProUGUI jobStatusText;
    public GameObject jobCompletePanel; // Assign in Inspector
    public string campsiteSceneName = "CampsiteScene"; // Change to your actual scene name

    [Header("Money System")]
    public int rewardAmount = 500;

    private GameObject currentWaypoint;
    private GameObject nextWaypoint;
    private bool routeCompleted = false;

    void Start()
    {
        if (waypointPrefabs.Count == 0)
        {
            Debug.LogError("No waypoints assigned to DeliveryRouteManager!");
            return;
        }

        foreach (GameObject wp in waypointPrefabs)
        {
            waypointQueue.Enqueue(wp);
            wp.SetActive(false);
        }

        // Hide job complete panel
        if (jobCompletePanel != null)
            jobCompletePanel.SetActive(false);

        // Activate the first two waypoints
        ActivateNextTwoWaypoints();
    }

    private void ActivateNextTwoWaypoints()
    {
        if (waypointQueue.Count > 0)
        {
            currentWaypoint = waypointQueue.Dequeue();
            currentWaypoint.SetActive(true);
            SetWaypointColor(currentWaypoint, Color.green);
        }

        if (waypointQueue.Count > 0)
        {
            nextWaypoint = waypointQueue.Dequeue();
            nextWaypoint.SetActive(true);
            SetWaypointColor(nextWaypoint, Color.yellow);
        }

        if (jobStatusText != null && currentWaypoint != null)
            jobStatusText.text = "Drive to " + currentWaypoint.name;
    }

    public void OnWaypointReached(GameObject waypoint)
    {
        Debug.Log("Reached waypoint: " + waypoint.name);

        if (waypoint == currentWaypoint)
        {
            Destroy(waypoint); // Remove reached waypoint
            currentWaypoint = nextWaypoint;

            if (currentWaypoint != null)
                SetWaypointColor(currentWaypoint, Color.green);

            if (waypointQueue.Count > 0)
            {
                nextWaypoint = waypointQueue.Dequeue();
                nextWaypoint.SetActive(true);
                SetWaypointColor(nextWaypoint, Color.yellow);
            }
            else
            {
                nextWaypoint = null;
            }

            // If no waypoints left and current is done
            if (currentWaypoint == null && nextWaypoint == null)
            {
                CompleteJob();
            }
        }
    }

    private void SetWaypointColor(GameObject waypoint, Color color)
    {
        Renderer rend = waypoint.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = color;
        }
    }

    private void CompleteJob()
    {
        if (routeCompleted) return;

        routeCompleted = true;
        Debug.Log("Job complete! Player earned R" + rewardAmount);

        if (jobStatusText != null)
            jobStatusText.text = "Job complete! You earned R" + rewardAmount;

        if (MoneySystem.Instance != null)
            MoneySystem.Instance.AddMoney(rewardAmount);

        if (jobCompletePanel != null)
            jobCompletePanel.SetActive(true);
    }

    // Called by UI Button
    public void ReturnToCampsite()
    {
        Debug.Log("Returning to Campsite scene...");
        SceneManager.LoadScene(campsiteSceneName);
    }
}
