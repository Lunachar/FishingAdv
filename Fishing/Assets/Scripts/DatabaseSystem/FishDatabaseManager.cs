using System.IO;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;


public class FishDatabaseManager
{
    private string _connectionString = "Data Source=FishDatabase.sqlite;Version=3";

    public FishDatabaseManager()
    {
        CreateDatabase();
        if (!IsDatabaseInitialized())
        {
            InsertFishData();
        }
    }

    private void CreateDatabase()
    {
        if (!System.IO.File.Exists("FishDatabase.sqlite"))
        {
            SqliteConnection.CreateFile("FishDatabase.sqlite");
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Fish (
                                FishName TEXT,
                                PreferredDepth REAL,
                                PreferredCastDistance REAL,
                                ActiveSeason TEXT,
                                ActiveWeather TEXT,
                                Rarity TEXT,
                                PreferredBait TEXT)";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }

    private bool IsDatabaseInitialized()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(*) FROM Fish";
            SqliteCommand command = new SqliteCommand(sql, connection);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }

    private void InsertFishData()
    {
        var fishList = GetFishList();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            foreach (var fish in fishList)
            {
                string sql =
                    @"INSERT INTO Fish (FishName, PreferredDepth, PreferredCastDistance, ActiveSeason, ActiveWeather, Rarity, PreferredBait)
                               VALUES (@FishName, @PreferredDepth, @PreferredCastDistance, @ActiveSeason, @ActiveWeather, @Rarity, @PreferredBait)";

                SqliteCommand command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@FishName", fish.FishName);
                command.Parameters.AddWithValue("@PreferredDepth", fish.PreferredDepth);
                command.Parameters.AddWithValue("@PreferredCastDistance", fish.PreferredCastDistance);
                command.Parameters.AddWithValue("@ActiveSeason", fish.ActiveSeason);
                command.Parameters.AddWithValue("@ActiveWeather", fish.ActiveWeather);
                command.Parameters.AddWithValue("@Rarity", fish.Rarity);
                command.Parameters.AddWithValue("@PreferredBait", fish.PreferredBait);
                command.ExecuteNonQuery();
            }
        }
    }

    private List<Fish> GetFishList()
    {
        return new List<Fish>
        {
            new Fish("Окунь",       5f,  10f, "Лето",   "Ясно",     "Обычная",  "Червь"),
            new Fish("Скумбрия",    9f,  22f, "Осень",  "Ясно",     "Обычная",  "Червь"),
            new Fish("Форель",      4f,   8f, "Весна",  "Облачно",  "Обычная",  "Насекомое"),
            new Fish("Луциан",      5f,  11f, "Осень",  "Облачно",  "Обычная",  "Насекомое"),
            new Fish("Сом",         3f,   9f, "Лето",   "Туман",    "Обычная",  "Червь"),
            new Fish("Камбала",     4f,   9f, "Весна",  "Туман",    "Обычная",  "Червь"),
            new Fish("Треска",      7f,  14f, "Зима",   "Снег",     "Обычная",  "Червь"),
            new Fish("Линь",        6f,  10f, "Весна",  "Туман",    "Обычная",  "Тесто"),   

            new Fish("Сельдь",      2f,   5f, "Зима",   "Снег",     "Необычная", "Насекомое"),
            new Fish("Угорь",       3f,   7f, "Зима",   "Мелкий дождь", "Необычная", "Насекомое"),
            new Fish("Карп",        6f,  12f, "Осень",  "Мелкий дождь", "Необычная", "Тесто"),
            new Fish("Осётр",       6f,  18f, "Осень",  "Облачно",  "Необычная", "Тесто"),
            new Fish("Тунец",      10f,  25f, "Лето",   "Ветрено",  "Необычная", "Креветка"),
            new Fish("Палтус",      9f,  19f, "Весна",  "Ясно",     "Необычная", "Мальки"),

            new Fish("Щука",        8f,  20f, "Весна",  "Шторм",    "Редкая",   "Мальки"),
            new Fish("Барракуда",  10f,  28f, "Лето",   "Ясно",     "Редкая",   "Креветка"),
            new Fish("Сёмга",       7f,  15f, "Зима",   "Дождь",    "Редкая",   "Мальки"),
            new Fish("Марлин",     14f,  32f, "Лето",   "Дождь",    "Редкая",   "Креветка"),
            new Fish("Акула",      15f,  35f, "Осень",  "Шторм",    "Редкая",   "Мальки"),
            new Fish("Меч-рыба",   12f,  30f, "Лето",   "Ветрено",  "Редкая",   "Креветка")
        };



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
            
                // correspond different languages names
                string englishName = _fishNameMapping.ContainsKey(name) ? _fishNameMapping[name] : name;
                
                // load sprite from Resources folder
                Sprite fishSprite = Resources.Load<Sprite>($"Pictures/FishSprites/{englishName}") ?? Resources.Load<Sprite>($"Pictures/FishSprites/Cod");

                return new Fish(name, preferredDepth, preferredCastDistance, activeSeason, activeWeather, rarity,
                    preferredBait)
                {
                    Sprite = fishSprite
                };
            }
        }
        return null;    // no fish found
    }
}