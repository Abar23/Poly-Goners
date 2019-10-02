using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponAnimationConfig")]
public class WeaponAnimationConfig : ScriptableObject
{
    [SerializeField] AnimationClip primaryAttackAnimation;

    public AnimationClip GetPrimaryAttackAnimation()
    {
        return primaryAttackAnimation;
    }
}
