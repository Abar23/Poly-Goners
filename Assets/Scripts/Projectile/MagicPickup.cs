using UnityEngine;
using UnityEngine.UI;

public class MagicPickup : MonoBehaviour
{
    private MagicBox magicBox;
    private float promptActivationDistance = .75f;
    private GameObject player1;
    private GameObject player2;
    public GameObject textPanel;
    public Text inventoryFullText; 

    void Start() {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        textPanel.SetActive(false);
        inventoryFullText.gameObject.SetActive(false);
    }

    void Update() {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 < promptActivationDistance)
        {
            textPanel.SetActive(true);

            // Check if Player 1 buys item
            if (distanceFromPlayer1 <= promptActivationDistance)
            {
                if (player1.GetComponent<Player>().CheckUseButtonPress()) {
                    if (!player1.GetComponent<Inventory>().IsMagicFull()) {
                        EquipMagic(player1);
                    }
                    else {
                        inventoryFullText.gameObject.SetActive(true);
                    }
                }       
            }

            // Check if Player 2 buys item
            else if (distanceFromPlayer2 <= promptActivationDistance)
            {
                if (player2.GetComponent<Player>().CheckUseButtonPress()) {
                    if (!player2.GetComponent<Inventory>().IsMagicFull()) {
                        EquipMagic(player2);
                    }
                    else {
                        inventoryFullText.gameObject.SetActive(true);
                    }
                }  
            }
        }

        else
        {
            textPanel.SetActive(false);
            inventoryFullText.gameObject.SetActive(false);
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