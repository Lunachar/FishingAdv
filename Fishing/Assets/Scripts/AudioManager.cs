using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Menu Audio")]
    public AudioClip menuMusic;

    [Header("Game Audio")]
    public List<AudioClip> GameMusicClips;
    public AudioClip successClip;
    public AudioClip failClip;
    public AudioClip noFishClip;
    public AudioClip fishFlappingTailClip;
    
    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _sfxSource = gameObject.AddComponent<AudioSource>();
        
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayRandomGameMusic()
    {
        if(GameMusicClips.Count == 0) return;
        
        AudioClip randomClip = GameMusicClips[Random.Range(0, GameMusicClips.Count)];
        PlayMusic(randomClip);
    }

    private void PlayMusic(AudioClip audioClip)
    {
        if (_musicSource.isPlaying)
        {
            _musicSource.Stop();
        }
        _musicSource.clip = audioClip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        _sfxSource.Stop();
        _sfxSource.PlayOneShot(audioClip);
    }

    public void PlaySoundSequence(List<AudioClip> audioClips)
    {
        StartCoroutine(PlaySoundSuquenceCorutine(audioClips));
    }

    private IEnumerator PlaySoundSuquenceCorutine(List<AudioClip> audioClips)
    {
        foreach (AudioClip audioClip in audioClips)
        {
            _sfxSource.Stop();
            _sfxSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(audioClip.length);
        }
    }
}