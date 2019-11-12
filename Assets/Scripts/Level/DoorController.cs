using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DoorController : MonoBehaviour
{
    [SerializeField] private List<Door> m_Doors;
    [SerializeField] private float m_Delay;
    [SerializeField] private UnityEvent m_OnEnterRoom;
    private List<Player> playersInside;
    private Collider m_Collider;
    private bool triggered = false;

    void Awake()
    {
        playersInside = new List<Player>();
        m_Collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!triggered && playersInside.Count > 0)
        {
            foreach (Door door in m_Doors)
            {
                StartCoroutine(CloseDoor(door, m_Delay));
            }
            StartCoroutine(OnEnterRoom(m_Delay));
            m_Collider.enabled = false;
            triggered = true;
        }
    }

    IEnumerator OnEnterRoom(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (m_OnEnterRoom != null)
        {
            m_OnEnterRoom.Invoke();
        }
    }

    public void OpenAllDoors()
    {
        foreach (Door door in m_Doors)
        {
            StartCoroutine(OpenDoor(door, m_Delay));
        }
    }

    IEnumerator CloseDoor(Door door, float delay)
    {
        yield return new WaitForSeconds(delay);
        door.Close();
    }

    IEnumerator OpenDoor(Door door, float delay)
    {
        yield return new WaitForSeconds(delay);
        door.Open();
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
            }
        }
    }
}
