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

    [Header("Music Options")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip baseMusic;
    [SerializeField] float fadeDuration;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

    public IEnumerator FadeToNewMusic(AudioClip newClip, float fadeDuration)
    {
        yield return musicSource.DOFade(0f, fadeDuration).SetUpdate(true).WaitForCompletion();
        musicSource.clip = newClip;
        musicSource.Play();
        yield return musicSource.DOFade(1f, fadeDuration).SetUpdate(true).WaitForCompletion();
    }

    private void PlaySFX(AudioClip newClip)
    {
        
        sfxSource.clip = newClip;
        sfxSource.Play();
        
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
