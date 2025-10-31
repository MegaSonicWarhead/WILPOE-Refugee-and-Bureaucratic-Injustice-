using UnityEngine;
using TMPro;

public class DeliveryJob : MonoBehaviour
{
    public TextMeshProUGUI jobStatusText;
    public int rewardAmount = 200;

    private bool hasPackage = false;

    void Start()
    {
        if (jobStatusText != null)
        {
            jobStatusText.text = "Drive to the Pickup Point.";
            Debug.Log("DeliveryJob: jobStatusText is assigned.");
        }
        else
        {
            Debug.LogError("DeliveryJob: jobStatusText is NOT assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER with: " + other.name + " | Tag: " + other.tag);

        if (other.CompareTag("Pickup") && !hasPackage)
        {
            hasPackage = true;
            Debug.Log("Package picked up.");

            if (jobStatusText != null)
                jobStatusText.text = "Package collected! Deliver to the destination.";
            else
                Debug.LogError("jobStatusText is null, cannot update text!");
        }
        else if (other.CompareTag("Delivery") && hasPackage)
        {
            hasPackage = false;
            Debug.Log("Package delivered. Reward earned: R" + rewardAmount);

            if (jobStatusText != null)
                jobStatusText.text = "Delivery Complete! You earned R" + rewardAmount;
            else
                Debug.LogError("jobStatusText is null, cannot update text!");
        }
    }
}
