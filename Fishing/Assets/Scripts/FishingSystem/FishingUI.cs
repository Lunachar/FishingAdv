using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingUI : MonoBehaviour
{
    public Slider CastDistanceSlider;
    public Slider DepthSlider;
    public Button stopButton;
    public Text BaitText;

    private bool _isMoving = true;
    private float _timeElapsed = 0f;
    private FishingSystem _fishingSystem;

    public float speed = 2f;

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

    void MoveSlider()
    {
        _timeElapsed += Time.deltaTime * speed;

        float value = Mathf.Sin(_timeElapsed) * 0.5f + 0.5f;
        CastDistanceSlider.value = value;
    }

    void ToggleSliderMovement()
    {
        _isMoving = !_isMoving;
        Debug.Log("Current Slider Value: " + CastDistanceSlider.value);
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        stopButton.GetComponentInChildren<TMP_Text>().text = _isMoving ? "Stop" : "Start";
    }
}