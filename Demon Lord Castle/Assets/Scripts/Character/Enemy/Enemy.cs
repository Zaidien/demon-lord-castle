using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    private GameObject player;
    private Rigidbody rb;
    private bool attackRange;
    private bool attack;

    private void Awake()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        attackRange = false;
        attack = false;
    }

    private void Update()
    {
        if (health <= 0)
            Destroy(gameObject);

        // Look at Player and detect player in proximity
        transform.LookAt(player.transform.position);
        LayerMask playerLayer = LayerMask.GetMask("Player");
        Collider[] playerHitColliders = Physics.OverlapSphere(transform.position, 1f, playerLayer);
        if (playerHitColliders.Length > 0)
            attackRange = true;

        // Move towards player
        if (attackRange == false)
        {
            Vector3 targetPos = Vector3.Slerp(rb.transform.position, player.transform.position, 1f * Time.deltaTime);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            attack = true;
        }

        // Detect nearby enemies for avoidance
        //LayerMask enemies = LayerMask.GetMask("Enemy");
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f, enemies);

        

    }
}
