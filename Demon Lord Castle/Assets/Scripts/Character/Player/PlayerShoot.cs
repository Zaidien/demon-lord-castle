using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float bulletSpeed;
    public float fireRate, bulletDamage;
    public float ammoAmount;
    public bool isAuto;

    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;
    public GameObject gunPrefab;

    private float timer;

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime / fireRate;

        if (ammoAmount != 0)
        {
            gunPrefab.SetActive(true);
            if (isAuto)
            {
                if (Input.GetButton("Fire1") && timer <= 0)
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1") && timer <= 0)
                {
                    Shoot();
                }
            }
        }
        else
        {
            gunPrefab.SetActive(false);
           
        }
    }

    void Shoot()
    {
        GameObject piwwoBullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation, GameObject.FindGameObjectWithTag("WorldObjectHolder").transform);
        piwwoBullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * bulletSpeed, ForceMode.Impulse);
        piwwoBullet.GetComponent<Bullet>().damage = bulletDamage;

        timer = 1;
        ammoAmount--;
    }
}
