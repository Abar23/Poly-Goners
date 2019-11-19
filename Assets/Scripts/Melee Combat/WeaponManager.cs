using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject currentWeapon;

    public Dictionary<string, GameObject> weaponPickups { get; private set; }

    void Start()
    {
        weaponPickups = new Dictionary<string, GameObject>();

        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject newWeapon = this.gameObject.transform.GetChild(i).gameObject;
            weaponPickups.Add(newWeapon.name + " Pickup", newWeapon);
        }
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

    public WeaponConfig GetWeaponConfig()
    {
        return currentWeapon.GetComponent<Weapon>().GetConfig();
    }
}
