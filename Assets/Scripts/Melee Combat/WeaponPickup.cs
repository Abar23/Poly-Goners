using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private WeaponManager weaponManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            weaponManager = other.gameObject.GetComponentInChildren<WeaponManager>();
            if (weaponManager != null) 
            {
                weaponManager.EquipWeapon(this.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }
}
