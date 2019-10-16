using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Damageable))]
public class SkeletonAnimatorController : MonoBehaviour
{

    private Animator m_Animator;

    private Damageable m_Damageable;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Damageable = GetComponent<Damageable>();
    }

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

    public void Attack(bool isAttacking)
    {
        m_Animator.SetBool("IsAttacking", isAttacking);
    }

    public void Walk(bool isWalking)
    {
        m_Animator.SetBool("IsWalking", isWalking);
    }

    public void MeleeAttack()
    {
        m_Animator.SetTrigger("Melee Attack");
    }

    public void RangedAttack()
    {
        m_Animator.SetTrigger("Ranged Attack");
    }
    
}
