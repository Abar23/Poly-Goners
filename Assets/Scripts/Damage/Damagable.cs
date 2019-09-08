using UnityEngine;
using UnityEngine.UI;

public class Damagable : MonoBehaviour
{

    public DamagableConfig Config;
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

        if (damager.Config.Alignment != Config.Alignment)
        {
            TakeDamage(damager.Config);
        }
    }

    void TakeDamage(DamagerConfig config)
    {
        health -= config.Damage;
        if (health <= 0)
        {
            Destroy(this);
        }
    }

}
