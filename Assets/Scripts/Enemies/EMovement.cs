using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementType 
{
    Walk,
    Run,
    Injured
}


public class EMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public EController controller;
    [SerializeField] private GameObject body;
    [SerializeField] public Animator animator;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private BarracadeController barricadeController;
    [SerializeField] private NavMeshAgent navMeshAgent;


    [Header("Variables")]
    [SerializeField] public bool canMove = true;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float range;

    private void Start()
    {
        InitMovement();

        #region Speed-and-Animation
        int r = Random.Range(1, 100);
        animator.SetFloat("animIndex", r);
        float s = Random.Range(1.6f,4.5f);
        navMeshAgent.speed = s;
        #endregion


    }
    
    private void Update()
    {
        if (canMove && controller.hasEnteredMap) 
        {
            navMeshAgent.destination = player.transform.position;
            body.transform.forward = gameObject.transform.forward;
        }
    }

    private void InitMovement()
    {
        controller = GetComponent<EController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = body.GetComponent<Animator>();

        barricadeController = GameObject.FindGameObjectWithTag("BM").GetComponent<BarracadeController>();
        player = GameObject.FindGameObjectWithTag("Player");

        moveSpeed = 0.15f;
        range = 1.1f;

        targetTransform = barricadeController.FindClosetBarrierToMe(transform.position).transform;
        navMeshAgent.destination = targetTransform.position;
    }

    public void UpdateTarget(Transform t)
    {
        targetTransform = t;
    }

    public void EnableNavMesh()
    {
        navMeshAgent.enabled = true;
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        navMeshAgent.destination = targetTransform.position;
    }

}


