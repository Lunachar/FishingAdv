using UnityEngine;

public class GameManager
{
    private UIManager _uiManager;
    private FishingSystem _fishingSystem;
    private WeatherSystem _weatherSystem;
    private InventorySystem _inventorySystem;
    private IdleSkillSystem _idleSkillSystem;
    private DatabaseManager _databaseManager;

    public void Initialize(UIManager uiManager)
    {
        Debug.Log("GM");
        _weatherSystem = new WeatherSystem();
        _weatherSystem.Initialize();
        _databaseManager = new DatabaseManager();
        _databaseManager.Initialize();
        _fishingSystem = new FishingSystem(_databaseManager, _weatherSystem);
        _inventorySystem = new InventorySystem();
        _inventorySystem.Initialize();
        //_fishingSystem.Initialize();
        //_idleSkillSystem = new IdleSkillSystem();

        //_uiManager.Initialize();
        
        _uiManager = uiManager;
        _uiManager.Initialize(_weatherSystem, _fishingSystem, _inventorySystem);
        //_uiManager = new UIManager();
        // _idleSkillSystem.Initialize();
    }
}
