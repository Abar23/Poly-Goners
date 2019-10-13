using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    float distanceFromPlayer1;
    float distanceFromPlayer2;
    private GameObject player1;
    private GameObject player2;
    public GameObject buyPrompt;
    float promptActivationDistance = .75f;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GetComponentInParent<ShopManager>().player1;
        player2 = GetComponentInParent<ShopManager>().player2;
        buyPrompt.SetActive(false);
    }

    void Update()
    {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= promptActivationDistance)
        {
            buyPrompt.SetActive(true);
        }
        else
        {
            buyPrompt.SetActive(false);
        }
    }
}
