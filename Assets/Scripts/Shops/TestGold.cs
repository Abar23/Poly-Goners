using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        this.player = PlayerManager.GetInstance().GetPlayerOneGameObject();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<Inventory>().IncreaseGold(10000);
        }
    }
}
