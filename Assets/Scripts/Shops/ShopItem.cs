﻿using System.Collections;
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
    public Text itemName;
    float promptActivationDistance = .75f;
    public int price;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GetComponentInParent<ShopManager>().player1;
        player2 = GetComponentInParent<ShopManager>().player2;
        buyPrompt.SetActive(false);
        itemName.enabled = false;
        price = 500;
    }

    void Update()
    {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);


        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 <= promptActivationDistance)
        {
            buyPrompt.SetActive(true);
            itemName.enabled = true;

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
            itemName.enabled = false;
        }
    }

    void BuyItem(GameObject player)
    {
        player.GetComponent<Inventory>().DecreaseGold(price);
        
        // TODO: add to player's inventory

        Destroy(this.gameObject);
    }
}
