using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    [SerializeField]
    public GameObject prefab;
    [SerializeField]
    public float rotation;

    public List<Transform> GetDooorways()
    {
        return this.prefab.GetComponent<DungeonDoorways>().DungeonDoorwayPositions;
    }
}
