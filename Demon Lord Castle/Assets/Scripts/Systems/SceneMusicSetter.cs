using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public AudioClip combatMusic;

    void Start()
    {
        if (SoundManager.instance != null && backgroundMusic != null)
        {
            SoundManager.instance.PlayMusic(backgroundMusic);
        }
    }

}
