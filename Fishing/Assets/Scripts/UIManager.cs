using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public WeatherUI weatherUI;
    public FishingUI fishingUI;
    
    
    
    //private WeatherSystem _weatherSystem;
    
    // private IdleSkillsUI _idleSkillsUI;
    // private CatchUI _catchUI;
    //
    public void Initialize(WeatherSystem weatherSystem, FishingSystem fishingSystem)
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
        
        
     //   _weatherSystem = new WeatherSystem();
        
        // _fishingUI = new FishingUI();

        
        // _idleSkillsUI = new IdleSkillsUI();
        // _catchUI = new CatchUI();
        
        // Initialization
    
        //weatherUI.Initialize(_weatherSystem);
        // _idleSkillsUI.Initialize();
        // _catchUI.Initialize();
    
    }

    private void Update()
    {
        weatherUI?.UpdateWeatherUI();
    }
}
