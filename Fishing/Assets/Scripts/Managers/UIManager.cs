using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")] public WeatherUI weatherUI;
    public FishingUI fishingUI;
    public InventoryUI inventoryUI;
    public ResourcesUI resourcesUI;

    public void Initialize(WeatherSystem weatherSystem, FishingSystem fishingSystem, InventorySystem inventorySystem)
    {
        // Remove the coroutine and do direct initialization
        if (weatherSystem != null && fishingSystem != null && inventorySystem != null)
        {
            Debug.Log("Initializing UI components...");
            weatherUI?.Initialize();
            fishingUI?.Initialize();
            inventoryUI?.Initialize();
            resourcesUI?.Initialize();
        }
        else
        {
            Debug.LogError("One or more systems are null during UI initialization");
        }
    }

    

    private void UpdateWeather()
    {
        weatherUI?.UpdateWeatherUI();
    }

    // public void SetInventoryUI()
    // {
    //     inventoryUI = FindObjectOfType<InventoryUI>();
    // }
    public void UpdateInventory(Dictionary<string, int> fishInventory)
    {
        inventoryUI?.UpdateInventoryUI(fishInventory);
    }

    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }

    public WeatherUI GetWeatherUI()
    {
        return weatherUI;
    }

    public FishingUI GetFishingUI()
    {
        return fishingUI;
    }

    public ResourcesUI GetResourcesUI()
    {
        return resourcesUI;
    }
}