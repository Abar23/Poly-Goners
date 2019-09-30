using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory), typeof(Damageable))]
public class Collector : MonoBehaviour
{
    
    void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        Collectable collectable = obj.GetComponent<Collectable>();
        if (collectable == null) return;
        if (collectable.CollectableType == Collectable.Type.Coin)
        {
            Inventory inventory = GetComponent<Inventory>();
            inventory.IncreaseGold(1);
        } else if (collectable.CollectableType == Collectable.Type.Potion) {
            PotionConfig config = collectable.GetComponent<Collectable>().Config;
            if (config is HealthPotionConfig)
            {
                Damageable damageable = GetComponent<Damageable>();
                StartCoroutine(HealthPotionEffect(damageable, (HealthPotionConfig)config));
            }
            else if (config is MagicPotionConfig)
            {
                
            }
            else if (config is StrengthPotionConfig)
            {

            }
        }
    }

    IEnumerator HealthPotionEffect(Damageable damageable, HealthPotionConfig config)
    {
        float elapse = 0f;
        while (elapse < config.EffectiveTime)
        {
            damageable.IncreaseHealth(config.Amount);
            yield return new WaitForSeconds(config.Interval);
            elapse += Time.deltaTime;
        }
    }

}
