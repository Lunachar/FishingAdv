using System;
using UnityEngine;

public class FishingSystem
{
    private float _castDistance;
    private float _depth;
    private string _bait;
    private float _playerSkill;
    private WeatherSystem _weather;

    public FishingSystem(float playerSkill, WeatherSystem weather)
    {
        _playerSkill = playerSkill;
        _weather = weather;
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
        throw new NotImplementedException();
    }

    private float CalculateDepthFactor()
    {
        throw new NotImplementedException();
    }

    private float CalculateBaitFactor()
    {
        throw new NotImplementedException();
    }

    private float CalculateWeatherFactor()
    {
        throw new NotImplementedException();
    }

    private bool IsFishCaught(float probability)
    {
        return UnityEngine.Random.value < probability;
    }

    private Fish DetermineFishCaught(float catchProbability)
    {
        throw new NotImplementedException();
        //return new Fish("Обычная рыба", 1)
    }


    public string GetCurrentBait()
    {
        throw new NotImplementedException();
    }
}
