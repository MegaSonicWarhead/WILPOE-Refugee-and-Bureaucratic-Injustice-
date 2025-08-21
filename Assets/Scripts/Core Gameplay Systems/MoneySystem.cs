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
        Debug.Log($"[MoneySystem] Awake in scene: {SceneManager.GetActiveScene().name}, object: {gameObject.name}");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[MoneySystem] Instance set and DontDestroyOnLoad applied to: {gameObject.name}");
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"[MoneySystem] Duplicate detected. Destroying ONLY MoneySystem component on {gameObject.name} (existing: {Instance.gameObject.name})");
            Destroy(this); // ✅ only remove this component, not the entire GameObject
            return;
        }
    }

    private void OnEnable()
    {
        Debug.Log($"[MoneySystem] OnEnable called on {gameObject.name}");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Debug.Log($"[MoneySystem] OnDisable called on {gameObject.name}");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        Debug.Log($"[MoneySystem] Start in scene: {SceneManager.GetActiveScene().name}");
        UpdateMoneyUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Debug.LogError($"[MoneySystem] The *real* MoneySystem was destroyed in scene {SceneManager.GetActiveScene().name}. This should NOT happen!");
            Instance = null;
        }
        else
        {
            Debug.Log($"[MoneySystem] Duplicate MoneySystem component cleaned up in {SceneManager.GetActiveScene().name}");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[MoneySystem] Scene loaded: {scene.name}. Checking for MoneyText...");

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

    public void SetMoneyText(TextMeshProUGUI newText)
    {
        moneyText = newText;
        Debug.Log("[MoneySystem] Money text reference updated via SetMoneyText");
        UpdateMoneyUI();
    }
}
