using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float bulletSpeed;
    public float fireRate, bulletDamage;
    public float ammoAmount;
    public float ammoMax;
    public bool isAuto;

    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;
    public GameObject gunPrefab;

    [Header("HUD Setup")]
    [SerializeField] Image gunIcon;
    public TextMeshProUGUI ammoText;

    private float timer;

    private Vector3 playerScale;

    public static PlayerShoot Instance;

    PlayerSoundController soundController;

    private void Awake()
    {
        Instance = this;
        soundController = GetComponent<PlayerSoundController>();

    }

    private void Start()
    {
        ammoText.text = $"{ammoAmount} / {ammoMax}";
    }

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

    private void Shoot()
    {
        GameObject piwwoBullet = Instantiate(
            bulletPrefab, 
            bulletSpawnTransform.position, 
            bulletSpawnTransform.rotation, 
            GameObject.FindGameObjectWithTag("WorldObjectHolder").transform);

       piwwoBullet.transform.localScale = transform.localScale;

        soundController.PlayAttack();

        piwwoBullet.GetComponent<Rigidbody>().AddForce(
            bulletSpawnTransform.forward * bulletSpeed, 
            ForceMode.Impulse);

        piwwoBullet.GetComponent<Bullet>().damage = bulletDamage;

        timer = 1;
        ammoAmount--;

        UpdateAmmo();
    }

    public void UpdateAmmo()
    {
        if (ammoAmount >= ammoMax * .50)
        {
            gunIcon.color = Color.green;
        }
        else if (ammoAmount >= ammoMax * .25)
        {
            gunIcon.color = Color.yellow;
        }
        else if(ammoAmount >= ammoMax * .10)
        {
            gunIcon.color = Color.red;
        }
        else if (ammoAmount == 0)
        {
            gunIcon.color = Color.black;
        }

        ammoText.text = $"{ammoAmount} / {ammoMax}";
    }
}
