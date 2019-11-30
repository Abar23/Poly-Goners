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
    private float pickupDistance = .75f;
    private GameObject createdPickup;
    private GameObject promptObject;
    
    void Start() {
        isActive = false;
        pickupObject = transform.GetChild(0).gameObject;
        displayObject = transform.GetChild(1).gameObject;
        promptObject = transform.GetChild(2).gameObject;
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        if (PlayerPrefs.HasKey(pickupObject.name))
            isActive = true;
    }

    void Update() {
        if (!isActive) {
            displayObject.SetActive(false);
            isActive = ItemUnlockManager.instance.CheckIfItemIsActive(pickupObject.name);
        }

        if (isActive) {
            displayObject.SetActive(true);

            float distanceFromPlayer1 = Vector3.Distance(promptObject.transform.position, player1.transform.position);
            float distanceFromPlayer2 = Vector3.Distance(promptObject.transform.position, player2.transform.position);

            if (distanceFromPlayer1 <= pickupDistance || distanceFromPlayer2 < pickupDistance) {
                if (distanceFromPlayer1 <= pickupDistance) {
                    if (createdPickup == null) {
                        CreateNewPickup();
                    } 

                    else {
                        if (player1.GetComponent<Player>().CheckUseButtonPress()) {
                            createdPickup.GetComponentInChildren<MeshRenderer>().enabled = true;
                        }
                    }
                }

                else if (distanceFromPlayer2 <= pickupDistance) {
                    if (createdPickup == null) {
                        CreateNewPickup();
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

    private void CreateNewPickup() {
        createdPickup = Instantiate(pickupObject, promptObject.transform.position, Quaternion.identity);
        createdPickup.GetComponentInChildren<PickupItemLabel>().enabled = false;
        createdPickup.name = pickupObject.name;
        createdPickup.GetComponentInChildren<MeshRenderer>().enabled = false;
        createdPickup.SetActive(true);
        createdPickup.GetComponentInChildren<ParticleSystem>().Stop();
        createdPickup.GetComponentInChildren<ParticleSystem>().Clear();
        createdPickup.GetComponentInChildren<PickupItemLabel>().enabled = true;
    }
}

