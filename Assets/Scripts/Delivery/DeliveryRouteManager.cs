using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryRouteManager : MonoBehaviour
{
    [Header("Waypoints in order")]
    public List<GameObject> waypointPrefabs;

    [Header("Waypoint Materials")]
    public Material greenMaterial; // Current target
    public Material yellowMaterial; // Next target

    [Header("UI")]
    public TextMeshProUGUI jobStatusText;

    [Header("Money System")]
    public int rewardAmount = 500;

    private Queue<GameObject> waypointQueue = new Queue<GameObject>();
    private GameObject currentWaypoint;
    private GameObject nextWaypoint;
    private bool routeCompleted = false;

    void Start()
    {
        if (waypointPrefabs.Count == 0)
        {
            Debug.LogError("No waypoints assigned!");
            return;
        }

        // Enqueue all waypoints
        foreach (GameObject wp in waypointPrefabs)
        {
            wp.SetActive(false);
            waypointQueue.Enqueue(wp);
        }

        // Activate the first two
        ActivateNextTwoWaypoints();
    }

    private void ActivateNextTwoWaypoints()
    {
        if (waypointQueue.Count > 0)
        {
            currentWaypoint = waypointQueue.Dequeue();
            currentWaypoint.SetActive(true);
            SetWaypointColor(currentWaypoint, greenMaterial);
        }

        if (waypointQueue.Count > 0)
        {
            nextWaypoint = waypointQueue.Dequeue();
            nextWaypoint.SetActive(true);
            SetWaypointColor(nextWaypoint, yellowMaterial);
        }

        UpdateUIText();
    }

    public void OnWaypointReached(GameObject waypoint)
    {
        Debug.Log("Reached waypoint: " + waypoint.name);
        if (waypoint == currentWaypoint)
        {
            Destroy(waypoint);
            PromoteNextWaypoint();
        }
    }

    private void PromoteNextWaypoint()
    {
        if (nextWaypoint != null)
        {
            currentWaypoint = nextWaypoint;
            SetWaypointColor(currentWaypoint, greenMaterial);
        }
        else
        {
            currentWaypoint = null;
        }

        // Spawn another yellow one if there’s more in the queue
        if (waypointQueue.Count > 0)
        {
            nextWaypoint = waypointQueue.Dequeue();
            nextWaypoint.SetActive(true);
            SetWaypointColor(nextWaypoint, yellowMaterial);
        }
        else
        {
            nextWaypoint = null;
        }

        // Check if finished
        if (currentWaypoint == null && nextWaypoint == null && !routeCompleted)
        {
            routeCompleted = true;
            if (jobStatusText != null)
                jobStatusText.text = "Delivery route complete! You earned R" + rewardAmount;

            if (MoneySystem.Instance != null)
                MoneySystem.Instance.AddMoney(rewardAmount);

            Debug.Log("All waypoints completed! Player rewarded R" + rewardAmount);
        }
        else
        {
            UpdateUIText();
        }
    }

    private void UpdateUIText()
    {
        if (jobStatusText == null) return;

        if (currentWaypoint != null)
            jobStatusText.text = "Deliver to " + currentWaypoint.name;
        else
            jobStatusText.text = "All deliveries complete!";
    }

    private void SetWaypointColor(GameObject waypoint, Material mat)
    {
        Renderer r = waypoint.GetComponent<Renderer>();
        if (r != null && mat != null)
            r.material = mat;
    }
}
