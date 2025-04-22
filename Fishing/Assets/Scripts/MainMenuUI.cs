using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button NewGameButton;
    public Button ContinueButton;

    private void Start()
    {
        NewGameButton.onClick.RemoveAllListeners();
        ContinueButton.onClick.RemoveAllListeners();
    
        NewGameButton.onClick.AddListener(StartNewGame);
        ContinueButton.onClick.AddListener(ContinueGame);
    }

    private void StartNewGame()
    {
        GlobalManager.Instance.StartNewGame();
        Debug.Log("Button clicked");
    }

    private void ContinueGame()
    {
        GlobalManager.Instance.ContinueGame();
    }

}
