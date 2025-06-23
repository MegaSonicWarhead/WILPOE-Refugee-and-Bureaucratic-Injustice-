using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WeatherSystem : MonoBehaviour
{
    public static WeatherSystem Instance;

    [System.Serializable]
    public class WeatherType
    {
        public string name;
        public Sprite icon;
        public GameObject particleEffect;
        public int weight;
    }

    [Header("UI Elements")]
    public Image weatherIcon;
    public TextMeshProUGUI weatherText;

    [Header("Weather Options")]
    public List<WeatherType> weatherTypes;

    private WeatherType currentWeather;

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
        AssignUI();
        SetRandomWeather(); // First weather on initial start

        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnNewDay += SetRandomWeather;
        }
    }

    private void OnDestroy()
    {
        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnNewDay -= SetRandomWeather;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUI();
        ApplyWeatherEffects(); // Reapply particle and icon/text when scene changes
    }

    private void AssignUI()
    {
        if (weatherIcon == null)
            weatherIcon = GameObject.FindWithTag("WeatherIcon")?.GetComponent<Image>();
        if (weatherText == null)
            weatherText = GameObject.FindWithTag("WeatherText")?.GetComponent<TextMeshProUGUI>();
    }

    private void SetRandomWeather()
    {
        if (currentWeather != null && currentWeather.particleEffect != null)
            currentWeather.particleEffect.SetActive(false);

        currentWeather = GetWeightedRandomWeather();
        ApplyWeatherEffects();
    }

    private void ApplyWeatherEffects()
    {
        if (currentWeather == null) return;

        if (weatherIcon != null) weatherIcon.sprite = currentWeather.icon;
        if (weatherText != null) weatherText.text = currentWeather.name;

        foreach (var weather in weatherTypes)
        {
            if (weather.particleEffect != null)
                weather.particleEffect.SetActive(weather == currentWeather);
        }
    }

    private WeatherType GetWeightedRandomWeather()
    {
        int totalWeight = 0;
        foreach (var weather in weatherTypes)
            totalWeight += weather.weight;

        int rand = Random.Range(0, totalWeight);
        int runningWeight = 0;

        foreach (var weather in weatherTypes)
        {
            runningWeight += weather.weight;
            if (rand < runningWeight)
                return weather;
        }

        return weatherTypes[0];
    }
}
