using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SkeletonBase : MonoBehaviour
{

    [SerializeField] private Transform m_Target;
    private NavMeshAgent m_MeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        m_MeshAgent = GetComponent<NavMeshAgent>();
        if (m_Target != null)
        {
            m_MeshAgent.destination = m_Target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
