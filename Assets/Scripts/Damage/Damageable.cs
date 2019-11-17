﻿using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<Damager, float> damagerCountDown;

    void Awake()
    {
        health = Config.StartingHealth;
        damageText = HealthBar.gameObject.GetComponentInChildren<Text>();
        damagerCountDown = new Dictionary<Damager, float>();
    }

    void Update()
    {
        float elapse = Time.deltaTime;

        HealthBar.value = health / (float)Config.MaxHealth;

        if (damageText != null)
        {
            if (elapsedTime < timeToShow)
            {
                elapsedTime += elapse;
                damageText.enabled = true;
            }
            else
            {
                damageText.enabled = false;
                damageAmount = 0;
            }
        }

        Dictionary<Damager, float> copy = new Dictionary<Damager, float>();
        // countinuous effective damager update
        foreach (Damager damager in damagerCountDown.Keys)
        {
            float countDown = damagerCountDown[damager] - elapse;
            if (countDown > 0)
            {
                copy.Add(damager, countDown);
            }
        }
        damagerCountDown = copy;
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
        if (this.gameObject.layer == 9 && GetComponent<MeshBlink>().IsInvincible())
            return;

        Damager damager = other.GetComponent<Damager>();
        if (damager == null)
            return;

        if (damager.Config is ContinuousEffectiveDamagerConfig)
            return;

        if ((int)damager.Alignment + (int)Config.Alignment > 0x1
                && damager.Alignment != Config.Alignment)
        {
            TakeDamage(damager.Config, damager.GetMultiplier(), damager);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (this.gameObject.layer == 9 && GetComponent<MeshBlink>().IsInvincible())
            return;
        Damager damager = other.GetComponent<Damager>();
        if (damager == null)
            return;
        if (damager.Config is ContinuousEffectiveDamagerConfig)
        {
            TakeCEDamage(damager, damager.GetMultiplier());
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (this.gameObject.layer == 9 && GetComponent<MeshBlink>().IsInvincible())
            return;

        Damager damager = collision.collider.GetComponent<Damager>();
        if (damager == null)
            return;

        if ((int)damager.Alignment + (int)Config.Alignment > 0x1
                && damager.Alignment != Config.Alignment)
        {
            TakeDamage(damager.Config, damager.GetMultiplier(), damager);
        }
    }

    void TakeDamage(DamagerConfig config, float multiplier, Damager damager)
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

    void TakeCEDamage(Damager damager, float multiplier)
    {
        if (damagerCountDown.ContainsKey(damager))
            return;
        health -= (int)(damager.Config.Damage * multiplier);
        if (damageText != null)
        {
            damageAmount += (int)(damager.Config.Damage * multiplier);
            elapsedTime = 0f;
            damageText.text = "-" + damageAmount.ToString();
        }
        damagerCountDown.Add(damager, ((ContinuousEffectiveDamagerConfig)(damager.Config)).DamageInterval);
        CheckHealth();
    }

    public void TakeFallDamage()
    {
        if (!GetComponent<MeshBlink>().IsInvincible())
        {
            if (health <= 0)
                return;
            health -= Config.MaxHealth / 10;
            TriggerEvent(OnHit);
            CheckHealth();
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
