using System.Collections.Generic;
using UnityEngine;

public class InventorySystem
{
    private Dictionary<string, int> _inventory = new();
    
    public void Initialize()
    {
        _inventory = new Dictionary<string, int>();
    }

    public Dictionary<string, int> GetInventory()
    {
        return _inventory;
    }

    private void AddItem(string item, int amount)
    {
        if (_inventory.ContainsKey(item))
        {
            _inventory[item] += amount;
        }
        else
        {
            _inventory.Add(item, amount);
        }
        
        Debug.Log($"Item {item} added. New count: {_inventory[item]}");
    }

    public void AddFish(Fish fish, int amount = 1)
    {
        AddItem(fish.FishName, amount);
    }

    public void RemoveItem(string item, int amount)
    {
        if (_inventory.ContainsKey(item))
        {
            _inventory[item] -= amount;
            if (_inventory[item] <= 0)
            {
                _inventory.Remove(item);
            }
        }
    }

    public void RemoveFish(Fish fish, int amount = 1)
    {
        RemoveItem(fish.FishName, amount);
    }

    // public InventorySystem GetInventorySystem()
    // {
    //     return InventorySystem;
    // }
}
