using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image IconImage;
    public TMP_Text NameText;
    //public TMP_Text DescriptionText;
    public TMP_Text PriceText;
    public Button ActionButton;
    public TMP_Text ButtonText;
    public Image ButtonImage;

    private MarketItem _item;
    private System.Action _onActionCallback;

    public void Setup(MarketItem item, System.Action onActionCallback, bool isSelling, bool isButtonActive)
    {
        _item = item;
        _onActionCallback = onActionCallback;

        // Обновляем UI
        if (item.Icon != null)
        {
            IconImage.sprite = item.Icon;
            IconImage.gameObject.SetActive(true);
        }
        else
        {
            IconImage.gameObject.SetActive(false);
        }

        NameText.text = item.Name;
        //DescriptionText.text = item.Description;
        PriceText.text = $"{item.Price}";
        
        // Устанавливаем текст кнопки в зависимости от типа действия
        ButtonText.text = isSelling ? "Продать" : "Купить";

        // Настраиваем кнопку действия
        ActionButton.onClick.RemoveAllListeners();
        ActionButton.onClick.AddListener(OnActionButtonClick);
        
        // Устанавливаем состояние кнопки
        ActionButton.interactable = isButtonActive;
        if (ButtonImage != null)
        {
            ButtonImage.color = isButtonActive ? Color.white : new Color(0.7f, 0.7f, 0.7f, 1f);
        }
    }

    private void OnActionButtonClick()
    {
        _onActionCallback?.Invoke();
    }
} 