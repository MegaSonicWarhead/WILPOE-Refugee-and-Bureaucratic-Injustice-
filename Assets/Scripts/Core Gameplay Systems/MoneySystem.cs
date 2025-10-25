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
    private bool instanceSet = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            instanceSet = true;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[MoneySystem] Instance set for {gameObject.name}");
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"[MoneySystem] Duplicate detected. Destroying only this component on {gameObject.name}");
            Destroy(this);
            return;
        }
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

    private void OnDestroy()
    {
        if (instanceSet)
            Instance = null;
    }

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
        Debug.Log($"[MoneySystem] Money earned: +{amount}. Total: {currentMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0) return true; // ignore zero or negative
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            Debug.Log($"[MoneySystem] Money spent: -{amount}. Remaining: {currentMoney}");
            return true;
        }
        else
        {
            // If not enough, set to 0 and log
            currentMoney = 0;
            UpdateMoneyUI();
            Debug.LogWarning($"[MoneySystem] Not enough money! Money set to 0.");
            return false;
        }
    }

    public int GetMoney() => currentMoney;

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"R {currentMoney}";
    }

    public void SetMoneyText(TextMeshProUGUI newText)
    {
        moneyText = newText;
        UpdateMoneyUI();
    }
}
