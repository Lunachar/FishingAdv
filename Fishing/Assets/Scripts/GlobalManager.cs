using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public PlayerManager PlayerManager { get; private set; }
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
        if (_databaseManager == null)
        {
            _databaseManager = new DatabaseManager();
            _databaseManager.Initialize();
        }

        // loading variables from database
        var (playerState, coins, medals, energy, weather, season, inventory) =
            _databaseManager.GetPlayerProgressManager().LoadProgress();

        // basic Systems initialization
        PlayerManager = gameObject.AddComponent<PlayerManager>();
        PlayerManager.Initialize(playerState, coins, medals, energy);

        WorldStateManager = gameObject.AddComponent<WorldStateManager>();
        WorldStateManager.Initialize(weather, season);

        _inventorySystem = gameObject.AddComponent<InventorySystem>();
        _inventorySystem.Initialize();
    }

    public void StartNewGame()
    {
        _gameSystemsInitialized = false;
        ResetGameData();
        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene" && !_gameSystemsInitialized)
        {
            InitializeGameSystems();
            _gameSystemsInitialized = true;


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
            _inventorySystem = gameObject.AddComponent<InventorySystem>();
            _inventorySystem.Initialize();
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
            Debug.Log("Fishing system initialized");
            _fishingSystem = new FishingSystem(_databaseManager, _weatherSystem);
        }

        if (UIManager == null && _fishingSystem != null)
        {
            UIManager = FindObjectOfType<UIManager>();
            if (UIManager != null)
            {
                Debug.Log("Initializing UI");
                Debug.Log($"3.is _fSui null? {(_fishingSystem == null).ToString()}");
                //UIManager.Initialize(_weatherSystem, _fishingSystem, _inventorySystem);
            }
        }
        else
        {
            Debug.Log("UIManager already initialized");
        }

        _fishingSystem.SetInventorySystem(_inventorySystem);
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
            _inventorySystem.GetInventory());
        foreach (var fish in _inventorySystem.GetInventory())
        {
            Debug.Log($"Fish: {fish.Key} Count: {fish.Value}");
        }
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

    private void ResetGameData()
    {
        _databaseManager.GetPlayerProgressManager().ResetGameData();
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void AddFishToInventory(Fish fish, int count)
    {
        if (_inventorySystem == null)
        {
            Debug.Log("Inventory system not initialized");
            return;
        }

        _inventorySystem.AddFish(fish, count);
        Debug.Log($"Added fish {fish.FishName} to inventory. Count: {count}");
        SaveProgress();
        UpdateInventoryUI();
    }

    private void UpdateResourcesUI()
    {
        var resourcesUI = UIManager?.GetResourcesUI();
        if (resourcesUI != null)
        {
            resourcesUI.UpdateUI(PlayerManager.Coins, PlayerManager.Medals, PlayerManager.Energy);
        }
    }

    private void UpdateInventoryUI()
    {
        var inventoryUI = UIManager?.GetInventoryUI();
        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI(_inventorySystem.GetInventory());
        }
    }

    public FishingSystem GetFishingSystem()
    {
        return _fishingSystem;
    }

    public InventorySystem GetInventorySystem()
    {
        return _inventorySystem;
    }

    public WeatherSystem GetWeatherSystem()
    {
        return _weatherSystem;
    }
    public void LoadMarket()
    {
        SceneManager.LoadScene("MarketScene");
    }
    
}