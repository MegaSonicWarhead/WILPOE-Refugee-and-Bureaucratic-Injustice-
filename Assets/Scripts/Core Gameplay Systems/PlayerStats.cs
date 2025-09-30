using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Stat Sliders")]
    public Slider healthSlider;
    public Slider thirstSlider;
    public Slider hungerSlider;
    public Slider sanitySlider;

    [Header("Current Values")]
    public float health = 100f;
    public float thirst = 100f;
    public float hunger = 100f;
    public float sanity = 100f;

    private int daysWithoutSleep = 0;
    private int lastSleepDay = -1;
    private int lastEatDay = -1;
    private bool isDead = false;

    // Jail state
    private bool isInJail = false;
    private int jailDaysCounter = 0;
    private bool jailStatsRestoredToday = false; // Prevent double stats restore

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[PLAYERSTATS] Awake called, instance set.");
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Start()
    {
        AssignSliders();
        UpdateUI();

        if (GameTime.Instance != null)
            GameTime.Instance.OnNewDay += OnNewDay;

        Debug.Log("[PLAYERSTATS] Start completed.");
    }

    private void OnDestroy()
    {
        if (GameTime.Instance != null)
            GameTime.Instance.OnNewDay -= OnNewDay;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignSliders();
        UpdateUI();
        Debug.Log($"[PLAYERSTATS] Scene loaded: {scene.name}");
    }

    private void AssignSliders()
    {
        if (healthSlider == null) healthSlider = GameObject.FindWithTag("HealthSlider")?.GetComponent<Slider>();
        if (thirstSlider == null) thirstSlider = GameObject.FindWithTag("ThirstSlider")?.GetComponent<Slider>();
        if (hungerSlider == null) hungerSlider = GameObject.FindWithTag("HungerSlider")?.GetComponent<Slider>();
        if (sanitySlider == null) sanitySlider = GameObject.FindWithTag("SanitySlider")?.GetComponent<Slider>();
    }

    void OnNewDay()
    {
        if (isDead) return;

        // Handle jail first
        if (isInJail)
        {
            if (!jailStatsRestoredToday)
            {
                RestoreStatsForJail();
                jailDaysCounter--;
                if (jailDaysCounter <= 0)
                {
                    isInJail = false;
                    Debug.Log("[JAIL] Player released from jail.");
                }
                jailStatsRestoredToday = true;
                UpdateUI();
            }
            return;
        }

        jailStatsRestoredToday = false; // Reset for normal days

        // Daily penalties
        ModifyHunger(-15f);
        ModifyThirst(-20f);

        if (hunger <= 10f) ModifyHealth(-5f);
        if (thirst <= 10f) ModifyHealth(-10f);

        if (lastEatDay != GameTime.Instance.currentDay)
        {
            ModifyHealth(-10f);
            ModifySanity(-5f);
        }

        if (lastSleepDay != GameTime.Instance.currentDay)
            daysWithoutSleep++;
        else
            daysWithoutSleep = 0;

      //  if (daysWithoutSleep >= 4) KillPlayer();

        UpdateUI();
    }

    private void RestoreStatsForJail()
    {
        health = 100f;
        hunger = 100f;
        thirst = 100f;
        sanity = 10f;
        daysWithoutSleep = 0;
        lastSleepDay = GameTime.Instance.currentDay;

        Debug.Log("[JAIL] Stats restored for jail day.");
    }

    public void Sleep()
    {
        if (isDead) return;
        if (isInJail) return;

        lastSleepDay = GameTime.Instance.currentDay;
        daysWithoutSleep = 0;
        ModifySanity(+20f);
        UpdateUI();
    }

    public void Eat()
    {
        if (isDead) return;
        hunger = 100f;
        lastEatDay = GameTime.Instance.currentDay;
        UpdateUI();
    }

    public void DrinkPollutedWater()
    {
        if (isDead) return;
        thirst = 100f;
        ModifyHealth(-10f);
        UpdateUI();
    }

    public void WashInPollutedWater()
    {
        if (isDead) return;
        ModifySanity(+10f);
        ModifyHealth(-4f);
        UpdateUI();
    }

    public void ModifyHealth(float amount)
    {
        if (isDead || isInJail) return;
        health = Mathf.Clamp(health + amount, 0f, 100f);
        if (healthSlider != null) healthSlider.value = health;

       // if (health <= 0f) KillPlayer();
    }

    public void ModifyThirst(float amount)
    {
        if (isDead || isInJail) return;
        thirst = Mathf.Clamp(thirst + amount, 0f, 100f);
        if (thirstSlider != null) thirstSlider.value = thirst;
    }

    public void ModifyHunger(float amount)
    {
        if (isDead || isInJail) return;
        hunger = Mathf.Clamp(hunger + amount, 0f, 100f);
        if (hungerSlider != null) hungerSlider.value = hunger;
    }

    public void ModifySanity(float amount)
    {
        if (isDead || isInJail) return;
        sanity = Mathf.Clamp(sanity + amount, 0f, 100f);
        if (sanitySlider != null) sanitySlider.value = sanity;
    }

   /* void KillPlayer()
    {
        if (isDead) return;
        isDead = true;
        StartCoroutine(HandlePlayerDeath());
    }
   */
    IEnumerator HandlePlayerDeath()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("PlayerDead");
    }

    void UpdateUI()
    {
        if (healthSlider != null) healthSlider.value = health;
        if (thirstSlider != null) thirstSlider.value = thirst;
        if (hungerSlider != null) hungerSlider.value = hunger;
        if (sanitySlider != null) sanitySlider.value = sanity;
    }

    // Jail functions
    public void GoToJail(int days)
    {
        isInJail = true;
        jailDaysCounter = days; // jail duration in days
        sanity = 10f;
        health = 100f;
        hunger = 100f;
        thirst = 100f;
        daysWithoutSleep = 0;
        lastSleepDay = GameTime.Instance.currentDay;

        Debug.Log($"[JAIL] Player sent to jail for {days} days. Stats restored.");
        UpdateUI();
    }

    public bool IsInJail() => isInJail;
}
