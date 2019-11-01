using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponAnimationConfig")]
public class WeaponAnimationConfig : ScriptableObject
{
    [SerializeField] AnimationClip primaryAttackAnimation;
    [SerializeField] int animationLayer;

    public AnimationClip GetPrimaryAttackAnimation()
    {
        return primaryAttackAnimation;
    }

    public int GetAnimationLayer()
    {
        return animationLayer;
    }
}
