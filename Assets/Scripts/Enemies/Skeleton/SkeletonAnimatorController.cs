using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Damageable))]
public class SkeletonAnimatorController : MonoBehaviour
{

    [Serializable]
    public struct MeleeAttackAnim
    {
        public AnimationClip Animation;
        [Range(0f, 1f)]
        public float TriggerChance;
    }

    [SerializeField] private List<MeleeAttackAnim> m_AttackAnimations;

    private AnimatorOverrideController animatorOverrideController;

    private Animator m_Animator;

    private Damageable m_Damageable;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Damageable = GetComponent<Damageable>();
        animatorOverrideController = new AnimatorOverrideController(m_Animator.runtimeAnimatorController);
        m_Animator.runtimeAnimatorController = animatorOverrideController;
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
        foreach (MeleeAttackAnim meleeAttack in m_AttackAnimations)
        {
            if (UnityEngine.Random.Range(0f, 1f) < meleeAttack.TriggerChance)
            {
                animatorOverrideController["PRIMARY_ATTACK"] = meleeAttack.Animation;
                break;
            }
        }
        m_Animator.SetBool("IsAttacking", isAttacking);
    }

    public void Walk(bool isWalking)
    {
        m_Animator.SetBool("IsWalking", isWalking);
    }

    public void MeleeAttack()
    {
        foreach (MeleeAttackAnim meleeAttack in m_AttackAnimations)
        {
            if (UnityEngine.Random.Range(0f, 1f) < meleeAttack.TriggerChance)
            {
                animatorOverrideController["PRIMARY_ATTACK"] = meleeAttack.Animation;
                break;
            }
        }
        m_Animator.SetTrigger("Melee Attack");
    }

    public void RangedAttack()
    {
        m_Animator.SetTrigger("Ranged Attack");
    }
    
}
