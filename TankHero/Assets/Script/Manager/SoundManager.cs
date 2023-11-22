using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    public GameObject soundSystem;

    private float _bgmVolume;
    private float _sfxVolume;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource moveSource;

    public List<AudioClip> bgmClip;
    
    public List<AudioClip> sfxClip;

    public Slider bgmSlider;
    public Slider sfxSlider;

    private bool _isHide;

    public enum bgmClips
    {
        Bgm1,
        Move
    }

    public enum sfxClips
    {
        StartUp,
        Boom,
        Shout
    }

    private void Awake() 
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        _isHide = false;
        
        PlayMusic(bgmClips.Bgm1);
    }

    private void Update()
    {
        soundSystem.SetActive(_isHide);
        
        bgmSource.volume = bgmSlider.value;
        moveSource.volume = bgmSlider.value;
        sfxSource.volume = sfxSlider.value;
        
    }

    public void Hide()
    {
        _isHide = false;
    }

    public void Show() {
        _isHide = true;
    }

    public void PlayMusic(bgmClips clip)
    {
        bgmSource.loop = true;
        bgmSource.clip = bgmClip[(int)clip];
        bgmSource.enabled = true;
        bgmSource.Play();
    }
    
    public void PlaySFX(sfxClips clip)
    {
        sfxSource.loop = false;
        sfxSource.clip = sfxClip[(int)clip];
        sfxSource.enabled = true;
        sfxSource.Play();
    }

    public void MoveStart()
    {
        moveSource.clip = bgmClip[(int)bgmClips.Move];
        moveSource.enabled = true;
        moveSource.Play();
    }

    public void MoveStop()
    {
        moveSource.Stop();
    }
}