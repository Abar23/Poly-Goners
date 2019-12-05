using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(DragonAnimatorController), typeof(Damageable))]
public class Dragon : MonoBehaviour, IEnemy
{

    private List<Player> m_Players;
    [Header("Game Play Ground Level 1")]
    [SerializeField] private float m_AttackDelayLevel1 = 4.0f;
    [Range(0f, 1f)]
    [SerializeField] private float m_MeleeAttackChanceLevel1;

    [Header("Game Play Fly Level 2")]
    [SerializeField] private float m_AttackDelayLevel2 = 4.0f;

    [Header("Game Play Ground Level 3")]
    [SerializeField] private float m_AttackDelayLevel3 = 4.0f;
    [Range(0f, 1f)]
    [SerializeField] private float m_MeleeAttackChanceLevel3;

    [Header("Motion Control")]
    [SerializeField] private float m_FlyHeight = 4f;
    [SerializeField] private bool m_AutoAim = false;
    [SerializeField] private ParticleSystem m_DustEffect;
    [SerializeField] private Transform m_Destination;
    [SerializeField] private float m_ShowOffTime;
    [SerializeField] private float m_ShowOffHeight;

    // Motion Control
    private const float k_TakeOffDelay = 0.5f;
    private const float k_TakeOffTime = 2f;
    private const float k_ShowOffTakeOffTime = 3f;
    private const float k_ShowTakeOffTime = 1f;
    private const float k_LandTime = 2f;
    private float initial_height;
    private bool isLanded = true;

    // Game Play
    private NavMeshAgent m_Agent;
    private DragonAnimatorController m_Controller;
    private int followingIndex = -1;
    private int levelTwoHealth;
    private int levelThreeHealth;
    private const float levelTwoPotion = 0.6f;
    private const float levelThreePotion = 0.2f;
    private MagicBox magicBox;
    private Damageable damageable;
    private float k_AutoAimDrag = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<DragonAnimatorController>();
        this.damageable = GetComponent<Damageable>();
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

        magicBox = GetComponentInChildren<MagicBox>();

        initial_height = transform.position.y;

