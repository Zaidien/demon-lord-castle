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
    [SerializeField] private AudioSource damageSource;

    [Header("Pitch Variety")]
    [SerializeField] float minPitch = 0.9f;
    [SerializeField] float maxPitch = 1.1f;

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
    [SerializeField] private List<AudioClip> attackClips;
    [SerializeField] private List<AudioClip> damageClips;

    private void Start()
    {
        // Debug Purposes 
        if (footstepSource == null)
            Debug.LogWarning("Footstep Audio Source is not set!");
        if (jumpSource == null)
            Debug.LogWarning("Jump Audio Source is not set!");
        if (landSource == null)
            Debug.LogWarning("Land Audio Source is not set!");
        if (attackSource == null)
            Debug.LogWarning("Attack Audio Source is not set!");
    }
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

                    // Set random pitch
                    footstepSource.pitch = Random.Range(minPitch, maxPitch);

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
        if (attackClips != null && attackClips.Count > 0)
        {
            AudioClip chosen = attackClips[Random.Range(0, attackClips.Count)];

            attackSource.pitch = Random.Range(minPitch, maxPitch);
            attackSource.PlayOneShot(chosen);
        }
        else if (attackClips == null)
            Debug.LogWarning("Attack Clips is Null");
        else if (attackClips.Count <= 0)
            Debug.LogWarning("No Attack Clips");
    }
    public void PlayDamage()
    {
        if (damageClips != null && damageClips.Count > 0)
        {
            AudioClip chosen = damageClips[Random.Range(0, damageClips.Count)];

            damageSource.pitch = Random.Range(minPitch, maxPitch);
            damageSource.PlayOneShot(chosen);
        }
        else if (damageClips == null)
            Debug.LogWarning("Damage Clips is Null");
        else if (damageClips.Count <= 0)
            Debug.LogWarning("No Damage Clips");
    }



}
