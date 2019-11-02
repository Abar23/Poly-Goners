using UnityEngine;
using UnityEngine.UI;

public class MagicPickup : MonoBehaviour
{
    private MagicBox magicBox;
    private float pickupDistance = .75f;
    private GameObject player1;
    private GameObject player2;

    void Start() {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();
    }

    void Update() {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= pickupDistance || distanceFromPlayer2 < pickupDistance)
        {
            // Check if Player 1 picks up item
            if (distanceFromPlayer1 <= pickupDistance)
            {
                if (player1.GetComponent<Player>().CheckUseButtonPress()) {
                    if (!player1.GetComponent<Inventory>().IsMagicFull()) {
                        EquipMagic(player1);
                    }
                }       
            }

            // Check if Player 2 picks up item
            else if (distanceFromPlayer2 <= pickupDistance)
            {
                if (player2.GetComponent<Player>().CheckUseButtonPress()) {
                    if (!player2.GetComponent<Inventory>().IsMagicFull()) {
                        EquipMagic(player2);
                    }
                }  
            }
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
                this.transform.root.gameObject.SetActive(false);
            }
        }
    }
}