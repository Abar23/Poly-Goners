using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnerConfig m_Config;

    private Transform[] spawnPoints;

    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        for (int i = 1; i < spawnPoints.Length; i++)
        {
            foreach (EnemySpawnerConfig.EnemyEntry entry in m_Config.EnemyList)
            {
                if (Random.Range(0f, 1f) < entry.SpawnChance)
                {
                    GameObject newEnemy = GameObject.Instantiate(entry.Enemy, transform);
                    newEnemy.transform.position = spawnPoints[i].position;
                    newEnemy.transform.rotation = spawnPoints[i].rotation;
                    break;
                }
            }
        }
    }
}
