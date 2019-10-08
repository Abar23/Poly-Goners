using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject sword;
    private GameObject axe;
    private GameObject dagger;
    private GameObject spear;
    private GameObject currentWeapon;

    public Dictionary<string, GameObject> weaponPickups { get; private set; }

    void Start()
    {
        weaponPickups = new Dictionary<string, GameObject>();

        // child indices must correspond to order that weapons appear as children under Weapons prefab
        sword = this.gameObject.transform.GetChild(0).gameObject;
        weaponPickups.Add("Sword Pickup", sword);

        axe = this.gameObject.transform.GetChild(1).gameObject;
        weaponPickups.Add("Axe Pickup", axe);

        dagger = this.gameObject.transform.GetChild(2).gameObject;
        weaponPickups.Add("Dagger Pickup", dagger);

        spear = this.gameObject.transform.GetChild(3).gameObject;
        weaponPickups.Add("Spear Pickup", spear);
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

    public void UnequipCurrentWeapon()
    {
        if (currentWeapon != null) 
        {
            currentWeapon.SetActive(false);
            currentWeapon = null;
        }
            
    }

    public WeaponAnimationConfig GetWeaponAnimationConfig()
    {
        return currentWeapon.GetComponent<Weapon>().GetAnimationConfig();
    }

    public WeaponStaminaConfig GetWeaponStaminaConfig()
    {
        return currentWeapon.GetComponent<Weapon>().GetStaminaConfig();
    }
}
