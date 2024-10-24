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

}
