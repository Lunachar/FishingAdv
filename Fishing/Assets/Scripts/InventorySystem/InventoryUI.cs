using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryPanel;
    public TMP_Text InventoryText;

    public void UpdateInventoryUI(Dictionary<string, int> fishInventory)
    {
        InventoryText.text = ""; // Clear the text
        foreach (var fish in fishInventory)
        {
            InventoryText.text += fish.Key + ": " + fish.Value + "\n";
        }
    }

}
