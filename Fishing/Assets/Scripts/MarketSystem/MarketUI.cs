using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject MarketPanel;
    public Transform Content; // Новое поле для контента
    public GameObject ItemPrefab;
    public Button CloseButton;
    public TMP_Text CoinsText;
    public TMP_Text EnergyText;
    public Image BackgroundOverlay; // Затемнение фона

    [Header("Animation Settings")]
    public float FadeInDuration = 0.3f;
    public float ScaleInDuration = 0.3f;

    [Header("Market Items")]
    public List<MarketItem> EnergyDrinks = new List<MarketItem>
    {
        new MarketItem("Энергетик (+10)", "Восстанавливает энергию", 50, null, 10, true),
        new MarketItem("Супер энергетик (+25)", "Восстанавливает энергию", 100, null, 25, true),
        new MarketItem("Мега энергетик (+50)", "Восстанавливает энергию", 200, null, 50, true)
    };

    private Dictionary<string, int> _fishInventory;
    private Dictionary<string, int> _fishPrices = new Dictionary<string, int>();
    private CanvasGroup _canvasGroup;
    private RectTransform _panelRectTransform;
    private bool _isAnimating = false;
    private bool _isOpen = false;

    private void Start()
    {
        _canvasGroup = MarketPanel.GetComponent<CanvasGroup>();
        _panelRectTransform = MarketPanel.GetComponent<RectTransform>();
        
        // Начальное состояние
        _canvasGroup.alpha = 0f;
        _panelRectTransform.localScale = Vector3.zero;
        MarketPanel.SetActive(false);
        if (BackgroundOverlay != null)
        {
            BackgroundOverlay.gameObject.SetActive(false);
        }

        CloseButton.onClick.AddListener(CloseMarket);
        LoadFishPrices();
    }

    private void LoadFishPrices()
    {
        // Загружаем цены рыб из базы данных
        var fishList = GlobalManager.Instance.GetDatabaseManager().GetFishDatabaseManager().GetAllFishList();
        foreach (var fish in fishList)
        {
            _fishPrices[fish.FishName] = fish.Coins;
        }
    }

    public void OpenMarket()
    {
        if (_isAnimating || _isOpen) return;
        _isOpen = true;
        
        MarketPanel.SetActive(true);
        if (BackgroundOverlay != null)
        {
            BackgroundOverlay.gameObject.SetActive(true);
        }
        _fishInventory = GlobalManager.Instance.GetInventorySystem().GetInventory();
        UpdateUI();
        StartCoroutine(AnimateOpen());
    }

    private IEnumerator AnimateOpen()
    {
        _isAnimating = true;
        float elapsedTime = 0f;
        
        // Анимация прозрачности
        while (elapsedTime < FadeInDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / FadeInDuration);
            if (BackgroundOverlay != null)
            {
                BackgroundOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.5f, elapsedTime / FadeInDuration));
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 1f;
        if (BackgroundOverlay != null)
        {
            BackgroundOverlay.color = new Color(0, 0, 0, 0.5f);
        }

        // Анимация масштаба
        elapsedTime = 0f;
        while (elapsedTime < ScaleInDuration)
        {
            _panelRectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / ScaleInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _panelRectTransform.localScale = Vector3.one;
        _isAnimating = false;
    }

    public void CloseMarket()
    {
        if (_isAnimating || !_isOpen) return;
        _isOpen = false;
        StartCoroutine(AnimateClose());
    }

    private IEnumerator AnimateClose()
    {
        _isAnimating = true;
        float elapsedTime = 0f;
        
        // Анимация масштаба
        while (elapsedTime < ScaleInDuration)
        {
            _panelRectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime / ScaleInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _panelRectTransform.localScale = Vector3.zero;

        // Анимация прозрачности
        elapsedTime = 0f;
        while (elapsedTime < FadeInDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / FadeInDuration);
            if (BackgroundOverlay != null)
            {
                BackgroundOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0.5f, 0f, elapsedTime / FadeInDuration));
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 0f;
        if (BackgroundOverlay != null)
        {
            BackgroundOverlay.color = new Color(0, 0, 0, 0f);
            BackgroundOverlay.gameObject.SetActive(false);
        }
        MarketPanel.SetActive(false);
        _isAnimating = false;
    }

    private void UpdateUI()
    {
        Debug.Log("UpdateUI started");
        CoinsText.text = $"{GlobalManager.Instance.PlayerManager.Coins}";
        EnergyText.text = $"{GlobalManager.Instance.PlayerManager.Energy}";

        // Очищаем контейнер с товарами
        Debug.Log($"Content child count before clear: {Content.childCount}");
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
        Debug.Log($"Content child count after clear: {Content.childCount}");

        // Сначала создаем элементы для продажи рыбы
        Debug.Log($"Creating fish items, inventory count: {_fishInventory?.Count ?? 0}");
        if (_fishInventory != null)
        {
            foreach (var fish in _fishInventory)
            {
                if (_fishPrices.ContainsKey(fish.Key))
                {
                    CreateFishSellItem(fish.Key, fish.Value, _fishPrices[fish.Key]);
                }
            }
        }

        // Затем создаем элементы для энергетиков
        Debug.Log($"Creating energy drinks, count: {EnergyDrinks.Count}");
        foreach (var drink in EnergyDrinks)
        {
            CreateMarketItem(drink);
        }
        Debug.Log($"Content child count after creation: {Content.childCount}");
    }

    private void CreateMarketItem(MarketItem item)
    {
        Debug.Log($"Creating market item: {item.Name}");
        GameObject itemObj = Instantiate(ItemPrefab, Content);
        Debug.Log($"Item instantiated, parent: {itemObj.transform.parent?.name}");
        
        MarketItemUI itemUI = itemObj.GetComponent<MarketItemUI>();
        if (itemUI == null)
        {
            Debug.LogError("MarketItemUI component not found on instantiated item!");
            return;
        }
        
        bool canAfford = GlobalManager.Instance.PlayerManager.Coins >= item.Price;
        itemUI.Setup(item, () =>
        {
            if (canAfford)
            {
                GlobalManager.Instance.GetPlayerManager().DeductCoins(item.Price);
                if (item.IsEnergyDrink)
                {
                    GlobalManager.Instance.PlayerManager.AddEnergy(item.EnergyRestoreAmount);
                    AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.buyEnergyClip);
                }
                UpdateUI();
            }
            else
            {
                Debug.Log("Недостаточно монет!");
            }
        }, false, canAfford);
    }

    private void CreateFishSellItem(string fishName, int count, int price)
    {
        Debug.Log($"Creating fish sell item: {fishName} (x{count})");
        GameObject itemObj = Instantiate(ItemPrefab, Content);
        Debug.Log($"Fish item instantiated, parent: {itemObj.transform.parent?.name}");
        
        MarketItemUI itemUI = itemObj.GetComponent<MarketItemUI>();
        if (itemUI == null)
        {
            Debug.LogError("MarketItemUI component not found on instantiated fish item!");
            return;
        }
        
        MarketItem fishItem = new MarketItem(
            $"{fishName} (x{count})",
            $"Продать за {price}",
            price,
            null
        );
        
        itemUI.Setup(fishItem, () =>
        {
            if (count > 0)
            {
                GlobalManager.Instance.PlayerManager.AddCoins(price);
                GlobalManager.Instance.GetInventorySystem().RemoveItem(fishName, 1);
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.sellFishClip);
                EventManager.UpdateInventory();
                UpdateUI();
            }
        }, true, true);
    }
} 