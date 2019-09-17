using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{

    public DamagerConfig Config;
    public Alignment Alignment;

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
        else if ((int)damageable.Config.Alignment + (int)Alignment <= 1
            || damageable.Config.Alignment == Alignment)
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
