using UnityEngine;

public class MagicPickup : MonoBehaviour
{
    private MagicBox magicBox;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EquipMagic(other.gameObject);
        }
    }

    public void EquipMagic(GameObject player) {
        Inventory inv = player.GetComponent<Inventory>();
        if (!inv.IsMagicFull())
        {
            magicBox = player.GetComponentInChildren<MagicBox>();
            if (magicBox != null)
            {
                inv.AddMagicAbility(this.gameObject.name, this.gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }
}
