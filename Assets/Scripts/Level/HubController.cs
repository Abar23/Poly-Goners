using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubController : MonoBehaviour
{

    [SerializeField] private Animator m_Animator;

    void Awake()
    {
        m_Animator.SetTrigger("Enter");
    }

}
