using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health;
    private NavMeshAgent agent;
    private GameObject player;
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

    private bool isInCombat = false;
    public static List<Enemy> activeEnemies = new List<Enemy>();


    [SerializeField] private Animator animator;
    [SerializeField] private EnemySoundController soundController;


    private void Awake()
    {
        activeEnemies.Add(this);
        player = GameObject.Find("Player");
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

    }

    private void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("isDying", true);
            if (!soundController.deathIsPlaying)
                soundController.PlayEnemyDeath();
            deathAnimTimer -= Time.deltaTime;
            if (deathAnimTimer <= 0f)
            {
                activeEnemies.Remove(this);
                CheckIfShouldSwitchToBackgroundMusic();
                Destroy(gameObject);
            }
        }


        // Detecting players and determining range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        bool shouldBeInCombat = playerInSightRange || playerInAttackRange;

        if (shouldBeInCombat && !isInCombat)
        {
            try
            {
                SoundManager.instance.PlayMusic(combatMusic);
            }
            catch
            {
                Debug.Log("Failed to get Combat Music");
            }
            isInCombat = true;
        }
        //else if (!shouldBeInCombat && isInCombat)
        {
      //      SoundManager.instance.PlayMusic(backgroundMusic);
      //      isInCombat = false;
        }

        if (!playerInSightRange && !playerInAttackRange && !animator.GetBool("isDying"))
            ChasePlayer();
        if (playerInSightRange && !playerInAttackRange && !animator.GetBool("isDying"))
            ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !animator.GetBool("isDying"))
            AttackPlayer();

        
    }

    private void CheckIfShouldSwitchToBackgroundMusic()
    {
        if (activeEnemies.Count == 0)
        {
            try
            {
                SoundManager.instance.PlayMusic(backgroundMusic);
            }
            catch
            {
                Debug.Log("Failed to get Background Music");
            }
        }
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

        // Check to play Idle Sound
        

        animator.SetBool("isMoving", false);
        if (!soundController.idleIsPlaying && !soundController.attackIsPlaying && !soundController.deathIsPlaying && !soundController.hurtIsPlaying)
            soundController.PlayEnemyIdle();
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
        if (!soundController.idleIsPlaying && !soundController.attackIsPlaying && !soundController.deathIsPlaying && !soundController.hurtIsPlaying)
            soundController.PlayEnemyIdle();

    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform.position);

        if (!alreadyAttacked)
        {
            animator.SetBool("isAttacking", true);
            attackHitbox.SetActive(true);
            if (!soundController.attackIsPlaying && !soundController.deathIsPlaying)
                soundController.PlayEnemyAttack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        attackHitbox.SetActive(false);
        alreadyAttacked = false;
        animator.SetBool("isAttacking", false);
    }
}
