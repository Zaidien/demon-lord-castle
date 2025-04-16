using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    private GameObject player;
    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (health <= 0)
            Destroy(gameObject);

        // Move towards player
        Vector3 targetPos = Vector3.Slerp(rb.transform.position, player.transform.position, 1f * Time.deltaTime);
        transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);

        // Detect nearby enemies for avoidance
        //LayerMask enemies = LayerMask.GetMask("Enemy");
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f, enemies);

        

    }
}
