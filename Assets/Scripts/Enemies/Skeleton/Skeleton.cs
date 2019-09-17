using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(SkeletonAnimatorController))]
public class Skeleton : MonoBehaviour
{

    [SerializeField] private TargetScanner m_Scanner;
    private static List<Transform> m_Players;
    [SerializeField] private float m_AttackDelay = 3.0f;

    private NavMeshAgent m_Agent;
    private SkeletonAnimatorController m_Controller;
    private const float k_ScanInterval = 0.5f;
    private int followingIndex;
    

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<SkeletonAnimatorController>();
        if (m_Players == null)
        {
            m_Players = new List<Transform>();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject ob in objects)
            {
                m_Players.Add(ob.transform);
            }
        }
    }

    void Start()
    {
        StartCoroutine(ScanForPlayer());
    }

    IEnumerator ScanForPlayer()
    {
        while (true)
        {
            for (int i = 0; i < m_Players.Count; i++)
            {
                if (m_Scanner.Detect(transform, m_Players[i]))
                {
                    followingIndex = i;
                    StartCoroutine(ReachForPlayer());
                    break;
                }
            }
            yield return new WaitForSeconds(k_ScanInterval);
        }
    }

    IEnumerator ReachForPlayer(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        m_Agent.SetDestination(m_Players[followingIndex].position);
        EnableWalk(true);
        while (true)
        {
            m_Agent.SetDestination(m_Players[followingIndex].position);
            if (CalculateDistanceToPlayer(followingIndex) <= m_Agent.stoppingDistance)
            {
                EnableWalk(false);
                m_Controller.Attack(true);
                StartCoroutine("ReachForPlayer", m_AttackDelay);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    float CalculateDistanceToPlayer(int i)
    {
        Vector3 toPlayer = transform.position - m_Players[i].position;
        toPlayer.y = 0;
        return toPlayer.magnitude;
    }

    void EnableWalk(bool isWalking)
    {
        m_Agent.isStopped = !isWalking;
        m_Controller.Walk(isWalking);
    }

}
