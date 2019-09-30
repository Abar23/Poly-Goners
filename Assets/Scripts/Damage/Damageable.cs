using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{

    public DamageableConfig Config;

    [SerializeField] public UnityEvent OnHit;

    [SerializeField] public UnityEvent OnDeath;

    [SerializeField] private Slider HealthBar;

    private int health;
    private bool isDead = false;

    void Awake()
    {
        health = Config.StartingHealth;
    }

    void Update()
    {
        HealthBar.value = health / (float)Config.MaxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public void IncreaseHealth(int number)
    {
        health += number;
    }

    public void RevivePlayer()
    {
        health = Config.MaxHealth / 4;
    }

    void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager == null)
            return;

        if ((int)damager.Alignment + (int)Config.Alignment > 0x1
                && damager.Alignment != Config.Alignment)
        {
            TakeDamage(damager.Config);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Damager damager = collision.collider.GetComponent<Damager>();
        if (damager == null)
            return;

        if ((int)damager.Alignment + (int)Config.Alignment > 0x1
                && damager.Alignment != Config.Alignment)
        {
            TakeDamage(damager.Config);
        }
    }

    void TakeDamage(DamagerConfig config)
    {
        if (health <= 0)
            return;
        if (config is OneTimeDamagerConfig)
        {
            health -= config.Damage;
            TriggerEvent(OnHit);
            CheckHealth();
        }
        else
        {
            StartCoroutine("TakeContinuousDamage", config);
        }
    }

    IEnumerator TakeContinuousDamage(ContinuousDamagerConfig config)
    {
        for (int i = 0; i < config.DamageIteration; i++)
        {
            health -= Mathf.Max(0, config.Damage - i * config.DamageDecay);
            CheckHealth();
            yield return new WaitForSeconds(config.DamageInterval);
        }
    }

    void CheckHealth()
    {
        if (health <= 0 && !isDead)
        {
            TriggerEvent(OnDeath);
            isDead = true;
        }
    }

    void TriggerEvent(UnityEvent uEvent)
    {
        if (uEvent != null)
        {
            uEvent.Invoke();
        }
    }

    public void ScheduleDestroy(float time)
    {
        Invoke("DestoryThis", time);
    }

    void DestoryThis()
    {
        Destroy(gameObject);
    }
}
