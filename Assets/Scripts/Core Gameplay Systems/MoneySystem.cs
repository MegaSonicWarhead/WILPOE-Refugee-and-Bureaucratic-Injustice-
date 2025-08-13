// Scripts/MoneySystem.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneySystem : MonoBehaviour
{
	public static MoneySystem Instance;

	[Header("UI Reference")]
	public TextMeshProUGUI moneyText;

	private int currentMoney = 150;

	private void Awake()
	{
		// Singleton pattern to persist across scenes
		if (Instance != null && Instance != this)
		{
			Debug.LogWarning($"[MoneySystem] Duplicate detected. Destroying: {gameObject.name} in scene {SceneManager.GetActiveScene().name}");
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject); // Persist across scenes
		Debug.Log($"[MoneySystem] Instance set and DontDestroyOnLoad for: {gameObject.name}");
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
		UpdateMoneyUI();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// Attempt to find a new TextMeshProUGUI tagged with "MoneyText"
		if (moneyText == null)
		{
			GameObject found = GameObject.FindWithTag("MoneyText");
			if (found != null)
			{
				moneyText = found.GetComponent<TextMeshProUGUI>();
				Debug.Log($"[MoneySystem] Found MoneyText in scene: {scene.name}");
				UpdateMoneyUI();
			}
			else
			{
				Debug.LogWarning($"[MoneySystem] MoneyText tag not found in scene: {scene.name}");
			}
		}
	}

	public void AddMoney(int amount)
	{
		currentMoney += amount;
		UpdateMoneyUI();
		Debug.Log($"[MoneySystem] Money earned: +{amount}. Total: {currentMoney}");
	}

	public bool SpendMoney(int amount)
	{
		if (currentMoney >= amount)
		{
			currentMoney -= amount;
			UpdateMoneyUI();
			Debug.Log($"[MoneySystem] Money spent: -{amount}. Remaining: {currentMoney}");
			return true;
		}
		else
		{
			Debug.LogWarning("[MoneySystem] Not enough money!");
			return false;
		}
	}

	public int GetMoney()
	{
		return currentMoney;
	}

	public void UpdateMoneyUI()
	{
		if (moneyText != null)
		{
			moneyText.text = $"R {currentMoney}";
			Debug.Log($"[MoneySystem] UpdateMoneyUI -> {moneyText.text}");
		}
		else
		{
			Debug.Log("[MoneySystem] UpdateMoneyUI skipped (moneyText is null)");
		}
	}

	// Allow UI in each scene to register itself
	public void SetMoneyText(TextMeshProUGUI newText)
	{
		moneyText = newText;
		Debug.Log("[MoneySystem] Money text reference updated via SetMoneyText");
		UpdateMoneyUI();
	}
}
