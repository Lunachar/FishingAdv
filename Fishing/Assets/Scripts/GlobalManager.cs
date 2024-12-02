using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;


    public PlayerData PlayerData { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayerData()
    {
        PlayerData = PlayerData.LoadFromDatabase();
        if (PlayerData == null)
        {
            PlayerData = new PlayerData();
        }
    }

    public void SavePlayerData()
    {
        PlayerData.SaveToDatabase();
    }

    public void StartNewGame()
    {
        // PlayerData.Reset();
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMarket()
    {
        SceneManager.LoadScene("MarketScene");
    }
}