using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(SkeletonAnimatorController))]
public class RangedSkeleton : MonoBehaviour
{

    [SerializeField] private TargetScanner m_Scanner;
    private List<Player> m_Players;
    [SerializeField] private float m_AttackDelay = 3.0f;
    [SerializeField] private float m_AttackRange = 10.0f;
    [SerializeField] private float m_FireDelay;

    private NavMeshAgent m_Agent;
    private SkeletonAnimatorController m_Controller;
    private const float k_ScanInterval = 0.5f;
    private const int k_RotateInterval = 10;
    private int followingIndex;
    private MagicBox m_MagicBox;
    

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<SkeletonAnimatorController>();
        m_Players = new List<Player>();

        GameObject player = PlayerManager.GetInstance().GetPlayerOneGameObject();

        m_Players.Add(PlayerManager.GetInstance().GetPlayerOneGameObject().GetComponent<Player>());
        m_Players.Add(PlayerManager.GetInstance().GetPlayerTwoGameObject().GetComponent<Player>());

        RoomController room = gameObject.GetComponentInParent<RoomController>();
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if (damageable != null && room != null)
        {
            room.RegisterEnemy();
            damageable.OnDeath.AddListener(room.RemoveEnemy);
        }
        m_MagicBox = gameObject.GetComponentInChildren<MagicBox>();

        StartCoroutine(ScanForPlayer());
    }

    IEnumerator ScanForPlayer()
    {
        bool foundPlayer = false;
        while (!foundPlayer)
        {
            for (int i = 0; i < m_Players.Count; i++)
            {
                if (m_Scanner.Detect(transform, m_Players[i].transform))
                {
                    followingIndex = i;
                    StartCoroutine(ReachForPlayer());
                    foundPlayer = true;
                    break;
                }
            }
            yield return new WaitForSeconds(k_ScanInterval);
        }
    }

    IEnumerator ReachForPlayer(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        RefinePlayerIndex();
        if (followingIndex != -1)
        {
            m_Agent.SetDestination(m_Players[followingIndex].transform.position);
            EnableWalk(true);
            while (true)
            {
                m_Agent.SetDestination(m_Players[followingIndex].transform.position);
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
        else
        {
            StartCoroutine(ScanForPlayer());
        }

    }

    IEnumerator AimPlayer()
    {
        float elpase = 0f;
        Vector3 direction = (m_Players[followingIndex].transform.position - transform.position).normalized;
        Quaternion quaternion = Quaternion.LookRotation(direction);
        Quaternion originalRotation = transform.rotation;
        for (int i = 0; i < k_RotateInterval; i++)
        {
            elpase += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(originalRotation, quaternion, (i + 1) / (float)k_RotateInterval);
            yield return new WaitForFixedUpdate();
        }
        m_Controller.RangedAttack();
        StartCoroutine(FireMagicAfterFrames(0, m_FireDelay));
        StartCoroutine("ReachForPlayer", Mathf.Max(m_AttackDelay, 0));
    }

    float CalculateDistanceToPlayer(int i)
    {
        Vector3 toPlayer = transform.position - m_Players[i].transform.position;
        toPlayer.y = 0;
        return toPlayer.magnitude;
    }

    void RefinePlayerIndex()
    {
        if (m_Players[followingIndex].IsDead())
        {
            //int new_index = -1;
            //if (followingIndex == 0 && !m_Players[1].IsDead() && m_Players[1].isActiveAndEnabled)
            //{
            //    new_index = 1;
            //}
            //else if (followingIndex == 1 && !m_Players[0].IsDead())
            //{
            //    new_index = 0;
            //}
            //followingIndex = new_index;
            followingIndex = -1;
        }
    }

    public void EnableWalk(bool isWalking)
    {
        m_Agent.isStopped = !isWalking;
        m_Controller.Walk(isWalking);
    }

    IEnumerator FireMagicAfterFrames(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        m_MagicBox.FireMagic(index);
    }

}
