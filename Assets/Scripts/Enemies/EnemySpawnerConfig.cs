using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerConfig", menuName = "ScriptableObjects/EnemySpawnerConfig", order = 1)]
public class EnemySpawnerConfig : ScriptableObject
{

    [Serializable]
    public struct EnemyEntry
    {
        public GameObject Enemy;
        [Range(0f, 1f)]
        public float SpawnChance;
    }

    public List<EnemyEntry> EnemyList;
}
