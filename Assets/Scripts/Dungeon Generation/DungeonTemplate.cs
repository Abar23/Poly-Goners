using System.Collections.Generic;
using UnityEngine;

public class DungeonTemplate : MonoBehaviour
{
    public float tileDimension;

    public List<DungeonRoom> goalRooms;
    public List<DungeonRoom> startRooms;
    public List<DungeonRoom> internalRooms;
    public List<DungeonRoom> deadEndRooms;
    public List<DungeonRoom> shops;
}
