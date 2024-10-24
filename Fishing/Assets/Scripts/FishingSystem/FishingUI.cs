using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FishingUI : MonoBehaviour
{
    public Slider CastDistanceSliderChoose;
    public TMP_Text Text_ChooseCastValue;
    [Space(10)]
    public Slider CastDistanceSliderSet;
    public TMP_Text Text_SetCastValue;
    [Space(10)]
    public Slider DepthSlider;
    public Button ButtonStopStart;
    public TMP_Text BaitText;

    private bool _isMoving = false;
    private float _timeElapsed = 0f;
    private FishingSystem _fishingSystem;

    public float speed = 2f;
    private float _minValue;
    private float _maxValue;
    private float _centerValue;

    public void Initialize(FishingSystem fishingSystem)
    {
        _fishingSystem = fishingSystem;
    }

    public void UpdateUI()
    {
        _fishingSystem.SetCastDistance(CastDistanceSliderSet.value);
        _fishingSystem.SetDepth(DepthSlider.value);
        BaitText.text = _fishingSystem.GetCurrentBait();
    }

    private void Start()
    {
        CastDistanceSliderSet.gameObject.SetActive(false);
        ButtonStopStart.onClick.AddListener(ToggleSliderMovement);
        _centerValue = CastDistanceSliderChoose.value;
        
        CastDistanceSliderChoose.onValueChanged.AddListener(OnChooseCastDistanceChanged);
        
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
        _isMoving = !_isMoving;
        Debug.Log("Current Slider Value: " + CastDistanceSliderSet.value);
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        ButtonStopStart.GetComponentInChildren<TMP_Text>().text = _isMoving ? "Stop" : "Start";
    }
}