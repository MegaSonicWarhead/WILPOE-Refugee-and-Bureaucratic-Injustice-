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
        // Player loses health & sanity if they didn't eat
        if (lastEatDay != GameTime.Instance.currentDay)
        {
            ModifyHealth(-10);
            ModifySanity(-5);
        }

        // Track sleep deprivation
        if (lastSleepDay != GameTime.Instance.currentDay)
        {
            daysWithoutSleep++;
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
        lastSleepDay = GameTime.Instance.currentDay;
        daysWithoutSleep = 0;
        ModifySanity(+20);
    }

    public void Eat()
    {
        hunger = 100f;
        lastEatDay = GameTime.Instance.currentDay;
        UpdateUI();
    }

    public void DrinkPollutedWater()
    {
        ModifyThirst(+100);
        ModifyHealth(-10);
    }

    public void WashInPollutedWater()
    {
        ModifySanity(+10);
        ModifyHealth(-4);
    }

    public void UsePublicBathroom()
    {
        //int bathroomCost = 20;

        //if (GameCurrency.Instance != null && GameCurrency.Instance.CanAfford(bathroomCost))
        //{
        //    GameCurrency.Instance.Spend(bathroomCost);
        //    ModifySanity(+60);
        //    ModifyHealth(+10);
        //}
        //else
        //{
        //    Debug.Log("Not enough money to use the public bathroom.");
        //}
    }

    // Modifiers
    public void ModifyHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, 100);
        if (healthSlider != null) healthSlider.value = health;

        if (health <= 0)
        {
            KillPlayer();
        }
    }

    public void ModifyThirst(float amount)
    {
        thirst = Mathf.Clamp(thirst + amount, 0, 100);
        if (thirstSlider != null) thirstSlider.value = thirst;
    }

    public void ModifyHunger(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, 100);
        if (hungerSlider != null) hungerSlider.value = hunger;
    }

    public void ModifySanity(float amount)
    {
        sanity = Mathf.Clamp(sanity + amount, 0, 100);
        if (sanitySlider != null) sanitySlider.value = sanity;
    }

    // Death Handler
    void KillPlayer()
    {
        Debug.Log("Player has died.");
        // Add Game Over logic here
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
