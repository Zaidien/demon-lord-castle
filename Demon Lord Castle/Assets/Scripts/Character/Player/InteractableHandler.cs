using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableHandler : MonoBehaviour
{
    public float interactionRange = 3f;
    public Camera playerCamera;
    public GameObject interactTextUI;
    public string sceneToLoad = "NextSceneName"; // Replace with your target scene name

    // private SoundManager soundManager;

    private void Awake()
    {
       // soundManager = GetComponent<SoundManager>();
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {

            // Colliding with scene changing doors
            if (hit.collider.CompareTag("Door"))
            {
                if (interactTextUI == null)
                {
                    Debug.LogWarning("Interact Text has not been set!");
                    return;
                }


                interactTextUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (sceneToLoad == null)
                    {
                        Debug.LogWarning("No scene to load!");
                        return;
                    }
                    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                    SceneManager.LoadScene(currentSceneIndex + 1);
                    SoundManager.instance?.PlaySelect();
                    interactTextUI.SetActive(false);

                    if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
                    {
                        SceneManager.LoadScene(currentSceneIndex + 1);
                    }
                    else
                    {
                        Debug.LogWarning("No more scenes in build settings.");
                    }

                }
            }
            else
            {
                interactTextUI.SetActive(false);
            }
        }
        else
        {
            interactTextUI.SetActive(false);
        }
    }
}
