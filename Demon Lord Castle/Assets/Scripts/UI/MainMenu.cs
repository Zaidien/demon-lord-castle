using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private SoundManager soundManager;

    

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    public void WantsToExit()
    {
        Application.Quit();
        Debug.Log("Quit The Game");
    }

    public void PlayGame()
    {
        if (soundManager == null)
        {
            Debug.LogError("SoundManager is not assigned in MainMenu!");
            return;
        }

            StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        float delay = soundManager.PlayGameStart();
        yield return new WaitForSeconds(delay + 0.1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
