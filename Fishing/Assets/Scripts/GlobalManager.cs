using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;
    
    // Player's common parameters
    public PlayerState PlayerState { get; private set; }
    public int Coins { get; private set; }
    public int Medals { get; private set; }
    public int Energy { get; private set; }
    
    // Weater and Season
    public string CurrentWeather { get; private set; }
    public string CurrentSeason { get; private set; }
    
    // Inventory
    public Dictionary<string, int> Inventory { get; private set; } = new Dictionary<string, int>();
    
    private PlayerProgressManager _progressManager;
    private ResourcesUI _resourcesUI;
    
    
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
        if (playerState != null)
        {
            PlayerState = playerState;
            Coins = coins;
            Medals = medals;
            Energy = energy;
            CurrentWeather = weather;
            CurrentSeason = season;
            Inventory = inventory;
        }
        else
        {
            StartNewGameInternal();
        }
    }

    private void StartNewGameInternal()
    {
        PlayerState = new PlayerState()
        {
            Level = 1,
            CurrentExperience= 0,
            ExperienceToNextLevel = 10
        };
        Coins = 0;
        Medals = 0;
        Energy = 10;
        CurrentWeather = "Ясно";
        CurrentSeason = "Лето";
        Inventory.Clear();
        
        SaveProgress();
        
    }


    public void StartNewGame()
    {
        StartNewGameInternal();
        SceneManager.LoadScene("GameScene");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _resourcesUI = FindObjectOfType<ResourcesUI>();
        UpdateResourcesUI();
    }

    public void SaveProgress()
    {
        _progressManager.SaveProgress(PlayerState, Coins, Medals, Energy, CurrentWeather, CurrentSeason, Inventory);
    }

    public void UpdateWeatherAndSeason(string weather, string season)
    {
        CurrentWeather = weather;
        CurrentSeason = season;
        SaveProgress();
    }

    public void UpdateInventory(string item, int count)
    {
        if (Inventory.ContainsKey(item))
        {
            Inventory[item] += count;
        }
        else
        {
            Inventory.Add(item, count);
        }
        SaveProgress();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        SaveProgress();
        UpdateResourcesUI();
    }

    public void AddMedals(int medals)
    {
        Medals += medals;
        SaveProgress();
        UpdateResourcesUI();
    }

    public void AddEnergy(int energy)
    {
        Energy += energy;
        SaveProgress();
        UpdateResourcesUI();
    }

    public void DeductEnergy(int energy)
    {
        Energy = Mathf.Max(Energy - energy, 0);
        SaveProgress();
        UpdateResourcesUI();
    }

    public void AddExperience(int experience)
    {
        PlayerState.CurrentExperience += experience;

        if (PlayerState.CurrentExperience >= PlayerState.ExperienceToNextLevel)
        {
            PlayerState.Level++;
            PlayerState.CurrentExperience -= PlayerState.ExperienceToNextLevel;
            PlayerState.ExperienceToNextLevel += Mathf.CeilToInt(PlayerState.ExperienceToNextLevel * 0.5f);
        }
        SaveProgress();
        UpdateResourcesUI();
    }

    public void AddFishToInventory(string fishName, int count)
    {
        _progressManager.UpdateInventory(fishName, count);
    }

    private void UpdateResourcesUI()
    {
        if (_resourcesUI != null)
        {
            _resourcesUI.UpdateUI(Coins, Medals, Energy);
        }
    }

    public void LoadMarket()
    {
        SceneManager.LoadScene("MarketScene");
    }
}