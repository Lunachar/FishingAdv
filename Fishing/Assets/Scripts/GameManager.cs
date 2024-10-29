using UnityEngine;

public class GameManager
{
    private UIManager _uiManager;
    private FishingSystem _fishingSystem;
    private WeatherSystem _weatherSystem;
    private IdleSkillSystem _idleSkillSystem;
    private DatabaseManager _databaseManager;

    public void Initialize(UIManager uiManager)
    {
        Debug.Log("GM");
        //_fishingSystem = new FishingSystem();
        _weatherSystem = new WeatherSystem();
        _weatherSystem.Initialize();
        //_idleSkillSystem = new IdleSkillSystem();
        _databaseManager = new DatabaseManager();

        //_uiManager.Initialize();
        
        // _fishingSystem.Initialize();
        //_uiManager = new UIManager();
        _uiManager = uiManager;
        _uiManager.Initialize(_weatherSystem);
        // _idleSkillSystem.Initialize();
        _databaseManager.Initialize();
    }
}
