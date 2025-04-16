using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowPile : MonoBehaviour
{
    [Header("Initial Setup")]
    public GameObject pillowPile;
    public GameObject pileCollider;
    public AudioSource replinishSource;
    public AudioClip pileReplinish;
    public float ammoGive;
    private float ammoSpawn;

    [Header("Respawn")]
    public bool allowRespawn;
    public float respawnTimer;
    private float respawnCounter;
    private bool alive = true;

    private void Start()
    {
        respawnCounter = respawnTimer;
        ammoSpawn = ammoGive;
        
    }
    private void Update()
    {
        if (respawnCounter <= 0)
        {
            pillowPile.SetActive(true);
            respawnCounter = respawnTimer;
            alive = true;
        }
        else if (!alive && allowRespawn)
        {
            respawnCounter = respawnCounter - Time.deltaTime;
            if (respawnCounter > 0) // Make sure to elimate this line when removing the debug line :)
            Debug.Log($"Respawn timer is at {respawnCounter:0}");

            ammoGive = ammoSpawn;
        }
        else if (!alive && !allowRespawn)
        {
            Debug.Log($"{pillowPile.name} can not respawn");
        }
    }

    public void PlayReplinish()
    {
        replinishSource.clip = pileReplinish;
        replinishSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alive) 
        { 
             if (other.GetComponent<PlayerShoot>() != null)
             {
                 other.GetComponent<PlayerShoot>().ammoAmount += ammoGive;

                // SoundManager.Instance.PlayPlayerSFX(pileReplinish);
                PlayReplinish();

                pillowPile.SetActive(false);
                alive = false;
                Debug.Log("Pile has died");


             } 
            else if (other.GetComponent<Bullet>() != null)
            {
                ammoGive++;
            }
        }
        else if (!alive)
        {
            pileCollider.GetComponent<BoxCollider>().enabled = false;
        }
    }



}
