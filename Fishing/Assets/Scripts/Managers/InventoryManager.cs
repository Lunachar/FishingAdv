using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<string, int> _inventory = new Dictionary<string, int>();

    public void Initialize(Dictionary<string, int> initialInventory)
    {
        _inventory = initialInventory ?? new Dictionary<string, int>();
    }

    public Dictionary<string, int> GetInventory()
    {
        return _inventory;
    }

    public void AddItem(string item, int amount)
    {
        if (_inventory.ContainsKey(item))
        {
            _inventory[item] += amount;
        }
        else
        {
            _inventory.Add(item, amount);
        }
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
}
