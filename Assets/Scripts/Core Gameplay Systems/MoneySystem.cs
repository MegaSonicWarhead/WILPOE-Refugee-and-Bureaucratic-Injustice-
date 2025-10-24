// Scripts/MoneySystem.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI moneyText;

    [Header("Starting Money")]
    [SerializeField] private int startingMoney = 150;

    private int currentMoney;
    private bool instanceSet = false;

    public static event Action<int> OnMoneyChanged; // other scripts can listen to updates

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            instanceSet = true;
            DontDestroyOnLoad(gameObject);
            currentMoney = startingMoney;
            Debug.Log($"[MoneySystem] Instance set for {gameObject.name}");
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0) return true;
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            return true;
        }

        Debug.LogWarning("[MoneySystem] Not enough money!");
        return false;
    }

    public int GetMoney() => currentMoney;

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"R {currentMoney}";
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void SetMoneyText(TextMeshProUGUI newText)
    {
        moneyText = newText;
        UpdateMoneyUI();
    }
}
