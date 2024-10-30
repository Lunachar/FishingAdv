public class Fish
{
    public string FishName;
    public float PreferredDepth;
    public float PreferredCastDistance;
    public string ActiveSeason;
    public string ActiveWeather;
    public string Rarity;
    public string PreferredBait;

    public Fish(string fishName, float preferredDepth, float preferredCastDistance, string activeSeason,
        string activeWeather, string rarity, string preferredBait)
    {
        FishName = fishName;
        PreferredDepth = preferredDepth;
        PreferredCastDistance = preferredCastDistance;
        ActiveSeason = activeSeason;
        ActiveWeather = activeWeather;
        Rarity = rarity;
        PreferredBait = preferredBait;
    }
}