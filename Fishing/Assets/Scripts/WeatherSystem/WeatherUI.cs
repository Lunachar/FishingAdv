using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WeatherUI: MonoBehaviour
{
    public TMP_Text WeatherText;
    public TMP_Text SeasonText;

    private WeatherSystem _weatherSystem;

    private void Awake()
    {
        
    }
    public void Initialize()
    {
        _weatherSystem = GlobalManager.Instance.GetWeatherSystem();
        UpdateWeatherUI();
        Debug.Log($"Initialized {GetType().Name}");
    }

    public void UpdateWeatherUI()
    {
        if (_weatherSystem != null)
        {
            WeatherText.text = _weatherSystem.GetWeather();
            SeasonText.text = _weatherSystem.GetSeason();
        }
        else
        {
            Debug.Log("WeatherSystem did not initialize");
        }
    }
}
