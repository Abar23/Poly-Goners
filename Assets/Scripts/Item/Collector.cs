using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory), typeof(Damageable))]
public class Collector : MonoBehaviour
{

    private Damageable damageable;
    private Inventory inventory;
    private MagicBox magicBox;
    private bool isStrengthed = false;

    void Awake()
    {
        damageable = GetComponent<Damageable>();
        inventory = GetComponent<Inventory>();
        magicBox = GetComponentInChildren<MagicBox>();
    }
    
    void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        Collectable collectable = obj.GetComponent<Collectable>();
        if (collectable == null) return;
        if (collectable.CollectableType == Collectable.Type.Coin)
        {
            inventory.IncreaseGold(1);
        }
        else if (collectable.CollectableType == Collectable.Type.Potion)
        {
            PotionConfig config = collectable.GetComponent<Collectable>().Config;
            if (config is HealthPotionConfig)
            {
                StartCoroutine(HealthPotionEffect(damageable, (HealthPotionConfig)config));
            }
            else if (config is MagicPotionConfig)
            {
                StartCoroutine(MagicPotionEffect(magicBox, (MagicPotionConfig)config));
            }
            else if (config is StrengthPotionConfig)
            {
                if (isStrengthed) return;
                Damager[] damagers = GetComponentsInChildren<Damager>();
                foreach (Damager damager in damagers)
                {
                    damager.SetMultiplier(((StrengthPotionConfig)config).Multiplier);
                }
                Invoke("ResetMultiplier", config.EffectiveTime);
            }
        }
    }

    IEnumerator HealthPotionEffect(Damageable damageable, HealthPotionConfig config)
    {
        float elapse = 0f;
        while (elapse < config.EffectiveTime)
        {
            damageable.IncreaseHealth(config.Amount);
            elapse += config.Interval;
            yield return new WaitForSeconds(config.Interval);
        }
    }

    IEnumerator MagicPotionEffect(MagicBox magicBox, MagicPotionConfig config)
    {
        float elapse = 0f;
        while (elapse < config.EffectiveTime)
        {
            magicBox.IncreaseMagicPoint(config.Amount);
            elapse += config.Interval;
            yield return new WaitForSeconds(config.Interval);
        }
    }

    void ResetMultiplier()
    {
        Damager[] damagers = GetComponentsInChildren<Damager>();
        foreach (Damager damager in damagers)
        {
            damager.SetMultiplier(1f);
        }
    }

}
