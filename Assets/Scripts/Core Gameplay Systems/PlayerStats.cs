using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateUI();

        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnNewDay += OnNewDay;
        }
    }

    private void OnDestroy()
    {
        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnNewDay -= OnNewDay;
        }
    }

    void OnNewDay()
    {
        if (isDead) return;

        // Hunger and thirst decay each day
        ModifyHunger(-15f);
        ModifyThirst(-20f);

        // Health penalties if hunger or thirst are critically low
        if (hunger <= 10f)
        {
            ModifyHealth(-5f);
        }
        if (thirst <= 10f)
        {
            ModifyHealth(-10f);
        }

        // Player loses health & sanity if they didn't eat
        if (lastEatDay != GameTime.Instance.currentDay)
        {
            ModifyHealth(-10f);
            ModifySanity(-5f);
        }

        // Track sleep deprivation
        if (lastSleepDay != GameTime.Instance.currentDay)
        {
            daysWithoutSleep++;
        }
        else
        {
            daysWithoutSleep = 0; // reset if player slept today
        }

        if (daysWithoutSleep >= 4)
        {
            Debug.Log("Player died from sleep deprivation.");
            KillPlayer();
        }

        UpdateUI();
    }

    // Actions
    public void Sleep()
    {
        if (isDead) return;

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

    public void UsePublicBathroom()
    {
        // Implement later if needed
    }

    // Modifiers
    public void ModifyHealth(float amount)
    {
        if (isDead) return;

        health = Mathf.Clamp(health + amount, 0f, 100f);
        if (healthSlider != null) healthSlider.value = health;

        if (health <= 0f)
        {
            KillPlayer();
        }
    }

    public void ModifyThirst(float amount)
    {
        if (isDead) return;

        thirst = Mathf.Clamp(thirst + amount, 0f, 100f);
        if (thirstSlider != null) thirstSlider.value = thirst;
    }

    public void ModifyHunger(float amount)
    {
        if (isDead) return;

        hunger = Mathf.Clamp(hunger + amount, 0f, 100f);
        if (hungerSlider != null) hungerSlider.value = hunger;
    }

    public void ModifySanity(float amount)
    {
        if (isDead) return;

        sanity = Mathf.Clamp(sanity + amount, 0f, 100f);
        if (sanitySlider != null) sanitySlider.value = sanity;
    }

    // Death Handler
    void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player has died.");
        // TODO: Add your Game Over logic here, e.g.:
        // GameManager.Instance.TriggerGameOver();
        // Disable player controls, show UI, etc.
    }

    // Refresh all sliders
    void UpdateUI()
    {
        if (healthSlider != null) healthSlider.value = health;
        if (thirstSlider != null) thirstSlider.value = thirst;
        if (hungerSlider != null) hungerSlider.value = hunger;
        if (sanitySlider != null) sanitySlider.value = sanity;
    }

}
