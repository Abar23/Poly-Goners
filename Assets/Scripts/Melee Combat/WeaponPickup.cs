using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    private WeaponManager weaponManager;
    private float promptActivationDistance = .75f;
    private GameObject player1;
    private GameObject player2;

    void Start() 
    {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();
    }

    void Update() 
    {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 < promptActivationDistance)
        {
            // Check if Player 1 equips weapon
            if (distanceFromPlayer1 <= promptActivationDistance)
            {
                if (player1.GetComponent<Player>().CheckUseButtonPress()) 
                {
                    if (!player1.GetComponent<Inventory>().IsMeleeFull()) 
                    {
                        EquipWeapon(player1);
                    }
                    else {
                        this.GetComponent<PickupItemLabel>().ShowInventoryFullText();
                    }
                }       
            }

            // Check if Player 2 equips weapon
            else if (distanceFromPlayer2 <= promptActivationDistance)
            {
                if (player2.GetComponent<Player>().CheckUseButtonPress()) 
                {
                    if (!player2.GetComponent<Inventory>().IsMeleeFull()) 
                    {
                        EquipWeapon(player2);
                    }
                }  
            }
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
