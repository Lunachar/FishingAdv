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
        StartCoroutine(WaitForSystemsToInitialize(weatherSystem, fishingSystem, inventorySystem));
    }

    private IEnumerator WaitForSystemsToInitialize(WeatherSystem weatherSystem, FishingSystem fishingSystem,
        InventorySystem inventorySystem)
    {
        while (weatherSystem == null || fishingSystem == null || inventorySystem == null)
        {
            Debug.Log("Waiting for systems to initialize...");
            yield return null;
        }
        
        Debug.Log("Systems initialized!");
        //weatherUI?.Initialize(weatherSystem);
        //fishingUI?.Initialize(fishingSystem);
        //inventoryUI?.Initialize();
        //resourcesUI?.Initialize();
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