using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{

    public DamageableConfig Config;
    public Slider HealthBar;
    int health;

    void Start()
    {
        health = Config.StartingHealth;
    }

    void Update()
    {
        HealthBar.value = health / (float)Config.MaxHealth;
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
            Destroy(gameObject);
        }
    }

}
