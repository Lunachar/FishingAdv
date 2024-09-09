using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void GameEvent();

    public static event GameEvent OnFishingStart;
    public static event GameEvent OnWeatherChange;

    public static void StartFishing()
    {
        OnFishingStart?.Invoke();
    }

    public static void ChangeWeather()
    {
        OnWeatherChange?.Invoke();
    }
}
