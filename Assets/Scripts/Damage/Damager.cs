using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{

    public delegate void Hit();

    public DamagerConfig Config;

    [Header("Events")]
    public UnityEvent OnHit;

    void OnTriggerEnter(Collider other)
    {
        if (OnHit != null)
        {
            OnHit.Invoke();
        }
    }

}
