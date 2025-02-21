using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public PlayerManager PlayerManager { get; private set; }
    private WorldStateManager WorldStateManager { get; set; }

    private UIManager UIManager { get; set; }

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
            Debug.Log("!!!!!!!!!!GlobalManager initialized");
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

        if (!_gameSystemsInitialized)
        {
            ResetGameData();
            
        }

        // loading variables from database
        var (playerState, coins, medals, energy, weather, season, inventory) =
            _databaseManager.GetPlayerProgressManager().LoadProgress();

        // basic Systems initialization
        PlayerManager = gameObject.AddComponent<PlayerManager>();
        PlayerManager.Initialize(playerState, coins, medals, energy);

        WorldStateManager = gameObject.AddComponent<WorldStateManager>();
        WorldStateManager.Initialize(weather, season);

        _inventorySystem = new InventorySystem();
        _inventorySystem.Initialize();
        
        UpdateInventoryUI();
    }

    public void StartNewGame()
    {
        _gameSystemsInitialized = false;
        ResetGameData();

        _inventorySystem = new InventorySystem();
        _inventorySystem.Initialize();
        
        //PlayerManager?.Initialize(PlayerManager.PlayerState, PlayerManager.Coins, PlayerManager.Medals, PlayerManager.Energy, new Dictionary<string, int>());
        
        _databaseManager = new DatabaseManager();
        _databaseManager.Initialize();
        
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SceneManager.sceneLoaded += OnNewGameLoaded;
        SceneManager.LoadScene("GameScene");
    }

    private void OnNewGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            SceneManager.sceneLoaded -= OnNewGameLoaded;

            InitializeGameSystems();
            _gameSystemsInitialized = true;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene" && !_gameSystemsInitialized)
        {
            InitializeGameSystems();
            //_gameSystemsInitialized = true;

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void InitializeGameSystems()
    {
        if (_gameSystemsInitialized) return; // re-initialization protection

        var (playerState, coins, medals, energy, weather, season, inventory) =
            _databaseManager.GetPlayerProgressManager().LoadProgress();

        // First initialize all non-UI systems

        PlayerManager = gameObject.AddComponent<PlayerManager>();
        PlayerManager.Initialize(playerState, coins, medals, energy);


        if (_inventorySystem == null)
        {
            _inventorySystem = new InventorySystem();
            _inventorySystem.Initialize();
            _inventorySystem.SetInventory(inventory);
        }


        WorldStateManager = gameObject.AddComponent<WorldStateManager>();
        WorldStateManager.Initialize(weather, season);


        _weatherSystem = new WeatherSystem();
        _weatherSystem.Initialize();

        _weatherSystem.SetWeather(weather);
        _weatherSystem.SetSeason(season);


        _fishingSystem = new FishingSystem(_databaseManager, _weatherSystem);
        _fishingSystem.SetInventorySystem();

        // Start UI initialization as a separate step
        StartCoroutine(InitializeUISystem());

        _gameSystemsInitialized = true; // re-initialization protection
    }

    private IEnumerator InitializeUISystem()
    {
        Debug.Log("Starting UI initialization...");

        // Wait for one frame to ensure all objects are active
        yield return null;

        UIManager = FindObjectOfType<UIManager>();

        if (UIManager == null)
        {
            Debug.LogError("UIManager not found in scene!");
            yield break;
        }

        Debug.Log("Found UIManager, checking UI components...");

        // Wait another frame to ensure all UI components are ready
        yield return null;

        // Verify all required UI components are present
        if (UIManager.GetInventoryUI() == null)
        {
            Debug.LogError("InventoryUI reference is missing in UIManager!");
            yield break;
        }

        if (UIManager.GetResourcesUI() == null)
        {
            Debug.LogError("ResourcesUI reference is missing in UIManager!");
            yield break;
        }

        Debug.Log("All UI components found, initializing...");

        // Initialize UI system
        UIManager.Initialize(_weatherSystem, _fishingSystem, _inventorySystem);

        // Wait for initialization to complete
        yield return null;

        // Update UI elements
        Debug.Log("Updating UI elements...");
        UpdateResourcesUI();
        UpdateInventoryUI();
        _fishingSystem.SetUIManager(UIManager);
        Debug.Log("UI initialization complete!");
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
        SceneManager.sceneLoaded += OnContinueGameLoaded;
        SceneManager.LoadScene("GameScene");
    }

    private void OnContinueGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            SceneManager.sceneLoaded -= OnContinueGameLoaded;
            InitializeGameSystems();
            _gameSystemsInitialized = true;

            if (_fishingSystem == null)
            {
                Debug.Log("Fishing is null. Creating new instance...");
                _fishingSystem = new FishingSystem(_databaseManager, _weatherSystem);
                _fishingSystem.SetInventorySystem();
            }
        }
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

    public void UpdateResourcesUI()
    {
        var resourcesUI = UIManager?.GetResourcesUI();
        if (resourcesUI != null)
        {
            resourcesUI.UpdateUI(PlayerManager.Medals, PlayerManager.Energy, PlayerManager.Coins);
        }
    }

    public void UpdateInventoryUI()
    {
        var inventoryUI = UIManager?.GetInventoryUI();
        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI(GetInventorySystem().GetInventory());
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