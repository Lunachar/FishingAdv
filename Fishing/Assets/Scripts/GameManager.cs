using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager
{
    private UIManager _uiManager;
    private FishingSystem _fishingSystem;
    private WeatherSystem _weatherSystem;
    private IdleSkillSystem _idleSkillSystem;
    private DatabaseManager _databaseManager;

    public void Initialize()
    {
        Debug.Log("GM");
        //_uiManager = gameObject.AddComponent<UIManager>();
        //_fishingSystem = new FishingSystem();
        _weatherSystem = new WeatherSystem();
        //_idleSkillSystem = new IdleSkillSystem();
        // _databaseManager = new DatabaseManager();

        //_uiManager.Initialize();
        
        // _fishingSystem.Initialize();
        _weatherSystem.Initialize(_weatherSystem);
        // _idleSkillSystem.Initialize();
        //_databaseManager.Initialize();
    }
}
