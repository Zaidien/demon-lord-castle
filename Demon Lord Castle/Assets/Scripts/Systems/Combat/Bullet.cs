using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float lifeTime = 3;

    EnemySoundController enemySoundController;

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            
            float newHealth = other.GetComponent<Enemy>().health -= damage;
            //if (!other.GetComponent<EnemySoundController>().hurtIsPlaying && other.GetComponent<Enemy>().health > 0)
            //    other.GetComponent<EnemySoundController>().PlayEnemyHurt();

            if (newHealth > 0)
                other.GetComponent<EnemySoundController>().PlayEnemyHurt();
        }

        Destroy(gameObject);
    }
    
}

