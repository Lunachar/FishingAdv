using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem
{
    private string _currentWeather;
    private string _currentSeason;

    private readonly List<string> _seasons = new()
    {
        "Spring",
        "Summer",
        "Autumn",
        "Winter"
    };
    
    private readonly List<string> _weatherTypes = new()
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

    public void SetWeather(string weather)
    {
        _currentWeather = weather;
    }

    public void SetSeason(string season)
    {
        _currentSeason = season;
    }
}
