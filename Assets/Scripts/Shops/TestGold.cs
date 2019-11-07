using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{

    [SerializeField] private AudioSource m_SFX;

    void OnTriggerEnter(Collider other)
    {
        GameObject playerOne = PlayerManager.GetInstance().GetPlayerOneGameObject();
        GameObject playerTwo = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        if (other.gameObject == playerOne)
        {
            playerOne.GetComponent<Inventory>().IncreaseGold(10000);
            m_SFX.Play();
        }
        else if(other.gameObject == playerTwo)
        {
            playerTwo.GetComponent<Inventory>().IncreaseGold(10000);
            m_SFX.Play();
        }
    }
}
