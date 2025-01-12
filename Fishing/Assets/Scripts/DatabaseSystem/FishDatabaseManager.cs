using System;
using System.IO;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;


public class FishDatabaseManager
{
    private string _connectionString = $"Data Source=FishDatabase.sqlite;Version=3";


    public FishDatabaseManager()
    {
        CreateDatabase();
        LogTableInfo();
        if (!IsDatabaseInitialized())
        {
            InsertFishData();
        }
    }

    private void CreateDatabase()
    {
        if (!File.Exists("FishDatabase.sqlite"))
        {
            SqliteConnection.CreateFile("FishDatabase.sqlite");
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sqlFdb = @"
                                CREATE TABLE IF NOT EXISTS Fish (
                                FishName TEXT,
                                PreferredDepth REAL,
                                PreferredCastDistance REAL,
                                ActiveSeason TEXT,
                                ActiveWeather TEXT,
                                Rarity TEXT,
                                PreferredBait TEXT,
                                Coins INTEGER,
                                Medals INTEGER,
                                NeededSkill INTEGER,
                                GatheredExperience INTEGER)";
                SqliteCommand command = new SqliteCommand(sqlFdb, connection);
                command.ExecuteNonQuery();
            }
        }
    }

    private bool IsDatabaseInitialized()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        try
        {
            string sql = "SELECT COUNT(*) FROM Fish";
            SqliteCommand command = new SqliteCommand(sql, connection);
            long count = (long)command.ExecuteScalar();
            Debug.Log($"Number of rows in Fish table: {count}");
            return count > 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error checking database initialization: {e.Message}");
            return false;
        }
    }


    private void InsertFishData()
{
    var fishList = GetFishList(); // Метод возвращает список объектов Fish.
    Debug.Log($"Inserting {fishList.Count} fish into the database.");

    string dbPath = Path.Combine(Application.persistentDataPath, "FishDatabase.sqlite");
    using (var connection = new SqliteConnection($"Data Source={dbPath};Version=3;"))
    {
        connection.Open();
        foreach (var fish in fishList)
        {
            try
            {
                string sql = @"
                    INSERT INTO Fish 
                    (FishName, PreferredDepth, PreferredCastDistance, ActiveSeason, ActiveWeather, Rarity, PreferredBait, Coins, Medals, NeededSkill, GatheredExperience) 
                    VALUES 
                    (@FishName, @PreferredDepth, @PreferredCastDistance, @ActiveSeason, @ActiveWeather, @Rarity, @PreferredBait, @Coins, @Medals, @NeededSkill, @GatheredExperience)";
                
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@FishName", fish.FishName);
                command.Parameters.AddWithValue("@PreferredDepth", fish.PreferredDepth);
                command.Parameters.AddWithValue("@PreferredCastDistance", fish.PreferredCastDistance);
                command.Parameters.AddWithValue("@ActiveSeason", fish.ActiveSeason);
                command.Parameters.AddWithValue("@ActiveWeather", fish.ActiveWeather);
                command.Parameters.AddWithValue("@Rarity", fish.Rarity);
                command.Parameters.AddWithValue("@PreferredBait", fish.PreferredBait);
                command.Parameters.AddWithValue("@Coins", fish.Coins);
                command.Parameters.AddWithValue("@Medals", fish.Medals);
                command.Parameters.AddWithValue("@NeededSkill", fish.NeededSkill);
                command.Parameters.AddWithValue("@GatheredExperience", fish.GatheredExperience);

                command.ExecuteNonQuery();
                Debug.Log($"Inserted fish: {fish.FishName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error inserting fish {fish.FishName}: {e.Message}");
            }
        }
    }
}


public List<Fish> GetFishList()
{
    return new List<Fish>
    {
        CreateFish("Окунь",       5f,  10f, "Лето",   "Ясно",     "Обычная",  "Червь",      10, 1, 0),
        CreateFish("Скумбрия",    9f,  22f, "Осень",  "Ясно",     "Обычная",  "Червь",      15, 1, 0),
        CreateFish("Форель",      4f,   8f, "Весна",  "Облачно",  "Обычная",  "Насекомое",  20, 1, 0),
        CreateFish("Луциан",      5f,  11f, "Осень",  "Облачно",  "Обычная",  "Насекомое",  25, 1, 0),
        CreateFish("Сом",         3f,   9f, "Лето",   "Туман",    "Обычная",  "Червь",      30, 1, 0),
        CreateFish("Камбала",     4f,   9f, "Весна",  "Туман",    "Обычная",  "Червь",      35, 1, 0),
        CreateFish("Треска",      7f,  14f, "Зима",   "Снег",     "Обычная",  "Червь",      40, 1, 0),
        CreateFish("Линь",        6f,  10f, "Весна",  "Туман",    "Обычная",  "Тесто",      45, 2, 1),

        CreateFish("Сельдь",      2f,   5f, "Зима",   "Снег",     "Необычная","Насекомое",  50, 2, 1),
        CreateFish("Угорь",       3f,   7f, "Зима",   "Мелкий дождь", "Необычная", "Насекомое",  55, 2, 1),
        CreateFish("Карп",        6f,  12f, "Осень",  "Мелкий дождь", "Необычная", "Тесто",      60, 2, 1),
        CreateFish("Осётр",       6f,  18f, "Осень",  "Облачно",  "Необычная", "Тесто",      65, 2, 1),
        CreateFish("Тунец",      10f,  25f, "Лето",   "Ветрено",  "Необычная", "Креветка",   70, 3, 2),
        CreateFish("Палтус",      9f,  19f, "Весна",  "Ясно",     "Необычная", "Мальки",     75, 3, 2),

        CreateFish("Щука",        8f,  20f, "Весна",  "Шторм",    "Редкая",   "Мальки",    100, 4, 3),
        CreateFish("Барракуда",  10f,  28f, "Лето",   "Ясно",     "Редкая",   "Креветка",  120, 4, 3),
        CreateFish("Сёмга",       7f,  15f, "Зима",   "Дождь",    "Редкая",   "Мальки",    130, 4, 3),
        CreateFish("Марлин",     14f,  32f, "Лето",   "Дождь",    "Редкая",   "Креветка",  150, 5, 4),
        CreateFish("Акула",      15f,  35f, "Осень",  "Шторм",    "Редкая",   "Мальки",    200, 5, 4),
        CreateFish("Меч-рыба",   12f,  30f, "Лето",   "Ветрено",  "Редкая",   "Креветка",  250, 5, 4),
    };
}

