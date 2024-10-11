using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    // private FishingUI _fishingUI;
    
    public WeatherUI weatherUI;
    
    
    private WeatherSystem _weatherSystem;
    
    // private IdleSkillsUI _idleSkillsUI;
    // private CatchUI _catchUI;
    //
    public void Initialize()
    {
        if (weatherUI == null)
        {
            Debug.Log("WeatherUI isn't set.");
        }
        _weatherSystem = new WeatherSystem();
        
        // _fishingUI = new FishingUI();

        
        // _idleSkillsUI = new IdleSkillsUI();
        // _catchUI = new CatchUI();
        
        // Initialization
    
        // _fishingUI.Initialize();
        weatherUI.Initialize(_weatherSystem);
        // _idleSkillsUI.Initialize();
        // _catchUI.Initialize();
    
    }

    private void Update()
    {
        weatherUI.UpdateWeatherUI();
    }
}
