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
        foreach (DropableItem item in DropList)
        {
            if (UnityEngine.Random.Range(0f, 1f) < item.SpawnChance)
            {
                GameObject newObj = Instantiate(item.Object, transform.position, Quaternion.identity);
            }
        }
    }
}
