﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    public enum RoomRotationAngle
    {
        DO_NOT_ROTATE = 0,
        TURN_CLOCKWISE = 90,
        TURN_COUNTER_CLOCKWISE = 270,
        TURN_AROUND = 180
    }

    [SerializeField]
    public GameObject prefab;
    [Range(0.0f, 1.0f)]
    public float spawnChance;

    private float roomRotation;

    public DungeonRoom(GameObject prefab, float rotation)
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
        this.prefab.transform.rotation = Quaternion.AngleAxis(this.roomRotation + this.prefab.transform.rotation.eulerAngles.y, Vector3.up);
    }

    public void OverrideRotation(Quaternion rotation)
    {
        this.prefab.transform.rotation = rotation;
    }

    public List<Transform> GetDooorways()
    {
        return this.prefab.GetComponent<DungeonDoorways>().DungeonDoorwayPositions;
    }

    public List<Transform> GetScrambledDoorways()
    {
        Quaternion originalRotation = this.prefab.transform.rotation;

        RotateRoom();

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

        OverrideRotation(originalRotation);

        return doorways;
    }
}