private Fish CreateFish(string name, float depth, float distance, string season, string weather, string rarity, string bait, 
    int coins, int medals, int skillLevel)
{
    float baseSkill = 1f;
    float baseExperience = 5f;
    float skillMultiplier = 1.6f;
    float experienceMultiplier = 1.5f;
    
    int neededSkill = (int)(baseSkill * Mathf.Pow(skillMultiplier, skillLevel));
    int gatheredExperience = (int)(baseExperience * Mathf.Pow(experienceMultiplier, skillLevel));

    return new Fish(name, depth, distance, season, weather, rarity, bait, coins, medals, neededSkill,
        gatheredExperience)
    {
        Sprite = Resources.Load<Sprite>($"Pictures/FishSprites/{TranslateNameToEnglish(name)}")
    };
}

private string TranslateNameToEnglish(string russianName)
{
    var nameMapping = new Dictionary<string, string>
    {
        { "Окунь", "Bass" },
        { "Скумбрия", "Mackerel" },
        { "Форель", "Trout" },
        { "Луциан", "Snapper" },
        { "Сельдь", "Herring" },
        { "Угорь", "Eel" },
        { "Карп", "Carp" },
        { "Осётр", "Sturgeon" },
        { "Тунец", "Tuna" },
        { "Палтус", "Halibut" },
        { "Щука", "Pike" },
        { "Барракуда", "Barracuda" },
        { "Сёмга", "Salmon" },
        { "Марлин", "Marlin" },
        { "Акула", "Shark" },
        { "Меч-рыба", "Swordfish" },
        { "Сом", "Catfish" },
        { "Камбала", "Flounder" },
        { "Треска", "Cod" },
        { "Линь", "Tench" },
    };
    
    return nameMapping.ContainsKey(russianName) ? nameMapping[russianName] : "Unknown";
}


private Dictionary<string, string> _fishNameMapping = new Dictionary<string, string>()
    {
        { "Окунь", "Bass" },
        { "Скумбрия", "Mackerel" },
        { "Форель", "Trout" },
        { "Луциан", "Snapper" },
        { "Сом", "Catfish" },
        { "Камбала", "Flounder" },
        { "Треска", "Cod" },
        { "Линь", "Tench" },
        { "Сельдь", "Herring" },
        { "Угорь", "Eel" },
        { "Карп", "Carp" },
        { "Осётр", "Sturgeon" },
        { "Тунец", "Tuna" },
        { "Палтус", "Halibut" },
        { "Щука", "Pike" },
        { "Барракуда", "Barracuda" },
        { "Сёмга", "Salmon" },
        { "Марлин", "Marlin" },
        { "Акула", "Shark" },
        { "Меч-рыба", "Swordfish" }
    };

    public List<Fish> GetAllFishList()
    {
        return GetFishList();
    }


    public Fish LoadFishData(string fishName)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM Fish WHERE FishName = @FishName";
            SqliteCommand command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@FishName", fishName);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string name = reader["FishName"].ToString();
                float preferredDepth = float.Parse(reader["PreferredDepth"].ToString());
                float preferredCastDistance = float.Parse(reader["PreferredCastDistance"].ToString());
                string activeSeason = reader["ActiveSeason"].ToString();
                string activeWeather = reader["ActiveWeather"].ToString();
                string rarity = reader["Rarity"].ToString();
                string preferredBait = reader["PreferredBait"].ToString();
                int price = int.Parse(reader["Coins"].ToString());
                int medals = int.Parse(reader["Medals"].ToString());
                int neededSkill = int.Parse(reader["NeededSkill"].ToString());
                int gatheredExperience = int.Parse(reader["GatheredExperience"].ToString());
            
                // correspond different languages names
                string englishName = _fishNameMapping.ContainsKey(name) ? _fishNameMapping[name] : name;
                
                // load sprite from Resources folder
                Sprite fishSprite = Resources.Load<Sprite>($"Pictures/FishSprites/{englishName}") ?? Resources.Load<Sprite>($"Pictures/FishSprites/Cod");
                if (fishSprite == null)
                {
                    Debug.LogError($"Fish sprite not found for {name}");
                }

                return new Fish(name, preferredDepth, preferredCastDistance, activeSeason, activeWeather, rarity,
                    preferredBait, price, medals, neededSkill, gatheredExperience)
                {
                    Sprite = fishSprite
                };
            }
        }
        return null;    // no fish found
    }
    private void LogTableInfo()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "FishDatabase.sqlite");
        using (var connection = new SqliteConnection($"Data Source={dbPath};Version=3;"))
        {
            connection.Open();
            string sql = "PRAGMA table_info(Fish)";
            SqliteCommand command = new SqliteCommand(sql, connection);
            using (var reader = command.ExecuteReader())
            {
                Debug.Log("Table structure for 'Fish':");
                while (reader.Read())
                {
                    Debug.Log($"Column: {reader["name"]}, Type: {reader["type"]}");
                }
            }
        }
    }

}