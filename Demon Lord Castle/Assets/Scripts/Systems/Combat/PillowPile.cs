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
    private PlayerShoot playerShoot;

    [Header("Respawn")]
    public bool allowRespawn;
    public float respawnTimer;
    private float respawnCooldown;
    private bool alive = true;

    private void Start()
    {
        respawnCooldown = respawnTimer;
        ammoSpawn = ammoGive;
        
    }
    private void Update()
    {
        if (respawnCooldown <= 0)
        {
            pillowPile.SetActive(true);
            pileCollider.GetComponent<Collider>().enabled = true;
            respawnCooldown = respawnTimer;
            alive = true;
        }
        else if (!alive && allowRespawn)
        {
            respawnCooldown = respawnCooldown - Time.deltaTime;
            if (respawnCooldown > 0) // Make sure to elimate this line when removing the debug line :)
            Debug.Log($"Respawn timer is at {respawnCooldown:0}");

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
        if(!alive)
        {
            pileCollider.GetComponent<BoxCollider>().enabled = false;
            return;
        }

        PlayerShoot playerShoot = other.GetComponent<PlayerShoot>();

        if (playerShoot != null)
        {
            playerShoot.ammoAmount += ammoGive;
            playerShoot.ammoAmount = Mathf.Min(playerShoot.ammoAmount, playerShoot.ammoMax);

            playerShoot.UpdateAmmo();

            PlayReplinish();

            pillowPile.SetActive(false);
            alive = false;

            Debug.Log("Pile has died");
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /*if (alive) 
        { 
             if (other.GetComponent<PlayerShoot>() != null)
             {
                playerShoot = other.GetComponent<PlayerShoot>();
                 playerShoot.ammoAmount += ammoGive;

                if (playerShoot.ammoAmount > playerShoot.ammoMax)
                    playerShoot.ammoAmount = playerShoot.ammoMax;

                // SoundManager.instance.PlayPlayerSFX(pileReplinish);
                PlayReplinish();

                other.GetComponent<PlayerShoot>().ammoAmount = playerShoot.ammoAmount;
                

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
        } */
    }



}
