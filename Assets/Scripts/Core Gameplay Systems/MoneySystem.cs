// Scripts/MoneySystem.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI moneyText;

    private int currentMoney = 100;

    private void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
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
                UpdateMoneyUI();
            }
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
        Debug.Log($"Money earned: +{amount}. Total: {currentMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            Debug.Log($"Money spent: -{amount}. Remaining: {currentMoney}");
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough money!");
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
        }
    }

    // Allow UI in each scene to register itself
    public void SetMoneyText(TextMeshProUGUI newText)
    {
        moneyText = newText;
        UpdateMoneyUI();
    }
}
