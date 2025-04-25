using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioSource landSource;
    [SerializeField] private AudioSource attackSource;

    [Header("Footsteps")]  // FS = Footsteps
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private List<AudioClip> carpetFSClips;
    [SerializeField] private List<AudioClip> stoneFSClips;
    [SerializeField] private List<AudioClip> tileFSClips;

    [Header("Jumps")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip carpetLandClip;
    [SerializeField] private AudioClip stoneLandClip;
    [SerializeField] private AudioClip tileLandClip;

    [Header("Other")]
    [SerializeField] private AudioClip attackClip;

    public void PlayFootstep()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer))
        {
            SurfaceIdentifier surface = hit.collider.GetComponent<SurfaceIdentifier>();

            if (surface != null)
            {
                List<AudioClip> clipsToUse = null;

                switch (surface.surfaceType)
                {
                    case SurfaceType.Carpet:
                        clipsToUse = carpetFSClips;
                        break;
                    case SurfaceType.Stone:
                        clipsToUse = stoneFSClips;
                        break;
                    case SurfaceType.Tile:
                        clipsToUse = tileFSClips;
                        break;
                }

                if (clipsToUse != null && clipsToUse.Count > 0)
                {
                    AudioClip chosen = clipsToUse[Random.Range(0, clipsToUse.Count)];
                    footstepSource.PlayOneShot(chosen);
                }
            }
        }
    }

    public void PlayJump()
    {
        jumpSource.clip = jumpClip;
        jumpSource.Play();
    }

    public void PlayLand()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer))
        {
            SurfaceIdentifier surface = hit.collider.GetComponent<SurfaceIdentifier>();

            if (surface != null)
            {
                AudioClip clipToUse = null;

                switch (surface.surfaceType)
                {
                    case SurfaceType.Carpet:
                        clipToUse = carpetLandClip;
                        break;
                    case SurfaceType.Stone:
                        clipToUse = stoneLandClip;
                        break;
                    case SurfaceType.Tile:
                        clipToUse = tileLandClip;
                        break;
                }

                if (clipToUse != null)
                {
                    landSource.clip = clipToUse;
                    landSource.Play();
                }
            }
        }
    }

    public void PlayAttack()
    {
        attackSource.clip = attackClip;
        attackSource.Play();
    }



}
