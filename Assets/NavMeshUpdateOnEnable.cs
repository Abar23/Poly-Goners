using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class NavMeshUpdateOnEnable : MonoBehaviour
{
    public Transform prefabTransform;
    public NavMeshData m_NavMeshData;
    private NavMeshDataInstance m_NavMeshInstance;

    private void Update()
    {
        this.m_NavMeshData.position = this.prefabTransform.position;
    }
    void OnEnable()
    {
        m_NavMeshInstance = NavMesh.AddNavMeshData(m_NavMeshData);
    }

    void OnDisable()
    {
        NavMesh.RemoveNavMeshData(m_NavMeshInstance);
    }
}