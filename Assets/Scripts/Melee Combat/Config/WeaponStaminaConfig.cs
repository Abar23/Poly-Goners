using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponStaminaConfig")]
public class WeaponStaminaConfig : ScriptableObject
{
    [SerializeField] float attackStamina;

    public float GetPrimaryAttackStamina()
    {
        return attackStamina;
    }
}
