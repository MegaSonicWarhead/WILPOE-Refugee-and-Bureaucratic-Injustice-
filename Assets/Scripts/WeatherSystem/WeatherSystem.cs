using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherSystem : MonoBehaviour
{
    [System.Serializable]
    public class WeatherType
    {
        public string name;
        public Sprite icon;
        public GameObject particleEffect;
        public int weight; // Higher = more common
    }

    public Image weatherIcon; // UI Image (WeatherIcon)
    public TextMeshProUGUI weatherText; // TextMeshPro below it
    public List<WeatherType> weatherTypes; // Fill in Inspector

    public float changeInterval = 30f;

    private WeatherType currentWeather;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomWeather(); // Start with random weather
        StartCoroutine(ChangeWeatherRoutine());
    }

    IEnumerator ChangeWeatherRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);
            SetRandomWeather();
        }
    }

    void SetRandomWeather()
    {
        WeatherType newWeather = GetWeightedRandomWeather();

        if (currentWeather != null && currentWeather.particleEffect != null)
            currentWeather.particleEffect.SetActive(false);

        currentWeather = newWeather;

        weatherIcon.sprite = newWeather.icon;
        weatherText.text = newWeather.name;

        if (newWeather.particleEffect != null)
            newWeather.particleEffect.SetActive(true);
    }

    WeatherType GetWeightedRandomWeather()
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

        return weatherTypes[0]; // Fallback
    }

    
}
