using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private WeaponManager weaponManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EquipWeapon(other.gameObject);
        }
    }

    public void EquipWeapon(GameObject player)
    {
        Inventory inv = player.GetComponent<Inventory>();
        if (!inv.IsMeleeFull())
        {
            weaponManager = player.gameObject.GetComponentInChildren<WeaponManager>();
            if (weaponManager != null)
            {
                inv.AddMeleeWeapon(weaponManager.weaponPickups[this.gameObject.name].GetComponent<Weapon>(), this.gameObject);
                weaponManager.EquipWeapon(this.gameObject.name);
                this.gameObject.SetActive(false);
            }
        }
    }
}
