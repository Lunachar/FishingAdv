using System;
using System.Collections.Generic;
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
    private InventorySystem _inventorySystem;
    private UIManager _uiManager;

    public FishingSystem(DatabaseManager databaseManager, WeatherSystem weather)
    {
        _databaseManager = databaseManager;
        _weather = weather;
    }

    public void SetInventorySystem(InventorySystem inventorySystem)
    {
        _inventorySystem = inventorySystem;
    }
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
        Debug.Log($"FS. .SetUIManager: {_uiManager}");
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

    private Fish DeterminePotentialFish()
    {
        // get a list of fish that matches the parameters
        List<Fish> potentialFishList = _databaseManager.GetFishMatchingParameters(_castDistance, _depth, _bait);
        
        if (potentialFishList.Count == 0)
        {
            return null; // no fish that matches the parameters has found
        }
        
        // get a random fish from matched ones
        return potentialFishList[UnityEngine.Random.Range(0, potentialFishList.Count)];
    }

    public void StartFishing()
    {
        // find which fish could be caught
        Fish potentialFish = DeterminePotentialFish();
        Debug.Log($"Distance: {_castDistance} \n" +
                  $"Depth: {_depth} \n" +
                  $"Bait: {_bait} \n" +
                  $"Potential fish: {potentialFish}");
        if (potentialFish != null)
        {
            _currentFish = potentialFish;
            Debug.Log($"Fish found: {potentialFish.FishName}");

            // find probability of catching current fish
            float catchProbability = CalculateCatchProbability();
            Debug.Log($"Catch probability: {catchProbability}");
            if (IsFishCaught(catchProbability))
            {
                Debug.Log($"!!!Success!!!   Fish caught: {_currentFish.FishName}");
                _inventorySystem.AddFish(_currentFish);
                _uiManager.GetInventoryUI().ShowCatchResult(_currentFish, true);
                _uiManager.UpdateInventory(_inventorySystem.GetInventory());
                
                // TODO: 1. save caught fish
                // TODO: 2. add fish to inventory
                // TODO: 3. refresh interface
                // TODO: 4. if it is a first fish of a type, display big picture with salute, otherwise display small picture
                // TODO: 5. every fish has a page in book where you can find more information, information is opening every time when fish of current type is caught
            }
            else
            {
                Debug.Log(" Oops!!!  Fish is not caught. Try again!");
                _uiManager.GetInventoryUI().ShowCatchResult(null, false);
                // TODO: what to do if fish is not caught?
            }
        }
        else
        {
            Debug.Log("No fish found. Try again!");
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
}
