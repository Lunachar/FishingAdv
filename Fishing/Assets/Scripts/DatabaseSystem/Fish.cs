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
    public int Coins;
    public int Medals;
    public int NeededSkill;
    public int GatheredExperience;
    public Sprite Sprite { get; set;}

    public Fish(string fishName, float preferredDepth, float preferredCastDistance, string activeSeason,
        string activeWeather, string rarity, string preferredBait, int coins, int medals, int neededSkill, int gatheredExperience)
    {
        FishName = fishName;
        PreferredDepth = preferredDepth;
        PreferredCastDistance = preferredCastDistance;
        ActiveSeason = activeSeason;
        ActiveWeather = activeWeather;
        Rarity = rarity;
        PreferredBait = preferredBait;
        Coins = coins;
        Medals = medals;
        NeededSkill = neededSkill;
        GatheredExperience = gatheredExperience; 
    }
}