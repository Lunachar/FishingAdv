using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem
{
    private string _currentWeather;
    private string _currentSeason;

    private List<string> _seasons = new List<string> { "Sping", "Summer", "Autumn", "Winter" };
    
    private List<string> _weatherTypes = new List<string>
    { 
        "Clear", 
        "Cloudy", 
        "Storm", 
        "Drizzle", 
        "Rain", 
        "Windy", 
        "Fog", 
        "Snow"
    };

    public void Initialize()
    {
        _currentWeather = GetRandomWeather();
        _currentSeason = GetRandomSeason();
    }

    private string GetRandomSeason()
    {
        int randomIndex = Random.Range(0, _seasons.Count);
        return _seasons[randomIndex];
    }

    private string GetRandomWeather()
    {
        int randomIndex = Random.Range(0, _weatherTypes.Count);
        return _weatherTypes[randomIndex];
    }

    public string GetWeather()
    {
        return _currentWeather;
    }

    public string GetSeason()
    {
        return _currentSeason;
    }

    public void UpdateWeather()
    {
        _currentWeather = GetRandomWeather();
        _currentSeason = GetRandomSeason();
    }
}
