using UnityEngine;
using UnityEngine.UI;

public class MarketButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnMarketButtonClick);
    }

    private void OnMarketButtonClick()
    {
        GlobalManager.Instance.LoadMarket();
    }
} 