using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem Instance { get; private set; }

    private bool _playMusic = true;
    
    [SerializeField]
    private AudioSource _musicSource;
    [SerializeField]
    private AudioSource _audioSource;
    

    private void Awake()
    {
        Instance = this;
    }

    public void Init(bool musicEnabled)
    {
        _playMusic = musicEnabled;
        RefreshMusicStatus();
    }

    public void SwitchMusic()
    {
        _playMusic = !_playMusic;

        RefreshMusicStatus();

        int musicInt = _playMusic ? 1 : 0;
        
        PlayerPrefs.SetInt(Game.MUSIC, musicInt);
    }

    public void RefreshMusicStatus()
    {
        if (_playMusic)
        {
            _musicSource.Play();
        }
        else
        {
            _musicSource.Stop();
        }
    }
}
