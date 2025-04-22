using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public float health;
    private GameObject player;
    private Rigidbody rb;
    private bool attackRange;
    private bool chasePlayer;


    private void Awake()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        chasePlayer = true;
    }

    private void Update()
    {
        if (health <= 0)
            Destroy(gameObject);

        if (chasePlayer)
            agent.SetDestination(player.transform.position);


        // Look at Player and detect player in proximity
        transform.LookAt(player.transform.position);
        LayerMask playerLayer = LayerMask.GetMask("Player");
        Collider[] playerHitColliders = Physics.OverlapSphere(transform.position, 3f, playerLayer);
        if (playerHitColliders.Length > 0)
            attackRange = true;

        // Detect nearby enemies for avoidance
        //LayerMask enemies = LayerMask.GetMask("Enemy");
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f, enemies);

        

    }
}
