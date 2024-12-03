using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;

public class PlayerProgressManager
{
    private string _connectionString = "Data Source=PlayerProgress.sqlite;Version=3;";

    public PlayerProgressManager()
    {
        CreateDatabase();
    }

    private void CreateDatabase()
    {
        if (!File.Exists("PlayerProgress.sqlite"))
        {
            SqliteConnection.CreateFile("PlayerProgress.sqlite");
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS PlayerProgress (
                                PlayerId INTEGER PRIMARY KEY,
                                Level INTEGER NOT NULL,
                                CurrentExperience INTEGER NOT NULL,
                                ExperienceToNextLevel INTEGER NOT NULL,
                                Coins INTEGER NOT NULL,
                                Medals INTEGER NOT NULL,
                                Energy INTEGER NOT NULL,
                                Weather TEXT NOT NULL,
                                Season TEXT NOT NULL)";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();

                string sqlInventory = @"CREATE TABLE IF NOT EXISTS Inventory (
                                        PlayerId INTEGER NOT NULL,                                        
                                        ItemName TEXT PRIMARY KEY,
                                        Count INTEGER NOT NULL)";
                SqliteCommand commandInventory = new SqliteCommand(sqlInventory, connection);
                commandInventory.ExecuteNonQuery();
            }
        }
    }

    public void SaveProgress(PlayerState playerState, int coins, int medals, int energy, string weather, string season,
        Dictionary<string, int> inventory)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string sql =
                @"INSERT OR REPLACE INTO PlayerProgress (PlayerId, Level, CurrentExperience, ExperienceToNextLevel, Coins, Medals, Energy, Weather, Season)
                           VALUES (@PlayerId, @Level, @CurrentExperience, @ExperienceToNextLevel, @Coins, @Medals, @Energy, @Weather, @Season)";
            SqliteCommand command = new SqliteCommand(sql, connection);

            // Set parameters
            command.Parameters.AddWithValue("@PlayerId", 1);
            command.Parameters.AddWithValue("@Level", playerState.Level);
            command.Parameters.AddWithValue("@CurrentExperience", playerState.CurrentExperience);
            command.Parameters.AddWithValue("@ExperienceToNextLevel", playerState.ExperienceToNextLevel);
            command.Parameters.AddWithValue("@Coins", coins);
            command.Parameters.AddWithValue("@Medals", medals);
            command.Parameters.AddWithValue("@Energy", energy);
            command.Parameters.AddWithValue("@Weather", weather);
            command.Parameters.AddWithValue("@Season", season);
            command.ExecuteNonQuery();

            // Inventory saving
            string deleteInventorySql = "DELETE FROM Inventory WHERE PlayerId = 1";
            new SqliteCommand(deleteInventorySql, connection).ExecuteNonQuery();

            foreach (var item in inventory)
            {
                string insertInventorySql =
                    "INSERT INTO Inventory (PlayerId, ItemName, Count) VALUES (@PlayerId, @ItemName, @Count)";
                SqliteCommand insertCommand = new SqliteCommand(insertInventorySql, connection);
                insertCommand.Parameters.AddWithValue("@PlayerId", 1);
                insertCommand.Parameters.AddWithValue("@ItemIName", item.Key);
                insertCommand.Parameters.AddWithValue("@Count", item.Value);
                insertCommand.ExecuteNonQuery();
            }
        }
    }


    public (PlayerState, int, int, int, string, string, Dictionary<string, int>) LoadProgress()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string sql = "SELECT * FROM PlayerProgress WHERE PlayerID = 1";
            SqliteCommand command = new SqliteCommand(sql, connection);
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                PlayerState playerState = new PlayerState
                {
                    Level = reader.GetInt32(1),
                    CurrentExperience = reader.GetInt32(2),
                    ExperienceToNextLevel = reader.GetInt32(3)
                };

                int coins = reader.GetInt32(4);
                int medals = reader.GetInt32(5);
                int energy = reader.GetInt32(6);
                string weather = reader.GetString(7);
                string season = reader.GetString(8);

                Dictionary<string, int> inventory = LoadInventory();

                return (playerState, coins, medals, energy, weather, season, inventory);
            }
        }

        return (null, 0, 0, 10, "Ясно", "Лето", new Dictionary<string, int>()); // Если нет данных
    }

    public void UpdateInventory(string itemName, int count)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string sql = "INSERT INTO Inventory (PlayerId, ItemName, Count) " +
                         "VALUES (@PlayerId, @ItemName, @Count)" +
                         "ON CONFLICT (ItemName) DO UPDATE SET Count = Count + @Count";

            SqliteCommand command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@PlayerId", 1);
            command.Parameters.AddWithValue("@ItemName", itemName);
            command.Parameters.AddWithValue("@Count", count);
            command.ExecuteNonQuery();
        }
    }

    public Dictionary<string, int> LoadInventory()
    {
        Dictionary<string, int> inventory = new Dictionary<string, int>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string sql = "SELECT ItemName, Count FROM Inventory WHERE PlayerId = 1";
            SqliteCommand command = new SqliteCommand(sql, connection);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string itemName = reader.GetString(0);
                int count = reader.GetInt32(1);
                inventory.Add(itemName, count);
            }
        }
        return inventory;
    }
}