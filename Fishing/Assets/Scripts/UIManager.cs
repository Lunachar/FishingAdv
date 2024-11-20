using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public WeatherUI weatherUI;
    public FishingUI fishingUI;
    public InventoryUI inventoryUI;
    
    //private WeatherSystem _weatherSystem;
    
    // private IdleSkillsUI _idleSkillsUI;
    // private CatchUI _catchUI;
    //
    public void Initialize(WeatherSystem weatherSystem, FishingSystem fishingSystem, InventorySystem inventorySystem)
    {
        weatherUI?.Initialize(weatherSystem);
        if (weatherUI == null)
        {
            Debug.Log("WeatherUI isn't set.");
        }
        
        fishingUI?.Initialize(fishingSystem);
        if (fishingUI == null)
        {
            Debug.Log("FishingUI isn't set.");
        }
        
        inventoryUI?.UpdateInventoryUI(inventorySystem.GetInventory());
        
     //   _weatherSystem = new WeatherSystem();
        
        // _fishingUI = new FishingUI();

        
        // _idleSkillsUI = new IdleSkillsUI();
        // _catchUI = new CatchUI();
        
        // Initialization
    
        //weatherUI.Initialize(_weatherSystem);
        // _idleSkillsUI.Initialize();
        // _catchUI.Initialize();
    
    }

    private void UpdateWeather()
    {
        weatherUI?.UpdateWeatherUI();
    }

    public void UpdateInventory(Dictionary<string, int> fishInventory)
    {
        inventoryUI?.UpdateInventoryUI(fishInventory);
    }

    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }
}
