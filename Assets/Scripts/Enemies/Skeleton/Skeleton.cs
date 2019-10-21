﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(SkeletonAnimatorController))]
public class Skeleton : MonoBehaviour
{

    [SerializeField] private TargetScanner m_Scanner;
    private List<Player> m_Players;
    [SerializeField] private float m_AttackDelay = 3.0f;

    private NavMeshAgent m_Agent;
    private SkeletonAnimatorController m_Controller;
    private const float k_ScanInterval = 0.5f;
    private int followingIndex;
    

    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<SkeletonAnimatorController>();
        m_Players = new List<Player>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ob in objects)
        {
            m_Players.Add(ob.GetComponent<Player>());
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
    }

    void RefinePlayerIndex()
    {
        if (m_Players[followingIndex].IsDead())
        {
            int new_index = -1;
            if (followingIndex == 0 && !m_Players[1].IsDead() && m_Players[1].isActiveAndEnabled)
            {
                new_index = 1;
            }
            else if (followingIndex == 1 && !m_Players[0].IsDead())
            {
                new_index = 0;
            }
            followingIndex = new_index;
        }
    }

    float CalculateDistanceToPlayer(int i)
    {
        Vector3 toPlayer = transform.position - m_Players[i].transform.position;
        toPlayer.y = 0;
        return toPlayer.magnitude;
    }

    public void EnableWalk(bool isWalking)
    {
        m_Agent.isStopped = !isWalking;
        m_Controller.Walk(isWalking);
    }

}
