using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UIElements.Slider;

public class FishingUI : MonoBehaviour
{
    public Slider CastDistanceSlider;
    public Slider DepthSlider;
    public Text BaitText;

    private FishingSystem _fishingSystem;

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
}