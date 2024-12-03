using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public AudioClip buttonClickSFX;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;
    private AudioSource _audioSource;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClick()
    {
        _audioSource.clip = buttonClickSFX;
        _audioSource.Play();
    }

    public void Result(bool _result)
    {
        var _clip = _result ? correctSFX : wrongSFX;
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
}
