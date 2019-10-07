using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(SkeletonAnimatorController))]
public class RangedSkeleton : MonoBehaviour
{

    [SerializeField] private TargetScanner m_Scanner;
    private List<Transform> m_Players;
    [SerializeField] private float m_AttackDelay = 3.0f;
    [SerializeField] private float m_AttackRange = 10.0f;

    private NavMeshAgent m_Agent;
    private SkeletonAnimatorController m_Controller;
    private const float k_ScanInterval = 0.5f;
    private const int k_RotateInterval = 10;
    private int followingIndex;
    

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<SkeletonAnimatorController>();
        m_Players = new List<Transform>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ob in objects)
        {
            m_Players.Add(ob.transform);
        }
        RoomController room = gameObject.GetComponentInParent<RoomController>();
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if (damageable != null && room != null)
        {
            room.RegisterEnemy();
            damageable.OnDeath.AddListener(room.RemoveEnemy);
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
            float distanceToPlayer = CalculateDistanceToPlayer(followingIndex);
            if (distanceToPlayer <= m_AttackRange)
            {
                EnableWalk(false);
                if (distanceToPlayer <= m_Agent.stoppingDistance)
                {
                    m_Controller.MeleeAttack();
                    StartCoroutine("ReachForPlayer", m_AttackDelay);
                    
                }
                else
                {
                    StartCoroutine(AimPlayer());
                }
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator AimPlayer()
    {
        float elpase = 0f;
        Vector3 direction = (m_Players[followingIndex].position - transform.position).normalized;
        Quaternion quaternion = Quaternion.LookRotation(direction);
        Quaternion originalRotation = transform.rotation;
        for (int i = 0; i < k_RotateInterval; i++)
        {
            elpase += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(originalRotation, quaternion, (i + 1) / (float)k_RotateInterval);
            yield return new WaitForFixedUpdate();
        }
        m_Controller.RangedAttack();
        StartCoroutine("ReachForPlayer", Mathf.Max(m_AttackDelay - elpase, 0));
    }

    float CalculateDistanceToPlayer(int i)
    {
        Vector3 toPlayer = transform.position - m_Players[i].position;
        toPlayer.y = 0;
        return toPlayer.magnitude;
    }

    public void EnableWalk(bool isWalking)
    {
        m_Agent.isStopped = !isWalking;
        m_Controller.Walk(isWalking);
    }

}
