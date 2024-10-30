using System;
using UnityEngine;

public class FishingSystem
{
    private float _castDistance;
    private float _depth;
    private string _bait;
    private float _playerSkill;
    private WeatherSystem _weather;
    private DatabaseManager _databaseManager;
    private Fish _currentFish;

    public FishingSystem(DatabaseManager databaseManager, WeatherSystem weather)
    {
        _databaseManager = databaseManager;
        _weather = weather;
    }

    public Fish LoadFishData(string fishName)
    {
        _currentFish = _databaseManager.LoadFishData(fishName);
        return _currentFish;
    }

    public Fish GetCurrentFish()
    {
        return _currentFish;
    }
    
    public void SetCastDistance(float distance)
    {
        _castDistance = distance;
    }

    public void SetDepth(float depth)
    {
        _depth = depth;
    }

    public void SetBait(string bait)
    {
        _bait = bait;
    }

    public void StartFishing()
    {
        float catchProbability = CalculateCatchProbability();
        if (IsFishCaught(catchProbability))
        {
            Fish caughtFish = DetermineFishCaught(catchProbability);
            Debug.Log($"Fish caught: {caughtFish}");
            // CatchCanvas UI update
        }
        else
        {
            Debug.Log("Fish is not caught. Try again!");
        }
    }

    /// <summary>
    /// catchProbability=baseProbability+distanceFactor+depthFactor+baitFactor+weatherFactor+skillFactor+randomFactor
    /// </summary>
    /// <returns></returns>
    private float CalculateCatchProbability()
    {
        float baseProbability = 0.2f;
        float distanceFactor = CalculateDistanceFactor();
        float depthFactor = CalculateDepthFactor();
        float baitFactor = CalculateBaitFactor();
        float weatherFactor = CalculateWeatherFactor();
        float skillFactor = _playerSkill * 0.05f;                        // increase probability at 5% for level
        
        float randomFactor = UnityEngine.Random.Range(-0.05f, 0.05f);   // sudden element -0.05 to 0.05
        
        float catchProbability = baseProbability + distanceFactor + depthFactor + baitFactor + weatherFactor + skillFactor + randomFactor;
        
        return Mathf.Clamp(catchProbability, 0f, 1.0f);         // limit probability from 0 to 1.0
    }

    private float CalculateDistanceFactor()
    {
        float preferredDistance = _currentFish.PreferredCastDistance;
        float distanceAccuracy = Mathf.Abs(_castDistance - preferredDistance);
        
        float factor = Mathf.Clamp(1 - distanceAccuracy / 10f, 0f, 1f);     // if accuracy < 1 then factor is max, if accuracy > 10 then factor is min
        return factor * 0.2f;                               // this factor weight in overall catch probability
    }

    private float CalculateDepthFactor()
    {
        float preferredDepth = _currentFish.PreferredDepth;
        float depthAccuracy = Mathf.Abs(_depth - preferredDepth);
        
        // analogy: the closer depth - the higher the factor
        float factor = Mathf.Clamp(1 - depthAccuracy / 10f, 0f, 1f);     // if accuracy < 1 then factor is max, if accuracy > 10 then factor is min
        return factor * 0.2f;                               // this factor weight in overall catch probability
    }

    private float CalculateBaitFactor()
    {
        // if chosen bait is the same as preferred bait then factor is max
        return _bait == _currentFish.PreferredBait ? 0.2f : 0f;
    }

    private float CalculateWeatherFactor()
    {
        // check if the current weather is the same as preferred
        string currentWeather = _weather.GetWeather();
        return _currentFish.ActiveWeather == currentWeather ? 0.15f : 0f;
    }

    private bool IsFishCaught(float probability)
    {
        return UnityEngine.Random.value < probability;
    }

    private Fish DetermineFishCaught(float catchProbability)
    {
        return _currentFish; 
        //return new Fish("Обычная рыба", 1)
    }


    public string GetCurrentBait()
    {
        throw new NotImplementedException();
    }
}
