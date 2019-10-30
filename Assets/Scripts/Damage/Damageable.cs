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

    private Text damageText;
    private int damageAmount = 0;
    private float elapsedTime = 2f;
    private float timeToShow = 2f;

    void Awake()
    {
        health = Config.StartingHealth;
        damageText = HealthBar.gameObject.GetComponentInChildren<Text>();
    }

    void Update()
    {
        HealthBar.value = health / (float)Config.MaxHealth;

        if (damageText != null)
        {
            if (elapsedTime < timeToShow)
            {
                elapsedTime += Time.deltaTime;
                damageText.enabled = true;
            }
            else
            {
                damageText.enabled = false;
                damageAmount = 0;
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void IncreaseHealth(int number)
    {
        health = Mathf.Min(health + number, Config.MaxHealth);
    }

    public void RevivePlayer()
    {
        health = Config.MaxHealth / 4;
        this.isDead = false;
    }

    public void ResetHealthToFull() 
    {
        health = Config.StartingHealth;
        this.isDead = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();
        if (damager == null)
            return;

        if ((int)damager.Alignment + (int)Config.Alignment > 0x1
                && damager.Alignment != Config.Alignment)
        {
            TakeDamage(damager.Config, damager.GetMultiplier());
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
            TakeDamage(damager.Config, damager.GetMultiplier());
        }
    }

    void TakeDamage(DamagerConfig config, float multiplier)
    {
        if (health <= 0)
            return;
        if (config is OneTimeDamagerConfig)
        {
            health -= (int)(config.Damage * multiplier);
            if (damageText != null)
            {
                damageAmount += (int)(config.Damage * multiplier);
                elapsedTime = 0f;
                damageText.text = "-" + damageAmount.ToString();
            }
            TriggerEvent(OnHit);
            CheckHealth();
        }
        else if (config is ContinuousDamagerConfig)
        {
            StartCoroutine(TakeContinuousDamage((ContinuousDamagerConfig)config, multiplier));
        }
    }

    IEnumerator TakeContinuousDamage(ContinuousDamagerConfig config, float multiplier)
    {
        for (int i = 0; i < config.DamageIteration; i++)
        {
            health -= (int)(Mathf.Max(0, (config.Damage - i * config.DamageDecay)) * multiplier);
            if (damageText != null)
            {
                damageAmount += (int)(Mathf.Max(0, (config.Damage - i * config.DamageDecay)) * multiplier);
                elapsedTime = 0f;
                damageText.text = "-" + damageAmount.ToString();
            }
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
