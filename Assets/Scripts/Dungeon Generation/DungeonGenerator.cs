using System.Collections.Generic;
using UnityEngine;
using static DungeonRoom;
using UnityEngine.Events;

public class DungeonGenerator : MonoBehaviour
{
    public int numberOfRooms; // Public room number constraints. Temporary, need to create dynamic way to do this
    public DungeonTemplate template;
    [SerializeField] private UnityEvent OnGenerationFinished;

    private DungeonNode dungeonTree;
    private DungeonLookUpTable lookUpTable;
    private IDungeonGenerationState dungeonGenerationState;

    // Limit the size of the lookup table. We will never create a 100 room dungeon.
    private const int lookUpTableDimensions = 20;

    void Start()
    {
        /*---- Initial Data Setup ----*/
        
        // Init look up table for dungeon generation step
        this.lookUpTable = new DungeonLookUpTable(lookUpTableDimensions);
        
        // Randomly choose starting room
        DungeonRoom startingRoom = this.template.StartRooms[Random.Range(0, this.template.StartRooms.Count)];
        
        // Set dungeon tree root to the starting room
        int centerTilePosition = (lookUpTableDimensions - 1) / 2;
        this.dungeonTree = new DungeonNode(startingRoom, new Vector2Int(centerTilePosition, centerTilePosition));
        
        // Fill look up tabel position with starting room position
        this.lookUpTable.fillPosition(this.dungeonTree.lookUpPosition);
        
        // Create starting room gameobject
        Instantiate(startingRoom.prefab, Vector3.zero, Quaternion.AngleAxis((float)startingRoom.roomRotation, Vector3.up));

        // Set inital state of the dungeon generator to the creation state
        this.dungeonGenerationState = new DungeonCreationState(this.template, this.lookUpTable, this.dungeonTree, this.numberOfRooms - 1);
    }

    void Update()
    {
        IDungeonGenerationState newState = this.dungeonGenerationState.Update();

        if(newState != null)
        {
            this.dungeonGenerationState = newState;
        }

        if (this.dungeonGenerationState is FinishedDungeonState)
        {
            if (OnGenerationFinished != null)
            {
                OnGenerationFinished.Invoke();
                OnGenerationFinished = null;
            }
        }
    }

    public static DungeonNode AddNewRoom(List<DungeonRoom> roomList,
        Vector2Int lookUpTablePosition,
        DungeonNode parentNode,
        float roomXPositionOffset,
        float roomYPositionOffset,
        int numberOfRoomsLeftToPlace)
    {
        DungeonNode newNode = null;

        // Get spawn chance value
        float spawnProbability = Random.Range(0.0f, 1.0f);

        // Get random room from list of rooms
        List<DungeonRoom> validRoomsToChoose = new List<DungeonRoom>();
        foreach(DungeonRoom potentialRoom in roomList)
        {
            if(potentialRoom.GetDooorways().Count - 1 <= numberOfRoomsLeftToPlace)
            {
                if(spawnProbability >= 1.0f - potentialRoom.spawnChance)
                {
                    validRoomsToChoose.Add(potentialRoom);
                }
            }
        }

        if(validRoomsToChoose.Count > 0)
        {
            DungeonRoom room = validRoomsToChoose[Random.Range(0, validRoomsToChoose.Count)];

            // Create copy of the randomly chosen room
            DungeonRoom newRoom = new DungeonRoom(Instantiate(room.prefab), room.roomRotation);

            // Get the parent node position
            Vector3 parentNodePosition = parentNode.GetPosition();

            // Set the new dungeon room to the proper position and rotation
            newRoom.SetPosition(new Vector3(parentNodePosition.x + roomXPositionOffset, 0.0f, parentNodePosition.z + roomYPositionOffset));
            newRoom.RotateRoom();

            // Add the new dungeon node to the dungeon tree
            newNode = new DungeonNode(newRoom, lookUpTablePosition, parentNode);
        }

        return newNode;
    }

    public static DungeonNode AddDeadEnd(List<DungeonRoom> deadEndRoomsList,
        Vector2Int lookUpTablePosition,
        DungeonNode parentNode,
        float roomXPositionOffset,
        float roomYPositionOffset,
        RoomRotationAngle angleToRotateDeadEndRoom)
    {
        // Get spawn chance value
        float spawnProbability = Random.Range(0.0f, 1.0f);

        // Get random room from list of rooms
        List<DungeonRoom> validDeadEndRoomsToChoose = new List<DungeonRoom>();
        foreach (DungeonRoom potentialDeadEnd in deadEndRoomsList)
        {
            if (spawnProbability >= 1.0f - potentialDeadEnd.spawnChance)
            {
                validDeadEndRoomsToChoose.Add(potentialDeadEnd);
            }
        }

        // Get random room from list of rooms
        DungeonRoom room = validDeadEndRoomsToChoose[Random.Range(0, validDeadEndRoomsToChoose.Count)];

        // Create copy of the randomly chosen room
        DungeonRoom newRoom = new DungeonRoom(Instantiate(room.prefab), angleToRotateDeadEndRoom);

        // Get the parent node position
        Vector3 parentNodePosition = parentNode.GetPosition();

        // Set the new dungeon room to the proper position and rotation
        newRoom.SetPosition(new Vector3(parentNodePosition.x + roomXPositionOffset, 0.0f, parentNodePosition.z + roomYPositionOffset));
        newRoom.RotateRoom();

        // Add the new dungeon node to the dungeon tree
        DungeonNode newNode = new DungeonNode(newRoom, lookUpTablePosition, parentNode);

        return newNode;
    }
}
