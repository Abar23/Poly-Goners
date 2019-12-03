using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpawner : MonoBehaviour
{

    [SerializeField] private GameObject m_SpawnPoints;
    [SerializeField] private List<GameObject> m_Objects;

    void Awake()
    {
        Transform[] transforms = m_SpawnPoints.GetComponentsInChildren<Transform>();
        List<Transform> transform_list = new List<Transform>(transforms);
        transform_list.Remove(m_SpawnPoints.transform);
        if (m_Objects.Count > transform_list.Count)
            return;
        foreach (GameObject gameObject in m_Objects)
        {
            int index = Random.Range(0, transform_list.Count);
            Transform temp = transform_list[index];
            transform_list.RemoveAt(index);
            gameObject.transform.position = temp.position;
            gameObject.transform.rotation = temp.rotation;
        }
    }
}
