using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource enemyIdleSource;
    [SerializeField] private AudioSource enemyAttackSource;
    [SerializeField] private AudioSource enemyHurtSource;
    [SerializeField] private AudioSource enemyDeathSource;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip enemyIdle;
    [SerializeField] private AudioClip enemyAttack;
    [SerializeField] private AudioClip enemyDeath;
    [SerializeField] private AudioClip enemyHurt;

    public bool idleIsPlaying;
    public bool attackIsPlaying;
    public bool deathIsPlaying;
    public bool hurtIsPlaying;

    void Update()
    {
        // check if idle sound clip is playing
        if (enemyIdleSource.isPlaying)
            idleIsPlaying = true;
        else
            idleIsPlaying = false;

        // check if attack sound clip is playing
        if (enemyAttackSource.isPlaying)
            attackIsPlaying = true;
        else
            attackIsPlaying = false;

        // check if death sound clip is playing
        if (enemyDeathSource.isPlaying)
            deathIsPlaying = true;
        else
            deathIsPlaying = false;

        // check if hurt sound clip is playing
        if (enemyHurtSource.isPlaying)
            hurtIsPlaying = true;
        else
            hurtIsPlaying = false;
    }

    public void PlayEnemyIdle()
    {
        enemyIdleSource.clip = enemyIdle;
        enemyIdleSource.Play();
    }

    public void PlayEnemyAttack()
    {
        enemyAttackSource.clip = enemyAttack;
        enemyAttackSource.Play();
    }

    public void PlayEnemyDeath()
    {
        enemyDeathSource.clip = enemyDeath;
        enemyDeathSource.Play();
    }

    public void PlayEnemyHurt()
    {
        enemyHurtSource.clip = enemyHurt;
        enemyHurtSource.Play();
    }
}
