using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickupItemLabel : MonoBehaviour
{
    private float promptActivationDistance = .75f;
    private GameObject player1;
    private GameObject player2;
    public GameObject textPanel;
    public Text inventoryFullText;
    private bool nearPlayer1;
    private bool nearPlayer2;

    void Start() {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        textPanel.SetActive(false);
        inventoryFullText.gameObject.SetActive(false);
    }

    void Update() {
        if (textPanel.transform.position.y < player1.transform.position.y) {
            textPanel.transform.position = new Vector3(textPanel.transform.position.x, player1.transform.position.y + 1f, textPanel.transform.position.z);
        }

        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 < promptActivationDistance)
        {
            textPanel.SetActive(true);

            // Check if Player 1 buys item
            if (distanceFromPlayer1 <= promptActivationDistance)
            {
                nearPlayer1 = true;     
            }

            // Check if Player 2 buys item
            else if (distanceFromPlayer2 <= promptActivationDistance)
            {
                nearPlayer2 = true;
            }
        }

        else
        {
            nearPlayer1 = false;
            nearPlayer2 = false;
            textPanel.SetActive(false);
            inventoryFullText.gameObject.SetActive(false);
        }
    }

    public void ShowInventoryFullText() {
        inventoryFullText.gameObject.SetActive(true);
    }

    public bool ItemIsNearPlayer1() {
        return nearPlayer1;
    }

    public bool ItemIsNearPlayer2() {
        return nearPlayer2;
    }
}
