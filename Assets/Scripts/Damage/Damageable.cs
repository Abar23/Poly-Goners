using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{

    public DamageableConfig Config;

    [SerializeField] private UnityEvent OnHit;

    [SerializeField] private UnityEvent OnDeath;

    [SerializeField] private Slider HealthBar;

    private int health;

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
        if (health <= 0)
        {
            TriggerEvent(OnDeath);
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
