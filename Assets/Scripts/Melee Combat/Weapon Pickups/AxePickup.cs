using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxePickup : MonoBehaviour
{
    private WeaponManager weaponManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            weaponManager = other.gameObject.GetComponentInChildren<WeaponManager>();
            if (weaponManager != null) 
            {
                weaponManager.EquipAxe();
                Destroy(this.gameObject);
            }
        }
    }
}
