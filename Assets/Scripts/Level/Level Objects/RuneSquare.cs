using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSquare : MonoBehaviour
{

    [SerializeField] private RunePillar m_Pillar;
    [SerializeField] private AudioSource m_Audio;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        if (!m_Pillar.Active)
        {
            m_Audio.Play();
        }
        m_Pillar.SetActive();
        
    }
}
