using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLabel : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    public GameObject textPanel;
    float promptActivationDistance = .75f;

    void Start() {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();
        textPanel.SetActive(false);
    }

    void Update() {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 < promptActivationDistance) {
            textPanel.SetActive(true);
        } else {
            textPanel.SetActive(false);
        }
    }
}
