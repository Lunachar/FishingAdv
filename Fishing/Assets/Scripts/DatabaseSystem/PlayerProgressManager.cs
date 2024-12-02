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
                                ExpirienceToNextLevel INTEGER NOT NULL,
                                Coins INTEGER NOT NULL,
                                Medals INTEGER NOT NULL)";
                SqliteCommand command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }

    public void SaveProgress(PlayerState playerState, int coins, int medals)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            string sql = @"INSERT OR REPLACE INTO PlayerProgress (PlayerId, Level, CurrentExperience, ExpirienceToNextLevel, Coins, Medals)
                           VALUES (@PlayerId, @Level, @CurrentExperience, @ExpirienceToNextLevel, @Coins, @Medals)";
            SqliteCommand command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Level", playerState.Level);
            command.Parameters.AddWithValue("@CurrentExperience", playerState.CurrentExperience);
            command.Parameters.AddWithValue("@ExpirienceToNextLevel", playerState.ExpirienceToNextLevel);
            command.Parameters.AddWithValue("@Coins", coins);
            command.Parameters.AddWithValue("@Medals", medals);
            command.ExecuteNonQuery();
        }
    }


public (PlayerState, int, int) LoadProgress()
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
            return (playerState, coins, medals);
        }
    }
    return (null, 0, 0); // Если нет данных
}

}