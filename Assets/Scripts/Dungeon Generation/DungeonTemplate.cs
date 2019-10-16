using System.Collections.Generic;
using UnityEngine;

public class DungeonTemplate : MonoBehaviour
{
    public float tileDimension;

    public List<DungeonRoom> TopEntranceRooms;
    public List<DungeonRoom> BottomEntranceRooms;
    public List<DungeonRoom> LeftEntranceRooms;
    public List<DungeonRoom> RightEntranceRooms;
    public List<DungeonRoom> StartRooms;
}
