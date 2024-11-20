using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    //public GameObject InventoryPanel;
    public TMP_Text InventoryText;
    public Image FishImage;
    public Sprite EmptyHookSprite;
    public ParticleSystem SuccessEffect;
    
    [Header("Audio")]
    public AudioClip successClip;
    public AudioClip failClip;
    public AudioSource _audioSource;
    
    

    private void Start()
    {
        //_audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(bool isSuccessful)
    {
        _audioSource.PlayOneShot(isSuccessful ? successClip : failClip);
    }
    public void UpdateInventoryUI(Dictionary<string, int> fishInventory)
    {
        InventoryText.text = ""; // Clear the text
        foreach (var fish in fishInventory)
        {
            InventoryText.text += fish.Key + ": " + fish.Value + "\n";
        }
    }

    public void PlaySuccessEffect()
    {
        if (SuccessEffect != null)
        {
            SuccessEffect.Stop();
            SuccessEffect.Play();
        }
    }

    public void ShowCatchResult(Fish fish, bool isSuccessful)
    {
        if (isSuccessful)
        {
            Debug.Log($"Successful catch {fish.FishName}");
            FishImage.sprite = fish.Sprite;
            PlaySound(true);
            PlaySuccessEffect();
        }
        else
        {
            FishImage.sprite = EmptyHookSprite;
            PlaySound(false);
        }
    }

}
