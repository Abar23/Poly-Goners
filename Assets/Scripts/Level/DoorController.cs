using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Door m_Door;
    private List<Player> playersInside;
    private int num_player;

    void Awake()
    {
        playersInside = new List<Player>();
    }

    void Update()
    {
        num_player = GameObject.FindGameObjectsWithTag("Player").Length;
        if (playersInside.Count == num_player)
        {
            m_Door.Open();
        }
        else
        {
            m_Door.Close();
        }
        Debug.Log("Number of Player = " + num_player);
        Debug.Log("Number of Player Inside = " + playersInside.Count);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player == null || other.isTrigger)
            {
                return;
            }
            if (!playersInside.Contains(player))
            {
                playersInside.Add(player);
                
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player == null || other.isTrigger)
            {
                return;
            }
            if (playersInside.Contains(player))
            {
                playersInside.Remove(player);
                Debug.Log(other.gameObject);
            }
        }
    }
}
