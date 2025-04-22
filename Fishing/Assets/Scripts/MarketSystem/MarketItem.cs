using UnityEngine;

[System.Serializable]
public class MarketItem
{
    public string Name;
    public string Description;
    public int Price;
    public Sprite Icon;
    public int EnergyRestoreAmount; // Количество энергии, которое восстанавливает энергетик
    public bool IsEnergyDrink; // Является ли товар энергетиком

    public MarketItem(string name, string description, int price, Sprite icon, int energyRestoreAmount = 0, bool isEnergyDrink = false)
    {
        Name = name;
        Description = description;
        Price = price;
        Icon = icon;
        EnergyRestoreAmount = energyRestoreAmount;
        IsEnergyDrink = isEnergyDrink;
    }
} 