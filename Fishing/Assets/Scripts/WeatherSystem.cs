using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem
{
    private string _currentWeather;
    private string _currentSeason;

    public void Initialize()
    {
        _currentWeather = "Clear";
        _currentSeason = "Summer";
    }

    public string GetWeather()
    {
        return _currentWeather;
    }

    public string GetSeason()
    {
        return _currentSeason;
    }
}
