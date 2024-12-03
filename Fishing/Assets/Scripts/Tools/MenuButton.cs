using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Button Menu;

    private void Start()
    {
        Menu.onClick.AddListener(() => LoadMainMenuScene());
    }

    private void LoadMainMenuScene()
    {
        GlobalManager.Instance.SaveProgress();
        SceneManager.LoadScene("MainMenuScene");
    }
}
