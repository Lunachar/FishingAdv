using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FishingUI : MonoBehaviour
{
    #region UI Elements

    [Header("Cast Distance Selection")]
    public Slider CastDistanceSliderChoose;
    public TMP_Text Text_ChooseCastValue;
    
    [Header("Casting Setting")]
    public Slider CastDistanceSliderSet;
    public TMP_Text Text_SetCastValue;
    
    [Header("Depth Selection")]
    public Slider DepthSlider;
    public TMP_Text DepthText;
    
    [Header("Bait Selection")]
    public TMP_Dropdown BaitDropdown;
    
    [Header("Control Button")]
    public Button ButtonStopStart;

    #endregion

    #region Private Fields

    private bool _isMoving = false;       // Flag for slider movement
    private float _timeElapsed = 0f;      // Elapsed time for slider oscillation
    private FishingSystem _fishingSystem; // Reference to the fishing system
    private string _selectedBait;         // Holds selected bait from dropdown
    private float _selectedDepth;         // Holds selected depth from depth slider
    private float _selectedCastDistance = 1f;  // Holds selected cast distance from cast distance slider

    [Header("Slider Motion Setting")]
    public float speed = 2f;
    private float _minValue;
    private float _maxValue;
    private float _centerValue;

    #endregion

    public void Initialize(FishingSystem fishingSystem)
    {
        if(fishingSystem != null)
        {
            _fishingSystem = fishingSystem;
            Debug.Log($"FS in FUI ok.");
        }
        else
        {
            Debug.Log($"FS in FUI is null.");
        }
    }

    private void UpdateUI()
    {
        //_fishingSystem.SetCastDistance(float.Parse(Text_SetCastValue.text));
        _fishingSystem.SetCastDistance(_selectedCastDistance);
        _fishingSystem.SetDepth(_selectedDepth);
        _fishingSystem.SetBait(_selectedBait);
    }

    private void Start()
    {
        CastDistanceSliderSet.gameObject.SetActive(false);
        ButtonStopStart.onClick.AddListener(ToggleSliderMovement);
        _centerValue = CastDistanceSliderChoose.value;
        
        CastDistanceSliderChoose.onValueChanged.AddListener(OnChooseCastDistanceChanged);
        
        DepthSlider.onValueChanged.AddListener(delegate { DepthSliderValueChanged();});
        
        BaitDropdown.onValueChanged.AddListener(delegate { BaitDropdownValueChanged();});
        PopulateBaitOptions();
    }

    private void PopulateBaitOptions()
    {
        BaitDropdown.ClearOptions();
        List<string> baitOptions = new List<string> { "Червь", "Насекомое", "Тесто", "Мальки", "Креветка", "Хлеб" };
        BaitDropdown.AddOptions(baitOptions);
        _selectedBait = BaitDropdown.options[BaitDropdown.value].text;
        UpdateUI();
    }

    private void BaitDropdownValueChanged()
    {
        _selectedBait = BaitDropdown.options[BaitDropdown.value].text;
        Debug.Log("Selected bait: " + _selectedBait);
    }

    private string GetSelectedBait()
    {
        return _selectedBait;
    }

    private void DepthSliderValueChanged()
    {
        _selectedDepth = DepthSlider.value;
        DepthText.text = "Выбранная глубина: " + _selectedDepth.ToString("F1") + " метров";
        Debug.Log("Selected depth: " + _selectedDepth);
    }

    private float GetSelectedDepth()
    {
        return _selectedDepth;
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveSlider();
        }

        Text_ChooseCastValue.text = CastDistanceSliderChoose.value.ToString(CultureInfo.CurrentCulture);
    }

    private void OnChooseCastDistanceChanged(float value)
    {
        _centerValue = value;
        _minValue = _centerValue > 4 ? _centerValue - 3f : 1f;
        _maxValue = _centerValue + 5f;
        
        CastDistanceSliderSet.gameObject.SetActive(true);
        _isMoving = true;
        UpdateButtonText();
    }

    private void MoveSlider()
    {
        _timeElapsed += Time.deltaTime * speed;

        float sliderValue = Mathf.Sin(_timeElapsed) * 0.5f + 0.5f;
        CastDistanceSliderSet.value = sliderValue;

        float transformedValue = TransformSliderValue(sliderValue);

        Text_SetCastValue.text = transformedValue.ToString("F2");
        _selectedCastDistance = float.Parse(Text_SetCastValue.text);
    }

    private float TransformSliderValue(float sliderValue)
    {
        if (sliderValue <= 0.5f)
        {
            return Mathf.Lerp(_minValue, _centerValue, sliderValue * 2);
        }
        else
        {
            return Mathf.Lerp(_centerValue, _maxValue, (sliderValue - 0.5f) * 2);
        }
    }

    private void ToggleSliderMovement()
    {
        GlobalManager.Instance.PlayerManager.DeductEnergy(1);
        _isMoving = !_isMoving;
        Debug.Log("Current Slider Value: " + CastDistanceSliderSet.value);
        UpdateButtonText();

        if (!_isMoving)
        {
            UpdateUI();
            _fishingSystem.StartFishing();
        }
    }

    private void UpdateButtonText()
    {
        ButtonStopStart.GetComponentInChildren<TMP_Text>().text = _isMoving ? "Заброс" : "Перебросить";
    }
}