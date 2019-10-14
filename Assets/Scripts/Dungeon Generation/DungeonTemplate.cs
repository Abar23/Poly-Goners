using System.Collections.Generic;
using UnityEngine;

public class DungeonTemplate : MonoBehaviour
{
    public float tileDimension;

    public DungeonRoom GoalRoom;
    public List<DungeonRoom> StartRooms;
    public List<DungeonRoom> TopEntranceRooms;
    public List<DungeonRoom> BottomEntranceRooms;
    public List<DungeonRoom> LeftEntranceRooms;
    public List<DungeonRoom> RightEntranceRooms;
    public List<DungeonRoom> DeadEndRooms;
    public List<DungeonRoom> Shops;
}
