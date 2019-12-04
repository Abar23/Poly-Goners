﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUI : MonoBehaviour
{
    public string Header;
    public string Info;

    private float promptActivationDistance = 1.5f;
    private GameObject player1;
    private GameObject player2;
    private GameObject textPanel;
    private Text inventoryFullText;
    private Text information;
    private Text head;
    private bool nearPlayer1;
    private bool nearPlayer2;

    void Start()
    {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        textPanel = GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject;

        Text[] fields = textPanel.GetComponentsInChildren<Text>();
        foreach (Text t in fields)
        {
            if (t.name.Contains("Full"))
            {
                inventoryFullText = t;
                break;
            }
        }

        textPanel.SetActive(false);
        if (inventoryFullText != null)
            inventoryFullText.gameObject.SetActive(false);

        information = textPanel.transform.GetChild(6).gameObject.GetComponent<Text>();
        information.text = Info;

        head = textPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        head.text = Header;
    }

    void Update()
    {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 <= promptActivationDistance)
        {
            // Display menu to player 1
            if (distanceFromPlayer1 <= promptActivationDistance && IsClosestPickup(player1))
            {
                textPanel.SetActive(true);
                textPanel.transform.position = new Vector3(transform.position.x, player1.transform.position.y + 3.5f, transform.position.z);
                nearPlayer1 = true;
            }

            // Display menu to player 2
            else if (distanceFromPlayer2 <= promptActivationDistance && IsClosestPickup(player2))
            {
                textPanel.SetActive(true);
                textPanel.transform.position = new Vector3(transform.position.x, player2.transform.position.y + 3.5f, transform.position.z);
                nearPlayer2 = true;
            }

            else
            {
                textPanel.SetActive(false);
            }
        }

        else
        {
            nearPlayer1 = false;
            nearPlayer2 = false;
            textPanel.SetActive(false);
            if (inventoryFullText != null)
                inventoryFullText.gameObject.SetActive(false);
        }
    }

    public bool ItemIsNearPlayer1()
    {
        return nearPlayer1;
    }

    public bool ItemIsNearPlayer2()
    {
        return nearPlayer2;
    }

    public bool IsClosestPickup(GameObject player)
    {
        bool isClosest = false;

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");

        GameObject closestPickup = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = player.transform.position;

        foreach (GameObject obj in pickups)
        {
            Vector3 vectorDist = playerPosition - obj.transform.position;
            float magnitude = vectorDist.sqrMagnitude;
            if (magnitude <= closestDistance)
            {
                closestPickup = obj;
                closestDistance = magnitude;
            }
        }

        if (closestPickup == this.gameObject)
            isClosest = true;

        return isClosest;
    }
}