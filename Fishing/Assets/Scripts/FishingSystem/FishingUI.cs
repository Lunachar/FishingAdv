using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingUI : MonoBehaviour
{
    public Slider CastDistanceSlider;
    public Slider DepthSlider;
    public Button stopButton;
    public TMP_Text CastValueText;
    public Text BaitText;

    private bool _isMoving = true;
    private float _timeElapsed = 0f;
    private FishingSystem _fishingSystem;

    public float speed = 2f;
    private float minValue = 0.1f;
    private float maxValue = 2f;
    private float centerValue = 1f;

    public void Initialize(FishingSystem fishingSystem)
    {
        _fishingSystem = fishingSystem;
    }

    public void UpdateUI()
    {
        _fishingSystem.SetCastDistance(CastDistanceSlider.value);
        _fishingSystem.SetDepth(DepthSlider.value);
        BaitText.text = _fishingSystem.GetCurrentBait();
    }

    private void Start()
    {
        stopButton.onClick.AddListener(ToggleSliderMovement);
        UpdateButtonText();
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveSlider();
        }
    }

    private void MoveSlider()
    {
        _timeElapsed += Time.deltaTime * speed;

        float sliderValue = Mathf.Sin(_timeElapsed) * 0.5f + 0.5f;
        CastDistanceSlider.value = sliderValue;

        float transformedValue = TransformSliderValue(sliderValue);

        CastValueText.text = transformedValue.ToString("F2");
    }

    private float TransformSliderValue(float sliderValue)
    {
        if (sliderValue <= 0.5f)
        {
            return Mathf.Lerp(minValue, centerValue, sliderValue * 2);
        }
        else
        {
            return Mathf.Lerp(centerValue, maxValue, (sliderValue - 0.5f) * 2);
        }
    }

    private void ToggleSliderMovement()
    {
        _isMoving = !_isMoving;
        Debug.Log("Current Slider Value: " + CastDistanceSlider.value);
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        stopButton.GetComponentInChildren<TMP_Text>().text = _isMoving ? "Stop" : "Start";
    }
}