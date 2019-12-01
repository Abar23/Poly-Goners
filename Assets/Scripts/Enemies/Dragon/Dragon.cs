using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(DragonAnimatorController))]
public class Dragon : MonoBehaviour, IEnemy
{

    [SerializeField] private TargetScanner m_Scanner;
    private List<Player> m_Players;
    [SerializeField] private float m_AttackDelay = 3.0f;
    [SerializeField] private bool m_AutoAim = false;

    private NavMeshAgent m_Agent;
    private DragonAnimatorController m_Controller;
    private const float k_ScanInterval = 0.5f;
    private int followingIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<DragonAnimatorController>();
        m_Players = new List<Player>();
        m_Players.Add(PlayerManager.GetInstance().GetPlayerOneGameObject().GetComponent<Player>());
        m_Players.Add(PlayerManager.GetInstance().GetPlayerTwoGameObject().GetComponent<Player>());

        RoomController room = gameObject.GetComponentInParent<RoomController>();
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if (damageable != null && room != null)
        {
            room.RegisterEnemy(this);
            damageable.OnDeath.AddListener(delegate { room.RemoveEnemy(this); });
            gameObject.SetActive(false);
        }
        Spawn();
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowUp());
    }

    IEnumerator ShowUp()
    {
        yield return new WaitForSeconds(1f);
        m_Controller.TakeOff();
        yield return new WaitForSeconds(5f);
        m_Controller.Land();
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
    }
}
