using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health;
    private NavMeshAgent agent;
    private GameObject player;
    //private Rigidbody rb;
    private GameObject attackHitbox;
    private float deathAnimTimer = 2.3f;

    private Vector3 walkPoint;
    bool walkPointSet;
    private float walkPointRange;

    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    [SerializeField] private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    [SerializeField] private LayerMask groundMask, playerMask;


    private GameObject sceneGameObject;
    private SceneMusicSetter sceneMusic;
    public AudioClip combatMusic;
    private AudioClip backgroundMusic;

    [SerializeField] private Animator animator;


    private void Awake()
    {
        player = GameObject.Find("Player");
        //rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        attackHitbox = transform.GetChild(1).gameObject;

        sceneGameObject = GameObject.FindGameObjectWithTag("SceneMusicManager");
        sceneMusic = sceneGameObject.GetComponent<SceneMusicSetter>();
        if (sceneMusic == null)
            Debug.Log("Scene Music is null");

    }

    private void Start()
    {
        if (combatMusic == null)
        {
            combatMusic = sceneMusic.combatMusic;
        }

        if (backgroundMusic == null)
        {
            backgroundMusic = sceneMusic.backgroundMusic;
        }

        animator.SetBool("isAttacking", false);
    }

    private void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("isDying", true);
            deathAnimTimer -= Time.deltaTime;
            if (deathAnimTimer <= 0f)
                Destroy(gameObject);
        }

        // Detecting players and determining range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange && !animator.GetBool("isDying"))
            Patroling();
        if (playerInSightRange && !playerInAttackRange && !animator.GetBool("isDying"))
            ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !animator.GetBool("isDying"))
            AttackPlayer();
    }

    private void Patroling()
    {

        //if (!walkPointSet) 
        //    SearchWalkPoint();

        //if (walkPointSet)
        //    agent.SetDestination(walkPoint);

        //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        //if (distanceToWalkPoint.magnitude < 1f)
        //    walkPointSet = false;

        animator.SetBool("isMoving", false);
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {

        agent.SetDestination(player.transform.position);
        animator.SetBool("isMoving", true);

    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform.position);

        if (!alreadyAttacked)
        {
            attackHitbox.SetActive(true);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        attackHitbox.SetActive(false);
        alreadyAttacked = false;
    }
}
