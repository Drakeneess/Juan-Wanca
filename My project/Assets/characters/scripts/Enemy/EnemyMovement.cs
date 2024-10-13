using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Player;
    [SerializeField]
    private Animator Animator;
    public float UpdateRate = 0.1f;
    public NavMeshAgent Agent;
    private Coroutine FollowCoroutine;

    private const string IsWalking = "corre";
    private const string Atacking = "golpea";

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private void Update()
    {
        Animator.SetBool(IsWalking, Agent.velocity.magnitude > 0.01f);
    }

    public void StartChasing()
    {
        if (FollowCoroutine == null)
        {
            FollowCoroutine = StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("Called StartChasing on Enemy that is already chasing! This is likely a bug in some calling class!");
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (enabled)
        {
            Agent.SetDestination(Player.transform.position - (Player.transform.position - transform.position).normalized * 0.5f);
            yield return Wait;
        }
    }
}
