using UnityEngine;

public class MagicPickup : MonoBehaviour
{
    private MagicBox magicBox;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Inventory inv = other.GetComponent<Inventory>();
            if (!inv.IsMagicFull())
            {
                magicBox = other.gameObject.GetComponentInChildren<MagicBox>();
                if (magicBox != null)
                {
                    inv.AddMagicAbility(this.gameObject.name, this.gameObject);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
