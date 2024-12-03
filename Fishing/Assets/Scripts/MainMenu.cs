using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        AudioManager.Instance.PlayRandomGameMusic();
        SceneManager.LoadScene("GameScene");
    }
}
