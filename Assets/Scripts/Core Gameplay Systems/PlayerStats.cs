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

	private void Awake()
	{
		Debug.Log($"[PlayerStats] Awake in scene: {SceneManager.GetActiveScene().name}, object: {gameObject.name}");
		if (Instance != null && Instance != this)
		{
			Debug.LogWarning($"[PlayerStats] Duplicate detected. Destroying GameObject: {gameObject.name} (existing instance on: {Instance.gameObject.name})");
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
		Debug.Log($"[PlayerStats] Instance set and DontDestroyOnLoad applied to: {gameObject.name}");
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Start()
	{
		Debug.Log("[PlayerStats] Start - Assigning sliders and updating UI");
		AssignSliders();
		UpdateUI();

		if (GameTime.Instance != null)
		{
			GameTime.Instance.OnNewDay += OnNewDay;
		}
		else
		{
			Debug.LogWarning("[PlayerStats] GameTime.Instance is null on Start");
		}
	}

	private void OnDestroy()
	{
		if (GameTime.Instance != null)
		{
			GameTime.Instance.OnNewDay -= OnNewDay;
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log($"[PlayerStats] OnSceneLoaded: {scene.name} - reassigning sliders and updating UI");
		AssignSliders();
		UpdateUI();
	}

	private void AssignSliders()
	{
		if (healthSlider == null)
			healthSlider = GameObject.FindWithTag("HealthSlider")?.GetComponent<Slider>();
		Debug.Log(healthSlider != null ? "[PlayerStats] Found HealthSlider by tag" : "[PlayerStats] HealthSlider NOT found (tag: HealthSlider)");

		if (thirstSlider == null)
			thirstSlider = GameObject.FindWithTag("ThirstSlider")?.GetComponent<Slider>();
		Debug.Log(thirstSlider != null ? "[PlayerStats] Found ThirstSlider by tag" : "[PlayerStats] ThirstSlider NOT found (tag: ThirstSlider)");

		if (hungerSlider == null)
			hungerSlider = GameObject.FindWithTag("HungerSlider")?.GetComponent<Slider>();
		Debug.Log(hungerSlider != null ? "[PlayerStats] Found HungerSlider by tag" : "[PlayerStats] HungerSlider NOT found (tag: HungerSlider)");

		if (sanitySlider == null)
			sanitySlider = GameObject.FindWithTag("SanitySlider")?.GetComponent<Slider>();
		Debug.Log(sanitySlider != null ? "[PlayerStats] Found SanitySlider by tag" : "[PlayerStats] SanitySlider NOT found (tag: SanitySlider)");
	}

	void OnNewDay()
	{
		if (isDead) return;

		ModifyHunger(-15f);
		ModifyThirst(-20f);

		if (hunger <= 10f)
			ModifyHealth(-5f);
		if (thirst <= 10f)
			ModifyHealth(-10f);

		if (lastEatDay != GameTime.Instance.currentDay)
		{
			ModifyHealth(-10f);
			ModifySanity(-5f);
		}

		if (lastSleepDay != GameTime.Instance.currentDay)
		{
			daysWithoutSleep++;
		}
		else
		{
			daysWithoutSleep = 0;
		}

		if (daysWithoutSleep >= 4)
		{
			Debug.Log("Player died from sleep deprivation.");
			KillPlayer();
		}

		UpdateUI();
	}

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

	void KillPlayer()
	{
		if (isDead) return;

		isDead = true;
		Debug.Log("Player has died.");

		// Optionally delay scene change to show death feedback (e.g. animation)
		StartCoroutine(HandlePlayerDeath());
	}

	IEnumerator HandlePlayerDeath()
	{
		yield return new WaitForSeconds(1.5f); // Optional: wait before switching scene
		SceneManager.LoadScene("PlayerDead");
	}

	void UpdateUI()
	{
		Debug.Log($"[PlayerStats] UpdateUI -> H:{health} T:{thirst} Hu:{hunger} S:{sanity} | sliders present? H:{healthSlider!=null} T:{thirstSlider!=null} Hu:{hungerSlider!=null} S:{sanitySlider!=null}");
		if (healthSlider != null) healthSlider.value = health;
		if (thirstSlider != null) thirstSlider.value = thirst;
		if (hungerSlider != null) hungerSlider.value = hunger;
		if (sanitySlider != null) sanitySlider.value = sanity;
	}
}
