using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{

    public delegate void Hit();

    public DamagerConfig Config;

    [Header("Events")]
    public UnityEvent OnCauseDamage;
    public UnityEvent OnHitAlly;
    public UnityEvent OnHitDummy;

    void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable == null)
        {
            TriggerEvent(OnHitDummy);
        }
        else if ((int)damageable.Config.Alignment + (int)Config.Alignment <= 1)
        {
            TriggerEvent(OnHitAlly);
        }
        else
        {
            TriggerEvent(OnCauseDamage);
        }
    }

    void TriggerEvent(UnityEvent uEvent)
    {
        if (uEvent != null)
        {
            uEvent.Invoke();
        }
    }

}
