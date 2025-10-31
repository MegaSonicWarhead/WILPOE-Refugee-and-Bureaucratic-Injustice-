using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class JobOfferManager : MonoBehaviour
{
    [Header("Job Offer UI")]
    public GameObject jobOfferPanel;
    public TextMeshProUGUI jobOfferText;
    public Button acceptButton;
    public Button declineButton;

    [Header("Settings")]
    [Range(0f, 1f)] public float chancePerDay = 0.25f; // 25% chance each day
    public string deliverySceneName = "DeliveryGame";
    public float panelDelaySeconds = 2f; // optional delay before showing

    private bool offerShownToday = false;

    void Start()
    {
        if (jobOfferPanel != null)
            jobOfferPanel.SetActive(false);

        // Optional: if GameTime or another system advances the day,
        // call TryOfferJob() from there.  For quick test:
        StartCoroutine(RandomOfferRoutine());
    }

    // Example coroutine simulating checks every few seconds.
    private IEnumerator RandomOfferRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // every 10 seconds simulate a "new day"
            TryOfferJob();
        }
    }

    public void TryOfferJob()
    {
        if (offerShownToday) return; // only once per cycle
        offerShownToday = true;

        float roll = Random.value;
        Debug.Log($"[JobOffer] Rolled {roll}, Chance = {chancePerDay}");

        if (roll <= chancePerDay)
        {
            StartCoroutine(ShowJobPanelAfterDelay());
        }
    }

    private IEnumerator ShowJobPanelAfterDelay()
    {
        yield return new WaitForSeconds(panelDelaySeconds);

        if (jobOfferPanel != null)
        {
            jobOfferPanel.SetActive(true);
            jobOfferText.text = "A delivery job is available. Do you want to accept?";
        }
    }

    // --- Button Callbacks ---
    public void AcceptJob()
    {
        Debug.Log("[JobOffer] Player accepted job. Loading DeliveryGame scene...");
        SceneManager.LoadScene(deliverySceneName);
    }

    public void DeclineJob()
    {
        Debug.Log("[JobOffer] Player declined job.");
        jobOfferPanel.SetActive(false);
    }
}
