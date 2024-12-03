// using System;
//
// [Serializable]
// public class PlayerData
// {
//     public int Level;
//     public int Experience;
//     public int Coins;
//     public int Medals;
//     public int Energy;
//     public Inventory Inventory;
//
//     public PlayerData()
//     {
//         Reset();
//     }
//
//     private void Reset()
//     {
//         Level = 1;
//         Experience = 0;
//         Coins = 0;
//         Medals = 0;
//         Energy = 10;
//         Inventory = new Inventory();
//     }
//
//     public static PlayerData LoadFromDatabse()
//     {
//         return DatabaseManager.LoadPlayerData();
//     }
//
//     public void SaveToDatabase()
//     {
//         DatabaseManager.SavePlayerData(this);
//     }
// }
//
// public class Inventory
// {
// }