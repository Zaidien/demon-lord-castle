using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float lifeTime = 3;

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
            other.GetComponent<Enemy>().health -= damage;
            //if (!other.GetComponent<EnemySoundController>().hurtIsPlaying && other.GetComponent<Enemy>().health > 0)
            //    other.GetComponent<EnemySoundController>().PlayEnemyHurt();
        }

        Destroy(gameObject);
    }
    
}

