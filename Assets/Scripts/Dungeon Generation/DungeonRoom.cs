using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    public enum RoomRotationAngle
    {
        DO_NOT_ROTATE = 0,
        TURN_LEFT = 90,
        TURN_RIGHT = 270,
        TURN_AROUND = 180
    }

    [SerializeField]
    public GameObject prefab;
    [Range(0.0f, 1.0f)]
    public float spawnChance;
    [SerializeField]
    public RoomRotationAngle roomRotation;

    public DungeonRoom(GameObject prefab, RoomRotationAngle rotation)
    {
        this.prefab = prefab;
        this.roomRotation = rotation;
    }

    public void SetPosition(Vector3 position)
    {
        this.prefab.transform.position = position;
    }

    public void RotateRoom()
    {
        this.prefab.transform.rotation = Quaternion.AngleAxis((float)this.roomRotation + this.prefab.transform.rotation.eulerAngles.y, Vector3.up);
    }

    public void SetRotation(RoomRotationAngle angle)
    {
        this.prefab.transform.rotation = Quaternion.AngleAxis((float)angle, Vector3.up);
    }

    public List<Transform> GetDooorways()
    {
        return this.prefab.GetComponent<DungeonDoorways>().DungeonDoorwayPositions;
    }

    public List<Transform> GetScrambledDoorways()
    {
        List<Transform> doorways = this.GetDooorways();

        int numberOfDoorways = doorways.Count;
        while (numberOfDoorways > 0)
        {
            int randomPositionToSwap = Random.Range(0, numberOfDoorways);
            Transform tempTransform = doorways[randomPositionToSwap];
            doorways[randomPositionToSwap] = doorways[numberOfDoorways - 1];
            doorways[numberOfDoorways - 1] = tempTransform;
            numberOfDoorways--;
        }

        return doorways;
    }
}
