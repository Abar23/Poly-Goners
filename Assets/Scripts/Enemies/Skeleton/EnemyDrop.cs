using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{

    [Serializable]
    public struct DropableItem
    {
        public GameObject Object;
        [Range(0, 1)]
        public float SpawnChance;
    }

    [SerializeField] private List<DropableItem> DropList;

    public void DropItem()
    {
        float spawnChance = UnityEngine.Random.Range(0f, 1f);
        foreach (DropableItem item in DropList)
        {
            if (spawnChance < item.SpawnChance)
            {
                GameObject newObj = Instantiate(item.Object, transform.position, Quaternion.identity);
                newObj.name = newObj.name.Substring(0, newObj.name.Length - 7);
                break;
            }
        }
    }
}
