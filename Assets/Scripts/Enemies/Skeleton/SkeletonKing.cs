using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(SkeletonAnimatorController))]
public class SkeletonKing : MonoBehaviour
{

    [SerializeField] private TargetScanner m_Scanner;
    [SerializeField] private Transform m_Player;
    [SerializeField] private float m_AttackDelay = 3.0f;

    private NavMeshAgent m_Agent;
    private SkeletonAnimatorController m_Controller;
    private const float k_ScanInterval = 0.5f;
    

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<SkeletonAnimatorController>();
        
    }

    void Start()
    {
        StartCoroutine(ScanForPlayer());
    }

    IEnumerator ScanForPlayer()
    {
        while (true)
        {
            if (m_Scanner.Detect(transform, m_Player))
            {
                StartCoroutine(ReachForPlayer());
                break;
            }
            yield return new WaitForSeconds(k_ScanInterval);
        }
    }

    IEnumerator ReachForPlayer(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        m_Agent.SetDestination(m_Player.position);
        EnableWalk(true);
        while (true)
        {
            m_Agent.SetDestination(m_Player.position);
            if (CalculateDistanceToPlayer() <= m_Agent.stoppingDistance)
            {
                EnableWalk(false);
                m_Controller.Attack(true);
                StartCoroutine("ReachForPlayer", m_AttackDelay);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    float CalculateDistanceToPlayer()
    {
        Vector3 toPlayer = transform.position - m_Player.position;
        toPlayer.y = 0;
        return toPlayer.magnitude;
    }

    void EnableWalk(bool isWalking)
    {
        m_Agent.isStopped = !isWalking;
        m_Controller.Walk(isWalking);
    }

}
