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

    public ResourcesUI ResourcesUI { get; private set; }
    public InventoryUI InventoryUI { get; private set; }

    private PlayerProgressManager _progressManager;

    private bool _gameSystemsInitialized = false;

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
        _progressManager = new PlayerProgressManager();

        var (playerState, coins, medals, energy, weather, season, inventory) = _progressManager.LoadProgress();

        PlayerManager = gameObject.AddComponent<PlayerManager>();
        PlayerManager.Initialize(playerState, coins, medals, energy);

        InventoryManager = gameObject.AddComponent<InventoryManager>();
        InventoryManager.Initialize(inventory);

        WorldStateManager = gameObject.AddComponent<WorldStateManager>();
        WorldStateManager.Initialize(weather, season);
    }

    // private void StartNewGameInternal()
    // {
    //     PlayerState = new PlayerState()
    //     {
    //         Level = 1,
    //         CurrentExperience = 0,
    //         ExperienceToNextLevel = 10
    //     };
    //     Coins = 0;
    //     Medals = 0;
    //     Energy = 10;
    //     CurrentWeather = "Ясно";
    //     CurrentSeason = "Лето";
    //     Inventory.Clear();
    //
    //     SaveProgress();
    // }


    public void StartNewGame()
    {
        PlayerManager.Initialize(new PlayerState { Level = 1, CurrentExperience = 0, ExperienceToNextLevel = 10 }, 0, 0,
            10);
        InventoryManager.Initialize(new Dictionary<string, int>());
        WorldStateManager.Initialize("Ясно", "Лето");

        SaveProgress();
        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            ResourcesUI = FindObjectOfType<ResourcesUI>();
            InventoryUI = FindObjectOfType<InventoryUI>();

            UpdateResourcesUI();
            UpdateInventoryUI();


            if (InventoryUI != null)
            {
                InventoryUI.UpdateInventoryUI(InventoryManager.GetInventory());
            }

            if (ResourcesUI != null)
            {
                UpdateResourcesUI();
            }

            if (!_gameSystemsInitialized)
            {
                InitializeGameSystems();
                _gameSystemsInitialized = true;
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void InitializeGameSystems()
    {
        var weatherSystem = new WeatherSystem();
        weatherSystem.Initialize();
        
        var databaseManager = new DatabaseManager();
        databaseManager.Initialize();
        
        var fishingSystem = new FishingSystem(databaseManager, weatherSystem);
        fishingSystem.SetInventorySystem(InventoryManager);
    }


    public void SaveProgress()
    {
        _progressManager.SaveProgress(
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
        if (ResourcesUI != null)
        {
            ResourcesUI.UpdateUI(PlayerManager.Coins, PlayerManager.Medals, PlayerManager.Energy);
        }
    }

    private void UpdateInventoryUI()
    {
        if (InventoryUI != null)
        {
            InventoryUI.UpdateInventoryUI(InventoryManager.GetInventory());
        }
    }

    public void LoadMarket()
    {
        SceneManager.LoadScene("MarketScene");
    }
}