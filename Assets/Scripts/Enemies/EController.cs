using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public ZombieSpawner zmSpawner;
    [SerializeField] private EMovement movementComponent;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerController;

    [Header("Variables")]
    [SerializeField] private int healthVal;
    [SerializeField] public bool hasEnteredMap;
    [SerializeField] public bool isActive;
    [SerializeField] private float despawnCounter;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform spawnPoint;
    
    private void Start()
    {
        InitEnemyController();
    }

    private void Update()
    {
        if (attackCooldown > 0) 
            attackCooldown -= Time.deltaTime;
        
        if (!hasEnteredMap)
        {
            #region despawning
            despawnCounter -= Time.deltaTime;
            if (despawnCounter <= 0)
            {
                zmSpawner.ZombieDespawned();

            }
            #endregion
        }
        else
        {
            if (!isActive)
                return;

            // change targeting while down to prevent swarming while down
            if (playerController.isDowned)
            {
                movementComponent.UpdateTarget(spawnPoint);
            }
            else if (!playerController.isDowned)
            {
                movementComponent.UpdateTarget(player.transform);
            }

            //Check for and to attack
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < 1.1f && attackCooldown <= 0)
            {
                movementComponent.animator.SetTrigger("attack");
                playerController.TakeDamage(50);
                attackCooldown += 2.5f;
            }
        }
    }

    private void InitEnemyController()
    {
        movementComponent = GetComponent<EMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        healthVal = GameManager.instance.roundNum * 100;
        hasEnteredMap = false;
        isActive = true;
        despawnCounter = 100;
        attackCooldown = 0;
        spawnPoint = transform;
    }

    public void TakeDamage(int d)
    {
        if (!movementComponent.canMove)
            return;

        healthVal -= d;

        if (healthVal <= 0)
        {
            isActive = false;
            Death();
        }
        else
        {
            return;
        }
    }

    private void Death()
    {
        zmSpawner.DecreaseZCount();
        movementComponent.animator.SetTrigger("death");
        movementComponent.canMove = false;
    }

    public float GetHealth()
    {
        return healthVal;
    }

}
