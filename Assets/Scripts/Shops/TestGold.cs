using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<Inventory>().IncreaseGold(10000);
        }
    }
}
