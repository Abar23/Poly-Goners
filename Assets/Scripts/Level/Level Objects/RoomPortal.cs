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
        Renderer playerRenderer = player.GetComponent<CharacterBox>().GetActiveCharacterRenderer();
        playerRenderer.enabled = false;
        m_StartEffect.Play();
        yield return new WaitForSeconds(k_Delay);
        player.transform.position = new Vector3(m_Destination.position.x, m_Destination.position.y, m_Destination.position.z);
        m_EndEffect.Play();
        playerRenderer.enabled = true;
    }


}
