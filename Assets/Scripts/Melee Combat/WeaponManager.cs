﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject sword;
    private GameObject axe;
    private GameObject dagger;
    private GameObject spear;
    private Dictionary<string, GameObject> weaponPickups = new Dictionary<string, GameObject>();
    private GameObject currentWeapon;
    private Dictionary<GameObject, string> primaryAttackAnimations = new Dictionary<GameObject, string>();

    void Start()
    {
        // child indices must correspond to order that weapons appear as children under Weapons prefab
        sword = this.gameObject.transform.GetChild(0).gameObject;
        weaponPickups.Add("Sword Pickup", sword);
        primaryAttackAnimations.Add(sword, "PrimarySwordTrigger");

        axe = this.gameObject.transform.GetChild(1).gameObject;
        weaponPickups.Add("Axe Pickup", axe);
        primaryAttackAnimations.Add(axe, "PrimaryAxeTrigger");

        dagger = this.gameObject.transform.GetChild(2).gameObject;
        weaponPickups.Add("Dagger Pickup", dagger);
        primaryAttackAnimations.Add(dagger, "PrimaryDaggerTrigger");

        spear = this.gameObject.transform.GetChild(3).gameObject;
        weaponPickups.Add("Spear Pickup", spear);
        primaryAttackAnimations.Add(spear, "PrimarySpearTrigger");
    }

    public void EquipWeapon(string weaponPickupType)
    {
        if (weaponPickups[weaponPickupType] != null)
        {
            UnequipCurrentWeapon();
            weaponPickups[weaponPickupType].SetActive(true);
            currentWeapon = weaponPickups[weaponPickupType];
        }
    }

    public void UnequipWeapon(string weaponPickupType)
    {
        if (weaponPickups[weaponPickupType] != null)
        {
            weaponPickups[weaponPickupType].SetActive(false);
            currentWeapon = null;
        }
    }

    void UnequipCurrentWeapon()
    {
        if (currentWeapon != null) 
        {
            currentWeapon.SetActive(false);
            currentWeapon = null;
        }
            
    }

    public string GetPrimaryAttackAnimationTrigger()
    {
        return primaryAttackAnimations[currentWeapon];
    }
}
