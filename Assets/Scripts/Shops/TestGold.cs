using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameObject playerOne = PlayerManager.GetInstance().GetPlayerOneGameObject();
        GameObject playerTwo = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        if (other.gameObject == playerOne)
        {
            playerOne.GetComponent<Inventory>().IncreaseGold(10000);
        }
        else if(other.gameObject == playerTwo)
        {
            playerTwo.GetComponent<Inventory>().IncreaseGold(10000);
        }
    }
}
