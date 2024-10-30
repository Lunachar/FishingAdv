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
}
