using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    [SerializeField]
    public GameObject prefab;
    [SerializeField]
    public float rotation;

    public DungeonRoom(GameObject prefab, float rotation)
    {
        this.prefab = prefab;
        this.rotation = rotation;
    }

    public void SetPosition(Vector3 position)
    {
        this.prefab.transform.position = position;
    }

    public void SetRotation(float angle)
    {
        this.prefab.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    public List<Transform> GetDooorways()
    {
        return this.prefab.GetComponent<DungeonDoorways>().DungeonDoorwayPositions;
    }
}
