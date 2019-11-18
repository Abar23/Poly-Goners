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
    private GameObject buyPrompt;
    //public Text itemName;
    private Text priceText;
    private GameObject textPanel;
    private GameObject notEnoughGoldText;
    private Vector3 textPanelPosition;
    float promptActivationDistance = .75f;
    public int price;

    private Text nameText;
    private Text descText;

    void Start()
    {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();
        textPanel = transform.Find("ShopItemCanvas").transform.GetChild(0).gameObject;
        textPanel.SetActive(false);
        buyPrompt = textPanel.transform.GetChild(4).gameObject;
        buyPrompt.SetActive(false);
        //itemName.enabled = false;

        priceText = textPanel.transform.GetChild(2).gameObject.GetComponent<Text>();
        priceText.text = "Price: " + price.ToString();
        textPanelPosition = textPanel.transform.position;
        notEnoughGoldText = textPanel.transform.GetChild(6).gameObject;
        notEnoughGoldText.SetActive(true);

        Canvas[] cs = GetComponentsInChildren<Canvas>();
        foreach (Canvas c in cs)
        {
            if (c.name == "ItemPickupCanvas")
            {
                GameObject otherTextPanel = c.transform.GetChild(0).gameObject;
                nameText = textPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
                descText = textPanel.transform.GetChild(1).gameObject.GetComponent<Text>();
                nameText.text = otherTextPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text;
                descText.text = "\n" + otherTextPanel.transform.GetChild(6).gameObject.GetComponent<Text>().text;
            }
        }
    }

    void Update()
    {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);


        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 < promptActivationDistance)
        {
            buyPrompt.SetActive(true);
            textPanel.SetActive(true);

            // Check if Player 1 buys item
            if (distanceFromPlayer1 <= promptActivationDistance)
            {
                if (player1.GetComponent<Player>().CheckUseButtonPress()) {
                    if (player1.GetComponent<Inventory>().GetGold() >= price) {
                        BuyItem(player1);
                    }
                    else {
                        notEnoughGoldText.SetActive(true);
                    }
                }       
            }

            // Check if Player 2 buys item
            else if (distanceFromPlayer2 <= promptActivationDistance)
            {
                if (player2.GetComponent<Player>().CheckUseButtonPress()) {
                    if (player2.GetComponent<Inventory>().GetGold() >= price) {
                        BuyItem(player2);
                    }
                    else {
                        notEnoughGoldText.SetActive(true);
                    }
                }  
            }
        }

        else
        {
            buyPrompt.SetActive(false);
            textPanel.SetActive(false);
            notEnoughGoldText.SetActive(false);
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
            GameObject weaponPickup = Instantiate(this.gameObject.transform.GetChild(0).gameObject) as GameObject;
            // get rid of (clone) from end of name so weapon is properly added to inventory map
            weaponPickup.name = weaponPickup.name.Substring(0, weaponPickup.name.Length - 7);
            weaponPickup.GetComponent<WeaponPickup>().EquipWeapon(player);
            ItemUnlockManager.instance.AddPickupToUnlocks(weaponPickup.name);

            player.GetComponent<Inventory>().DecreaseGold(price);
            this.gameObject.SetActive(false);
        }

        // Buy potion if player is not already holding a potion
        else if (this.gameObject.tag == "ShopPotion" && !inv.HasPotion())
        {
            GameObject potionPickup = Instantiate(this.gameObject.transform.GetChild(0).gameObject) as GameObject;
            inv.AddPotionToInventory(potionPickup.GetComponent<Collectable>());
            
            player.GetComponent<Inventory>().DecreaseGold(price);
            this.gameObject.SetActive(false);
        }

        // Buy magic if player is not holding two magic types already
        else if (this.gameObject.tag == "ShopMagicBook" && !inv.IsMagicFull())
        {
            GameObject magicPickup = Instantiate(this.gameObject.transform.GetChild(1).gameObject) as GameObject;
            // get rid of (clone) from end of name so magic is properly added to inventory map
            magicPickup.name = magicPickup.name.Substring(0, magicPickup.name.Length - 7);
            magicPickup.GetComponentInChildren<MagicPickup>().EquipMagic(player);
            ItemUnlockManager.instance.AddPickupToUnlocks(magicPickup.name);
            player.GetComponent<Inventory>().DecreaseGold(price);
            this.gameObject.SetActive(false);
        }
    }
}
