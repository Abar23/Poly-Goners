using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{

    [SerializeField] private Animator m_Animator;

    [SerializeField] private Damageable m_Damageable;

    public void OnHit()
    {
        int health = m_Damageable.GetHealth();
        if (health > 0)
        {
            m_Animator.SetBool("Damaged", true);
        }
    }

    public void OnDead()
    {
        m_Animator.SetBool("IsDead", true);
    }
    
}
