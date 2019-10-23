using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshManager : MonoBehaviour
{
    public void BakeNavMesh()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in rooms)
        {
            room.AddComponent<NavMeshSurface>();
            NavMeshSurface surface = room.GetComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }
    }
}
