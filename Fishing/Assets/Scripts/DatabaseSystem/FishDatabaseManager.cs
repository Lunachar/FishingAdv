

using System;
using System.Data.SQLite;

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
            SQLiteConnection.CreateFile("FishDatabase.sqlite");
            using (var connection = new SQLiteConnection(_connectionString))
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
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }
    private bool IsDatabaseInitialized()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(*) FROM Fish";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }

    private void InsertFishData()
    {
        throw new System.NotImplementedException();
    }


}