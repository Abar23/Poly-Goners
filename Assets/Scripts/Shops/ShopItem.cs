using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    float distanceFromPlayer1;
    float distanceFromPlayer2;
    private GameObject player1;
    private GameObject player2;
    public GameObject buyPrompt;
    //public Text itemName;
    private Text priceText;
    public GameObject textPanel;
    private Vector3 textPanelPosition;
    float promptActivationDistance = .75f;
    public int price;

    void Start()
    {
        player1 = GetComponentInParent<ShopManager>().player1;
        player2 = GetComponentInParent<ShopManager>().player2;
        buyPrompt.SetActive(false);
        //itemName.enabled = false;
        textPanel.SetActive(false);
        priceText = textPanel.transform.GetChild(2).gameObject.GetComponent<Text>();
        priceText.text = "Price: " + price.ToString();
        textPanelPosition = textPanel.transform.position;
    }

    void Update()
    {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);


        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 <= promptActivationDistance)
        {
            buyPrompt.SetActive(true);
            //itemName.enabled = true;
            textPanel.SetActive(true);

            // Check if Player 1 buys item
            if (distanceFromPlayer1 <= promptActivationDistance)
            {
                if (player1.GetComponent<Inventory>().GetGold() >= price && player1.GetComponent<Player>().CheckUseButtonPress())
                    BuyItem(player1);
            }

            // Check if Player 2 buys item
            else if (distanceFromPlayer2 <= promptActivationDistance)
            {
                if (player2.GetComponent<Inventory>().GetGold() >= price && player2.GetComponent<Player>().CheckUseButtonPress())
                    BuyItem(player2);
            }
        }

        else
        {
            buyPrompt.SetActive(false);
            //itemName.enabled = false;
            textPanel.SetActive(false);
        }
    }

    void LateUpdate() {
        textPanel.transform.position = textPanelPosition;
    }

    void BuyItem(GameObject player)
    {
        Inventory inv = player.GetComponent<Inventory>();
        
        // Buy melee weapon if inventory isn't full
        if (this.gameObject.tag == "ShopWeapon" && !inv.IsMeleeFull())
        {
            WeaponManager wm = player.GetComponentInChildren<WeaponManager>();
            wm.EquipWeapon(this.name + " Pickup");
            inv.AddMeleeWeapon(wm.weaponPickups[this.gameObject.name + " Pickup"].GetComponent<Weapon>(), this.gameObject);

            player.GetComponent<Inventory>().DecreaseGold(price);
            this.gameObject.SetActive(false);
        }

        // Buy potion if player is not already holding a potion
        else if (this.gameObject.tag == "ShopPotion" && !inv.HasPotion())
        {
            inv.AddPotionToInventory(this.gameObject.GetComponent<Collectable>());
            
            player.GetComponent<Inventory>().DecreaseGold(price);
            this.gameObject.SetActive(false);
        }

        // Buy magic if player is not holding two magic types already
        else if (this.gameObject.tag == "ShopMagicBook" && !inv.IsMagicFull())
        {
            GameObject magicPickup = Instantiate(this.gameObject.transform.GetChild(1).gameObject) as GameObject;
            magicPickup.GetComponent<MagicPickup>().EquipMagic(player);
            player.GetComponent<Inventory>().DecreaseGold(price);
            this.gameObject.SetActive(false);
        }
    }
}
