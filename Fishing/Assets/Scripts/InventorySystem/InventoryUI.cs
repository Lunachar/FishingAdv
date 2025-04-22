using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    //public GameObject InventoryPanel;
    public TMP_Text InventoryText;
    public Image FishImage;
    public Sprite EmptyHookSprite;
    public CanvasGroup FishImageCanvasGroup;
    public ParticleSystem SuccessEffect;
    
    [Header("Timing")]
    public float displayDuration = 3f;
    public float fadeDuration = 1f;

    public int checker;


    private void Awake()
    {
        EventManager.OnInventoryUpdated += UpdateInventoryUI;
    }

    private void OnDestroy()
    {
        EventManager.OnInventoryUpdated -= UpdateInventoryUI;
    }
    
    public void Initialize()
    {
        checker += 1;
        FishImageCanvasGroup.alpha = 0f;
        Debug.Log($"Initialized {GetType().Name}");
    }

    // public void PlaySound(bool isSuccessful)
    // {
    //     _audioSource.PlayOneShot(isSuccessful ? successClip : failClip);
    // }
    public void UpdateInventoryUI()
    {
        var inventory = GlobalManager.Instance.GetInventorySystem().GetInventory();
        Debug.LogError(InventoryText.text);
        InventoryText.text = ""; // Clear the text
        InventoryText.ForceMeshUpdate(); // Force the text to update
        foreach (var fish in inventory)
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
        Debug.Log("ShowCatchResult called. FishImage: " + FishImage + ", isSuccessful: " + isSuccessful);
        if (isSuccessful)
        {
            Debug.Log($"Successful catch {fish.FishName}");
            checker += 1;
            Debug.Log(checker);
            FishImage.sprite = fish.Sprite;
            AudioManager.Instance.PlaySoundSequence(new List<AudioClip>
            {
                AudioManager.Instance.successClip,
                AudioManager.Instance.fishFlappingTailClip
            });
            
            PlaySuccessEffect();
        }
        else
        {
            FishImage.sprite = EmptyHookSprite;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.failClip);
        }
        StopCoroutine(nameof(ShowAndFadeOut));
        //StopAllCoroutines();
        StartCoroutine(ShowAndFadeOut());
    }

    private IEnumerator ShowAndFadeOut()
    {
        FishImageCanvasGroup.alpha = 1f;
        
        yield return new WaitForSeconds(displayDuration);
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            FishImageCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        FishImageCanvasGroup.alpha = 0f;  // Reset the alpha
    }
}
