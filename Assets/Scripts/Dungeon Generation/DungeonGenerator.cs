using System.Collections.Generic;
using UnityEngine;
using static DungeonRoom;
using UnityEngine.Events;

public class DungeonGenerator : MonoBehaviour
{
    [Range(5, 40)]
    public int minNumberOfRooms;
    [Range(6, 40)]
    public int maxNumberOfRooms;

    private int numberOfRoomsToGenerate;

    public List<DungeonTemplate> dungeonTemplates;
    private DungeonTemplate chosenTemplate;
    [SerializeField] private UnityEvent OnGenerationFinished;

    private DungeonNode dungeonTree;
    private DungeonLookUpTable lookUpTable;
    private IDungeonGenerationState dungeonGenerationState;

    // Limit the size of the lookup table. We will never create a 400 room dungeon.
    private const int lookUpTableDimensions = 20;

    void Start()
    {
        /*---- Initial Data Setup ----*/

        // Get number of rooms to generate based upon the number of completions
        DungeonCompletionTracker tracker = DungeonCompletionTracker.GetInstance();
        int numberOfCompletions = tracker.GetNumberOfCompletedDungeons();
        int numberOfCompeletionsToMaxSize = tracker.numberOfCompletionsToMaxDungeonSize;
        this.numberOfRoomsToGenerate = mapRange(numberOfCompletions, 0, numberOfCompeletionsToMaxSize, this.minNumberOfRooms, this.maxNumberOfRooms);

        // Choose random template from the list of templates given to the dungeon generator
        this.chosenTemplate = this.dungeonTemplates[Random.Range(0, this.dungeonTemplates.Count)];

        // Init look up table for dungeon generation step
        this.lookUpTable = new DungeonLookUpTable(lookUpTableDimensions);
        
        // Set dungeon tree root to the starting room
        int centerTilePosition = (lookUpTableDimensions - 1) / 2;

        // Create inital node from the start room list
        this.dungeonTree = AddNewRoom(this.chosenTemplate.StartRooms,
            new Vector2Int(centerTilePosition, centerTilePosition),
            null,
            0, 0,
            this.numberOfRoomsToGenerate);
        
        // Fill look up table position with starting room position
        this.lookUpTable.fillPosition(this.dungeonTree.lookUpPosition);

        // Set inital state of the dungeon generator to the creation state
        this.dungeonGenerationState = new DungeonCreationState(this.chosenTemplate, this.lookUpTable, this.dungeonTree, this.numberOfRoomsToGenerate - 1);
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

            // Set the new dungeon room to the proper position and rotation
            if(parentNode != null)
            {
                // Get the parent node position
                Vector3 parentNodePosition = parentNode.GetPosition();

                newRoom.SetPosition(new Vector3(parentNodePosition.x + roomXPositionOffset, 0.0f, parentNodePosition.z + roomYPositionOffset));
            }
            else
            {
                newRoom.SetPosition(new Vector3(roomXPositionOffset, 0.0f, roomYPositionOffset));
            }

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

    private int mapRange(float s, float a1, float a2, float b1, float b2)
    {
        return (int)(b1 + (s - a1) * (b2 - b1) / (a2 - a1));
    }
}