        levelTwoHealth = (int) (levelTwoPotion * this.damageable.GetMaxHealth());
        levelThreeHealth = (int)(levelThreePotion * this.damageable.GetMaxHealth());
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowUp());
        StartCoroutine(AutoAim());
    }

    IEnumerator AutoAim()
    {
        while (true)
        {
            if (m_AutoAim && followingIndex != -1)
            {
                Vector3 direction = (m_Players[followingIndex].transform.position - transform.position).normalized;
                Quaternion quaternion = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, k_AutoAimDrag);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ShowUp()
    {
        yield return new WaitForSeconds(1f);
        m_Agent.enabled = false;
        m_Controller.TakeOff();
        isLanded = false;
        StartCoroutine(ChangeHeight(transform.position.y, m_ShowOffHeight, k_ShowTakeOffTime));
        yield return new WaitForSeconds(k_ShowOffTakeOffTime);
        StartCoroutine(FlyToDest());
        yield return new WaitForSeconds(m_ShowOffTime);
        m_Controller.Land();
        isLanded = true;
        m_Agent.enabled = true;
        StartCoroutine(LandedAttackLvl1(2f));
    }

    IEnumerator FlyToDest()
    {
        float elapse = 0f;
        Vector3 orignalPos = transform.position;
        while (true)
        {
            transform.position = Vector3.Lerp(orignalPos, m_Destination.position, elapse / m_ShowOffTime);
            elapse += Time.deltaTime;
            if (elapse > m_ShowOffTime)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        transform.position = m_Destination.position;
    }

    IEnumerator LandedAttackLvl1(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        if (damageable.GetHealth() < levelTwoHealth)
        {
            StartCoroutine(TransformToFly());
        }
        else
        {
            RefinePlayerIndex();
            if (followingIndex != -1)
            {
                if (Random.Range(0f, 1f) < m_MeleeAttackChanceLevel1)
                {
                    // Melee Attack
                    m_Agent.SetDestination(m_Players[followingIndex].transform.position);
                    EnableWalk(true);
                    StartCoroutine(MeleeAttack("LandedAttackLvl1", m_AttackDelayLevel1));
                }
                else
                {
                    // Ranged Attack
                    m_Controller.RangedAttack();
                    StartCoroutine(LandedAttackLvl1(m_AttackDelayLevel1));
                }
            }
        }
    }

    IEnumerator FlyAttackLvl2(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        if (damageable.GetHealth() < levelThreeHealth)
        {
            StartCoroutine(TransformToLand());
        }
        else
        {
            RefinePlayerIndex();
            if (followingIndex != -1)
            {
                m_Controller.FlyAttack();
                StartCoroutine(LandedAttackLvl1(m_AttackDelayLevel2));
            }
        }
    }

    IEnumerator LandedAttackLvl3(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        
        RefinePlayerIndex();
        if (followingIndex != -1)
        {
            if (Random.Range(0f, 1f) < m_MeleeAttackChanceLevel1)
            {
                // Melee Attack
                m_Agent.SetDestination(m_Players[followingIndex].transform.position);
                EnableWalk(true);
                StartCoroutine(MeleeAttack("LandedAttackLvl3", m_AttackDelayLevel3));
            }
            else
            {
                // Ranged Attack
                m_Controller.RangedAttack();
                StartCoroutine(LandedAttackLvl3(m_AttackDelayLevel3));
            }
        }
    }

    IEnumerator MeleeAttack(string attackName, float attackDelay)
    {
        while (true)
        {
            m_Agent.SetDestination(m_Players[followingIndex].transform.position);
            if (CalculateDistanceToPlayer(followingIndex) <= m_Agent.stoppingDistance)
            {
                EnableWalk(false);
                m_Controller.MeleeAttack();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(attackName, Mathf.Max(attackDelay, 0));
    }

    IEnumerator TransformToFly()
    {
        m_Agent.enabled = false;
        m_Controller.TakeOff();
        yield return new WaitForSeconds(k_TakeOffDelay);
        StartCoroutine(ChangeHeight(transform.position.y, m_FlyHeight, k_TakeOffTime));
        isLanded = false;
        StartCoroutine(FlyAttackLvl2(m_AttackDelayLevel2));
    }

    IEnumerator TransformToLand()
    {
        m_Controller.Land();
        StartCoroutine(ChangeHeight(transform.position.y, initial_height, k_LandTime));
        isLanded = true;
        yield return new WaitForSeconds(m_AttackDelayLevel3);
        m_Agent.enabled = true;
        StartCoroutine(LandedAttackLvl3());
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

    IEnumerator ChangeHeight(float init_pos, float final_pos, float time)
    {
        float elapse = 0f;
        while (true)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(init_pos, final_pos, elapse / time), transform.position.z);
            elapse += Time.deltaTime;
            if (elapse > time)
                break;
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, final_pos, transform.position.z);
    }

    private void RefinePlayerIndex()
    {
        if (PlayerManager.GetInstance().GetNumberOfActivePlayers() == 1)
        {
            followingIndex = 0;
        }
        else
        {
            followingIndex = Random.Range(0f, 1f) < 0.5f ? 0 : 1;
            if (m_Players[followingIndex].IsDead())
                followingIndex = 1 - followingIndex;
        }
        if (m_Players[followingIndex].IsDead())
            followingIndex = -1;
    }

    public void FireBallMagic()
    {
        Vector3 direction = (m_Players[followingIndex].transform.position - magicBox.transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(direction);
        magicBox.gameObject.transform.rotation = q;
        magicBox.gameObject.transform.Rotate(Vector3.up * -90f, Space.Self);
        magicBox.FireMagic(0);
    }

    public void StartSpreadMagic()
    {
        magicBox.FireMagic(1);
        Invoke("StopSpreadMagic", 2.8f);
    }

    public void StopSpreadMagic()
    {
        magicBox.StopMagic(1);
    }

    public void PlayDustEffect()
    {
        m_DustEffect.gameObject.transform.position = new Vector3(transform.position.x, m_DustEffect.gameObject.transform.position.y, transform.position.z);
        m_DustEffect.Play();
    }

}
