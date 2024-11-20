using System.Collections.Generic;
using UnityEngine;

public class InventorySystem
{
    private Dictionary<string, int> _fishCount;
    public void Initialize()
    {
        _fishCount = new Dictionary<string, int>();
    }

    public void AddFish(Fish fish)
    {
        if (_fishCount.ContainsKey(fish.FishName))
        {
            _fishCount[fish.FishName]++;
        }
        else
        {
            _fishCount[fish.FishName] = 1;
        }
    }

    public Dictionary<string, int> GetInventory()
    {
        return _fishCount;
    }
    
    
    // public void ShowCatchResult(Fish fish, bool isSuccessful)
    // {
    //     if (isSuccessful)
    //     {
    //         AddFish(fish);
    //         Debug.Log($"You catch: {fish.FishName}");
    //         // ResultImage.Sprite = Fish.Sprite;
    //     }
    //     else
    //     {
    //         Debug.Log("You didn't catch anything");
    //     }
    // }
}
