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
    public Text itemName;
    float promptActivationDistance = .75f;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GetComponentInParent<ShopManager>().player1;
        player2 = GetComponentInParent<ShopManager>().player2;
        buyPrompt.SetActive(false);
        itemName.enabled = false;
    }

    void Update()
    {
        float distanceFromPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceFromPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        if (distanceFromPlayer1 <= promptActivationDistance)
        {
            buyPrompt.SetActive(true);
            itemName.enabled = true;
        }
        else
        {
            buyPrompt.SetActive(false);
            itemName.enabled = false;
        }
    }
}
