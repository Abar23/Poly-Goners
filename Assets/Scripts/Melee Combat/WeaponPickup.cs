using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private WeaponManager weaponManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Inventory inv = other.GetComponent<Inventory>();
            if (!inv.IsMeleeFull())
            {
                weaponManager = other.gameObject.GetComponentInChildren<WeaponManager>();
                if (weaponManager != null)
                {
                    inv.AddMeleeWeapon(weaponManager.weaponPickups[this.gameObject.name].GetComponent<Weapon>(), this.gameObject);
                    weaponManager.EquipWeapon(this.gameObject.name);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
