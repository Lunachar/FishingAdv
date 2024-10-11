using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WeatherUI: MonoBehaviour
{
    public TMP_Text WeatherText;
    public TMP_Text SeasonText;

    private WeatherSystem _weatherSystem;

    public void Initialize(WeatherSystem weatherSystem)
    {
        _weatherSystem = weatherSystem;
        UpdateWeatherUI();
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
