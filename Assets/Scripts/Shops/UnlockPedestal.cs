using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockPedestal : MonoBehaviour
{
    private bool isActive;
    private GameObject pickupObject;
    private GameObject displayObject;
    private GameObject player1;
    private GameObject player2;
    private float pickupDistance = 2.5f;
    private GameObject createdPickup;
    
    void Start() {
        isActive = false;
        pickupObject = transform.GetChild(0).gameObject;
        displayObject = transform.GetChild(1).gameObject;
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();
    }

    void Update() {
        if (!isActive) {
            displayObject.SetActive(false);
            isActive = ItemUnlockManager.instance.CheckIfItemIsActive(pickupObject.name);
        }

        if (isActive) {
            displayObject.SetActive(true);

            float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
            float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

            if (distanceFromPlayer1 <= pickupDistance || distanceFromPlayer2 < pickupDistance) {
                if (distanceFromPlayer1 <= pickupDistance) {
                    if (createdPickup == null) {
                        CreateNewPickup(player1.transform.position);
                    } 

                    else {
                        createdPickup.transform.position = player1.transform.position;
                        
                        if (player1.GetComponent<Player>().CheckUseButtonPress()) {
                            createdPickup.GetComponentInChildren<MeshRenderer>().enabled = true;
                        }
                    }
                }

                else if (distanceFromPlayer2 <= pickupDistance) {
                    if (createdPickup == null) {
                        CreateNewPickup(player2.transform.position);
                    } 

                    else {
                        createdPickup.transform.position = player2.transform.position;
                        
                        if (player2.GetComponent<Player>().CheckUseButtonPress()) {
                            createdPickup.GetComponentInChildren<MeshRenderer>().enabled = true;
                        }
                    }
                }
            }
        }
    }

    private void CreateNewPickup(Vector3 playerPosition) {
        createdPickup = Instantiate(pickupObject, playerPosition, Quaternion.identity);
        createdPickup.name = pickupObject.name;
        createdPickup.GetComponentInChildren<MeshRenderer>().enabled = false;
        createdPickup.SetActive(true);
        createdPickup.GetComponentInChildren<ParticleSystem>().Stop();
        createdPickup.GetComponentInChildren<ParticleSystem>().Clear();
    }
}

