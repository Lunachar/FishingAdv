using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerState PlayerState { get; private set; }
    public int Coins { get; private set; }
    public int Medals { get; private set; }
    public int Energy { get; private set; }
    
    public void Initialize(PlayerState playerState, int coins, int medals, int energy)
    {
        PlayerState = playerState ?? new PlayerState
        {
            Level = 1,
            CurrentExperience = 0,
            ExperienceToNextLevel = 10
        };
        Coins = coins;
        Medals = medals;
        Energy = energy;
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
    }

    public void AddMedals(int medals)
    {
        Medals += medals;
    }

    public void AddEnergy(int energy)
    {
        Energy += energy;
    }

    public void DeductCoins(int coins)
    {
        Coins -= coins;
    }

    public void DeductMedals(int medals)
    {
        Medals -= medals;
    }
    public void DeductEnergy(int energy)
    {
        Energy -= energy;
    }

    public void AddExperience(int amount)
    {
        PlayerState.CurrentExperience += amount;
        if (PlayerState.CurrentExperience >= PlayerState.ExperienceToNextLevel)
        {
            PlayerState.Level++;
            PlayerState.CurrentExperience -= PlayerState.ExperienceToNextLevel;
            PlayerState.ExperienceToNextLevel += PlayerState.ExperienceToNextLevel / 2; // Увеличение прогресса.
        }
    }
}