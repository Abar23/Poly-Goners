using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPortal : MonoBehaviour
{

    [SerializeField] private Transform m_Destination;
    [SerializeField] private ParticleSystem m_StartEffect;
    [SerializeField] private ParticleSystem m_EndEffect;
    private float k_Delay = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(TeleportPlayer(other.gameObject));
        }
    }

    IEnumerator TeleportPlayer(GameObject player)
    {
        player.SetActive(false);
        player.transform.position = m_Destination.position;
        m_StartEffect.Play();
        yield return new WaitForSeconds(k_Delay);
        m_EndEffect.Play();
        player.SetActive(true);
    }


}
