// Scripts/MoneySystem.cs
using UnityEngine;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI moneyText;

    private int currentMoney = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Stay across scenes
    }

    private void Start()
    {
        UpdateMoneyUI();
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

    public void SetMoneyText(TextMeshProUGUI newText)
    {
        moneyText = newText;
        UpdateMoneyUI();
    }
}
