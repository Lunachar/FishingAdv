using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button NewGameButton;
    public Button ContinueButton;

    private void Start()
    {
        NewGameButton.onClick.AddListener(() => GlobalManager.Instance.StartNewGame());
        ContinueButton.onClick.AddListener(() => GlobalManager.Instance.ContinueGame());
    }
}
