﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Damageable))]
public class DragonAnimatorController : MonoBehaviour
{
    [Serializable]
    public struct AttackAnim
    {
        public AnimationClip Animation;
        [Range(0f, 1f)]
        public float TriggerChance;
    }

    [SerializeField] private List<AttackAnim> m_MeleeAttackAnimations;
    [SerializeField] private List<AttackAnim> m_RangedAttackAnimations;

    private AnimatorOverrideController animatorOverrideController;

    private Animator m_Animator;

    private Damageable m_Damageable;

    private bool landed = true;

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
            m_Animator.SetTrigger("Damaged");
        }
    }

    public void OnDead()
    {
        m_Animator.SetBool("IsDead", false);
    }

    public void MeleeAttack(bool isAttacking)
    {
        foreach (AttackAnim meleeAttack in m_MeleeAttackAnimations)
        {
            if (UnityEngine.Random.Range(0f, 1f) < meleeAttack.TriggerChance)
            {
                animatorOverrideController["PRIMARY_ATTACK"] = meleeAttack.Animation;
                break;
            }
        }
        m_Animator.SetTrigger("MeleeAttack");
    }

    public void RangedAttack()
    {
        foreach (AttackAnim meleeAttack in m_RangedAttackAnimations)
        {
            if (UnityEngine.Random.Range(0f, 1f) < meleeAttack.TriggerChance)
            {
                animatorOverrideController["PRIMARY_ATTACK"] = meleeAttack.Animation;
                break;
            }
        }
        m_Animator.SetTrigger("RangedAttack");
    }

    public void TakeOff()
    {
        if (landed)
        {
            m_Animator.SetTrigger("Take Off");
            landed = false;
        }
            
    }

    public void Land()
    {
        if (!landed)
        {
            m_Animator.SetTrigger("Land");
            landed = true;
        }
    }

}
