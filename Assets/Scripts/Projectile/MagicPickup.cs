﻿using UnityEngine;
using UnityEngine.UI;

public class MagicPickup : MonoBehaviour
{
    private MagicBox magicBox;
    private float pickupDistance = .75f;
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

        if (distanceFromPlayer1 <= pickupDistance || distanceFromPlayer2 < pickupDistance)
        {
            // Check if Player 1 picks up item
            if (distanceFromPlayer1 <= pickupDistance)
            {
                if (player1.GetComponent<Player>().CheckUseButtonPress() && this.GetComponent<PickupItemLabel>().IsClosestPickup(player1)) 
                {
                    if (!player1.GetComponent<Inventory>().IsMagicFull()) 
                    {
                        EquipMagic(player1);
                    }
                    else 
                    {
                        this.GetComponent<PickupItemLabel>().ShowInventoryFullText();
                    }
                }       
            }

            // Check if Player 2 picks up item
            else if (distanceFromPlayer2 <= pickupDistance && this.GetComponent<PickupItemLabel>().IsClosestPickup(player2))
            {
                if (player2.GetComponent<Player>().CheckUseButtonPress()) 
                {
                    if (!player2.GetComponent<Inventory>().IsMagicFull()) 
                    {
                        EquipMagic(player2);
                    }
                    else 
                    {
                        this.GetComponent<PickupItemLabel>().ShowInventoryFullText();
                    }
                }  
            }
        }
    }

    public void EquipMagic(GameObject player) 
    {
        Inventory inv = player.GetComponent<Inventory>();
        if (!inv.IsMagicFull())
        {
            magicBox = player.GetComponentInChildren<MagicBox>();
            if (magicBox != null)
            {
                GameObject newDrop = Instantiate(this.gameObject, player.transform.position + player.transform.forward, Quaternion.identity);
                newDrop.name = this.gameObject.name;

                inv.AddMagicAbility(this.gameObject.name, newDrop);
                newDrop.transform.root.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }
}