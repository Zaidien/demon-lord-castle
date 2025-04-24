using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioSource attackSource;

    [Header("AudioClips")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip attackClip;

    public void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            footstepSource.clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepSource.Play();
        }
    }

    public void PlayJump()
    {
        jumpSource.clip = jumpClip;
        jumpSource.Play();
    }

    public void PlayAttack()
    {
        attackSource.clip = attackClip;
        attackSource.Play();
    }



}
