using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    public string CurrentWeather { get; private set;}
    public string CurrentSeason  { get; private set;}

    public void Initialize(string weather, string season)
    {
        CurrentWeather = weather;
        CurrentSeason = season;
    }

    public void UpdateWeather(string weather)
    {
        CurrentWeather = weather;
    }

    public void UpdateSeason(string season)
    {
        CurrentSeason = season;
    }
}
