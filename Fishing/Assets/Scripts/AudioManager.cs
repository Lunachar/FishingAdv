using System;
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
    
    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
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
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        _audioSource.clip = audioClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
}