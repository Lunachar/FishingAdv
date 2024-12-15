using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public WeatherUI weatherUI;
    public FishingUI fishingUI;
    public InventoryUI inventoryUI;
    public ResourcesUI resourcesUI;
    
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
        
        resourcesUI?.Initialize();
    
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
