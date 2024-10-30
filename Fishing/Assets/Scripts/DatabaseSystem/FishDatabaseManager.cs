using System.IO;
using System.Collections.Generic;
using Mono.Data.Sqlite;


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
            new Fish("Bass", 5f, 10f, "Summer", "Clear", "Common", "Worm"),
            new Fish("Trout", 4f, 8f, "Spring", "Cloudy", "Common", "Insect"),
            new Fish("Carp", 6f, 12f, "Autumn", "Drizzle", "Uncommon", "Dough"),
            new Fish("Salmon", 7f, 15f, "Winter", "Rain", "Rare", "Minnow"),
            new Fish("Catfish", 3f, 9f, "Summer", "Fog", "Common", "Worm"),
            new Fish("Pike", 8f, 20f, "Spring", "Storm", "Rare", "Minnow"),
            new Fish("Tuna", 10f, 25f, "Summer", "Windy", "Uncommon", "Shrimp"),
            new Fish("Mackerel", 9f, 22f, "Autumn", "Clear", "Common", "Worm"),
            new Fish("Herring", 2f, 5f, "Winter", "Snow", "Uncommon", "Insect"),
            new Fish("Perch", 5f, 10f, "Spring", "Clear", "Common", "Dough"),
            new Fish("Swordfish", 12f, 30f, "Summer", "Windy", "Rare", "Shrimp"),
            new Fish("Shark", 15f, 35f, "Autumn", "Storm", "Rare", "Minnow"),
            new Fish("Eel", 3f, 7f, "Winter", "Drizzle", "Uncommon", "Insect"),
            new Fish("Barracuda", 10f, 28f, "Summer", "Clear", "Rare", "Shrimp"),
            new Fish("Sturgeon", 6f, 18f, "Autumn", "Cloudy", "Uncommon", "Dough"),
            new Fish("Flounder", 4f, 9f, "Spring", "Fog", "Common", "Worm"),
            new Fish("Marlin", 14f, 32f, "Summer", "Rain", "Rare", "Shrimp"),
            new Fish("Snapper", 5f, 11f, "Autumn", "Cloudy", "Common", "Insect"),
            new Fish("Cod", 7f, 14f, "Winter", "Snow", "Common", "Worm"),
            new Fish("Halibut", 9f, 19f, "Spring", "Clear", "Uncommon", "Minnow")
        };
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
            
                return new Fish(name, preferredDepth, preferredCastDistance, activeSeason, activeWeather, rarity, preferredBait);
            }
        }
        return null;    // no fish found
    }
}