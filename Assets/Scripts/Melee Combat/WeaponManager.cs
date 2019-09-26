using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject sword;
    private GameObject axe;
    private Dictionary<string, GameObject> weaponPickups = new Dictionary<string, GameObject>();
    private GameObject currentWeapon;

    void Start()
    {
        // child indices must correspond to order that weapons appear as children under Weapons prefab
        sword = this.gameObject.transform.GetChild(0).gameObject;
        weaponPickups.Add("Sword Pickup", sword);

        axe = this.gameObject.transform.GetChild(1).gameObject;
        weaponPickups.Add("Axe Pickup", axe);
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
            currentWeapon.SetActive(false);
    }
}
