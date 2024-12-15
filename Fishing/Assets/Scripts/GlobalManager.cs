using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public PlayerManager PlayerManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    public WorldStateManager WorldStateManager { get; private set; }

    public UIManager UIManager { get; private set; }

    private DatabaseManager _databaseManager;

    private WeatherSystem _weatherSystem;
    private FishingSystem _fishingSystem;
    private InventorySystem _inventorySystem;

    private bool _gameSystemsInitialized;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Initialize()
    {
        _databaseManager = new DatabaseManager();
        _databaseManager.Initialize();
    }

    public void StartNewGame()
    {
        _gameSystemsInitialized = false;


        //SaveProgress();
        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            if (!_gameSystemsInitialized)
            {
                InitializeGameSystems();
                _gameSystemsInitialized = true;
            }

            UpdateResourcesUI();
            UpdateInventoryUI();


            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void InitializeGameSystems()
    {
        var (playerState, coins, medals, energy, weather, season, inventory) =
            _databaseManager.GetPlayerProgressManager().LoadProgress();

        if (PlayerManager == null)
        {
            PlayerManager = gameObject.AddComponent<PlayerManager>();
            PlayerManager.Initialize(playerState, coins, medals, energy);
        }

        if (_inventorySystem == null)
        {
            _inventorySystem = new InventorySystem();
            _inventorySystem.Initialize();
        }

        if (InventoryManager == null)
        {
            InventoryManager = gameObject.AddComponent<InventoryManager>();
            InventoryManager.Initialize(_inventorySystem.GetInventory());
        }
        else
        {
            InventoryManager.Initialize(inventory);
        }

        if (WorldStateManager == null)
        {
            WorldStateManager = gameObject.AddComponent<WorldStateManager>();
        }

        if (_weatherSystem == null)
        {
            _weatherSystem = new WeatherSystem();
            _weatherSystem.Initialize();
        }
        else
        {
            _weatherSystem.SetWeather(weather);
            _weatherSystem.SetSeason(season);
        }

        if (_fishingSystem == null)
        {
            _fishingSystem = new FishingSystem(_databaseManager, _weatherSystem);
        }

        if (UIManager == null)
        {
            UIManager = FindObjectOfType<UIManager>();
            UIManager.Initialize(_weatherSystem, _fishingSystem, _inventorySystem);
        }
        
        _fishingSystem.SetInventoryManager(InventoryManager);
        _fishingSystem.SetUIManager(UIManager);
        
    }


    public void SaveProgress()
    {
        _databaseManager.GetPlayerProgressManager().SaveProgress(
            PlayerManager.PlayerState,
            PlayerManager.Coins,
            PlayerManager.Medals,
            PlayerManager.Energy,
            WorldStateManager.CurrentWeather,
            WorldStateManager.CurrentSeason,
            InventoryManager.GetInventory()
        );
    }

    public void UpdateWeatherAndSeason(string weather, string season)
    {
        WorldStateManager.UpdateWeather(weather);
        WorldStateManager.UpdateSeason(season);
        SaveProgress();
    }

    public void ContinueGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void AddFishToInventory(string item, int count)
    {
        InventoryManager.AddItem(item, count);
        SaveProgress();
        UpdateInventoryUI();
    }

    private void UpdateResourcesUI()
    {
        if (UIManager.GetResourcesUI() != null)
        {
            UIManager.GetResourcesUI().UpdateUI(PlayerManager.Coins, PlayerManager.Medals, PlayerManager.Energy);
        }
    }

    private void UpdateInventoryUI()
    {
        if (UIManager.GetInventoryUI() != null)
        {
            UIManager.GetInventoryUI().UpdateInventoryUI(InventoryManager.GetInventory());
        }
    }

    public void LoadMarket()
    {
        SceneManager.LoadScene("MarketScene");
    }
}