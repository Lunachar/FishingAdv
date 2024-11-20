using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager
{
    private FishDatabaseManager _fishDatabaseManager; 
    
    public void Initialize()
    {
        _fishDatabaseManager = new FishDatabaseManager();
    }

    public FishDatabaseManager GetFishDatabaseManager()
    {
        return _fishDatabaseManager;
    }
    public void Connect()
    {
        
    }

    public void SaveProgress(string key, string value)
    {
        
    }

    public string LoadProgress(string key)
    {
        return "LoadedValue";
    }

    public Fish LoadFishData(string fishName)
    {
        return _fishDatabaseManager.LoadFishData(fishName);
    }

    // public List<Fish> GetFishByConditions(float castDistance, float depth, string bait)
    // {
    //     List<Fish> suitableFish= new List<Fish>();
    //
    //     foreach (Fish fish in _fishDatabaseManager.GetAllFishList())
    //     {
    //         if (Mathf.Abs(fish.PreferredCastDistance - castDistance) <= 5f &&
    //             Mathf.Abs(fish.PreferredDepth - depth) <= 3f &&
    //             fish.PreferredBait == bait)
    //         {
    //             suitableFish.Add(fish);
    //         }
    //     }
    //     return suitableFish;
    // }

    public List<Fish> GetFishMatchingParameters(float castDistance, float depth, string bait)
    {
        List<Fish> matchingFish = new List<Fish>();

        foreach (Fish fish in _fishDatabaseManager.GetAllFishList())
        {
            if (Mathf.Abs(fish.PreferredCastDistance - castDistance) <= 5f&&
                Mathf.Abs(fish.PreferredDepth - depth) <= 3f &&
                fish.PreferredBait == bait)
                matchingFish.Add(fish);
        }

        foreach (Fish fish in matchingFish)
        {
            Debug.Log($"MATCHING {fish.FishName}");
        }
        return matchingFish;
    }
}
