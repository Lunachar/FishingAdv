using UnityEngine;

public class Fish
{
    public string FishName;
    public float PreferredDepth;
    public float PreferredCastDistance;
    public string ActiveSeason;
    public string ActiveWeather;
    public string Rarity;
    public string PreferredBait;
    public int Price;
    public int Medals;
    public int NeededSkill;
    public int GatheredExperience;
    public Sprite Sprite { get; set;}

    public Fish(string fishName, float preferredDepth, float preferredCastDistance, string activeSeason,
        string activeWeather, string rarity, string preferredBait, int price, int medals, int neededSkill, int gatheredExperience)
    {
        FishName = fishName;
        PreferredDepth = preferredDepth;
        PreferredCastDistance = preferredCastDistance;
        ActiveSeason = activeSeason;
        ActiveWeather = activeWeather;
        Rarity = rarity;
        PreferredBait = preferredBait;
        Price = price;
        Medals = medals;
        NeededSkill = neededSkill;
        GatheredExperience = gatheredExperience; 
    }
}