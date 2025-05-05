using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Options Menu")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI volumeText;
    [SerializeField] int letterPerSecond;

    [Header("Menu Audio")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource selectSource;
    [SerializeField] AudioSource backSource;
    [SerializeField] AudioSource gameStartSource;
    [SerializeField] AudioSource moveSource;

    [SerializeField] AudioClip selectClip;
    [SerializeField] AudioClip backClip;
    [SerializeField] AudioClip gameStartClip;
    [SerializeField] AudioClip moveClip;
    [SerializeField] AudioClip baseMusic;
    [SerializeField] float fadeDuration;
    public float gameStartLength => gameStartClip.length;

    public static SoundManager instance {get; private set;}
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        } 
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            Load();

        }

        StartCoroutine(FadeToNewMusic(baseMusic, fadeDuration));
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SetVolumeText(volumeText);
        Save();
    }

    public void SetVolumeText(TextMeshProUGUI volumeText)
    {
        volumeText.text = $"{volumeSlider.value * 100:0.0}% ";
    }

    public void PlayMusic(AudioClip newMusic)
    {
        StartCoroutine(FadeToNewMusic(newMusic, fadeDuration));
    }

    public IEnumerator FadeToNewMusic(AudioClip newClip, float fadeDuration)
    {
        yield return musicSource.DOFade(0f, fadeDuration).SetUpdate(true).WaitForCompletion();
        musicSource.clip = newClip;
        musicSource.Play();
        yield return musicSource.DOFade(1f, fadeDuration).SetUpdate(true).WaitForCompletion();
    }



    private void PlaySFX(AudioSource source, AudioClip newClip)
    {
        if (source != null && newClip != null)
        {
            source.clip = newClip;
            source.Play();
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip in PlaySFX");
        }
    }

    // Menu SFX's 

    public void PlaySelect()
    {
        PlaySFX(selectSource, selectClip);
    }

    public float PlayGameStart()
    {
        PlaySFX(gameStartSource, gameStartClip);
        return gameStartClip.length;
    }

    public void PlayBack()
    {
        PlaySFX(backSource, backClip);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
