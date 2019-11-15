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

    public List<KeyValuePair<DungeonRoom, float>> TopRooms { get; private set; }
    public List<KeyValuePair<DungeonRoom, float>> BottomRooms { get; private set; }
    public List<KeyValuePair<DungeonRoom, float>> LeftRooms { get; private set; }
    public List<KeyValuePair<DungeonRoom, float>> RightRooms { get; private set; }
    public List<KeyValuePair<DungeonRoom, float>> StartRooms { get; private set; }
    public List<KeyValuePair<DungeonRoom, float>> Shops { get; private set; }
    public List<KeyValuePair<DungeonRoom, float>> DeadEnds { get; private set; }
    public List<KeyValuePair<DungeonRoom, float>> GoalRooms { get; private set; }

    [SerializeField] private UnityEvent OnGenerationFinished;
    [SerializeField] private Animator m_Animator;

    private DungeonNode dungeonTree;
    private DungeonLookUpTable lookUpTable;
    private IDungeonGenerationState dungeonGenerationState;

    // Limit the size of the lookup table. We will never create a 400 room dungeon.
    private const int lookUpTableDimensions = 20;

    void Awake()
    {
        if (m_Animator != null)
        {
            m_Animator.SetTrigger("Load");
        }
    }

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

        ConstructRoomLists();

        // Init look up table for dungeon generation step
        this.lookUpTable = new DungeonLookUpTable(lookUpTableDimensions);
        
        // Set dungeon tree root to the starting room
        int centerTilePosition = (lookUpTableDimensions - 1) / 2;

        // Create inital node from the start room list
        this.dungeonTree = AddNewRoom(this.StartRooms,
            new Vector2Int(centerTilePosition, centerTilePosition),
            null,
            0, 0,
            this.numberOfRoomsToGenerate,
            this.chosenTemplate.tileDimension);
        
        // Fill look up table position with starting room position
        this.lookUpTable.fillPosition(this.dungeonTree.lookUpPosition);

        // Set inital state of the dungeon generator to the creation state
        this.dungeonGenerationState = new DungeonCreationState(this.chosenTemplate.tileDimension, this.lookUpTable, this.dungeonTree, this.numberOfRoomsToGenerate - 1, this);
    }

    void Update()
    {
        IDungeonGenerationState newState = this.dungeonGenerationState.Update();

        if (newState != null)
        {
            this.dungeonGenerationState = newState;
        }

        if (this.dungeonGenerationState is FinishedDungeonState)
        {
            if (OnGenerationFinished != null)
            {
                OnGenerationFinished.Invoke();
                OnGenerationFinished = null;
                if (m_Animator != null)
                {
                    m_Animator.SetTrigger("Enter");
                }
                Destroy(gameObject);
            }
        }
    }

    public static DungeonNode AddNewRoom(List<KeyValuePair<DungeonRoom, float>> roomList,
        Vector2Int lookUpTablePosition,
        DungeonNode parentNode,
        float roomXPositionOffset,
        float roomYPositionOffset,
        int numberOfRoomsLeftToPlace,
        float tileDimensions)
    {
        DungeonNode newNode = null;

        // Get spawn chance value
        float spawnProbability = Random.Range(0.0f, 1.0f);

        // Get random room from list of rooms
        List<KeyValuePair<DungeonRoom, float>> validRoomsToChoose = new List<KeyValuePair<DungeonRoom, float>>();
        foreach(KeyValuePair<DungeonRoom, float> potentialRoom in roomList)
        {
            if(potentialRoom.Key.GetDooorways().Count - 1 <= numberOfRoomsLeftToPlace)
            {
                if(spawnProbability >= 1.0f - potentialRoom.Key.spawnChance)
                {
                    validRoomsToChoose.Add(potentialRoom);
                }
            }
        }

        if(validRoomsToChoose.Count > 0)
        {
            KeyValuePair<DungeonRoom, float> room = validRoomsToChoose[Random.Range(0, validRoomsToChoose.Count)];

            // Create copy of the randomly chosen room
            DungeonRoom newRoom = new DungeonRoom(Instantiate(room.Key.prefab), room.Value);

            // Add occluder to newly created room
            TileOccluder occluder = newRoom.prefab.AddComponent<TileOccluder>();
            occluder.tileDimentions = tileDimensions;

            // Set the new dungeon room to the proper position and rotation
            if (parentNode != null)
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

    public static DungeonNode AddDeadEnd(List<KeyValuePair<DungeonRoom, float>> deadEndRoomsList,
        Vector2Int lookUpTablePosition,
        DungeonNode parentNode,
        float roomXPositionOffset,
        float roomYPositionOffset,
        RoomRotationAngle angleOffset,
        float tileDimensions)
    {
        // Get spawn chance value
        float spawnProbability = Random.Range(0.0f, 1.0f);

        // Get random room from list of rooms
        List<KeyValuePair<DungeonRoom, float>> validDeadEndRoomsToChoose = new List<KeyValuePair<DungeonRoom, float>>();
        foreach (KeyValuePair<DungeonRoom, float> potentialDeadEnd in deadEndRoomsList)
        {
            if (spawnProbability >= 1.0f - potentialDeadEnd.Key.spawnChance)
            {
                validDeadEndRoomsToChoose.Add(potentialDeadEnd);
            }
        }

        // Get random room from list of rooms
        KeyValuePair<DungeonRoom, float> room = validDeadEndRoomsToChoose[Random.Range(0, validDeadEndRoomsToChoose.Count)];

        // Create copy of the randomly chosen room
        DungeonRoom newRoom = new DungeonRoom(Instantiate(room.Key.prefab), room.Value + (float)angleOffset);

        // Add occluder to newly created room
        TileOccluder occluder = newRoom.prefab.AddComponent<TileOccluder>();
        occluder.tileDimentions = tileDimensions;

        // Get the parent node position
        Vector3 parentNodePosition = parentNode.GetPosition();

        // Set the new dungeon room to the proper position and rotation
        newRoom.SetPosition(new Vector3(parentNodePosition.x + roomXPositionOffset, 0.0f, parentNodePosition.z + roomYPositionOffset));
        newRoom.RotateRoom();

        // Add the new dungeon node to the dungeon tree
        DungeonNode newNode = new DungeonNode(newRoom, lookUpTablePosition, parentNode);

        return newNode;
    }

    private void ConstructRoomLists()
    {
        this.TopRooms = new List<KeyValuePair<DungeonRoom, float>>();
        this.BottomRooms = new List<KeyValuePair<DungeonRoom, float>>();
        this.LeftRooms = new List<KeyValuePair<DungeonRoom, float>>();
        this.RightRooms = new List<KeyValuePair<DungeonRoom, float>>();
        this.StartRooms = new List<KeyValuePair<DungeonRoom, float>>();
        this.GoalRooms = new List<KeyValuePair<DungeonRoom, float>>();
        this.Shops = new List<KeyValuePair<DungeonRoom, float>>();
        this.DeadEnds = new List<KeyValuePair<DungeonRoom, float>>();

        ConstructRooms(this.TopRooms, Vector3.back, this.chosenTemplate.internalRooms);
        ConstructRooms(this.BottomRooms, Vector3.forward, this.chosenTemplate.internalRooms);
        ConstructRooms(this.LeftRooms, Vector3.right, this.chosenTemplate.internalRooms);
        ConstructRooms(this.RightRooms, Vector3.left, this.chosenTemplate.internalRooms);
        ConstructRooms(this.StartRooms, Vector3.back, this.chosenTemplate.startRooms);
        ConstructRooms(this.GoalRooms, Vector3.back, this.chosenTemplate.goalRooms);
        ConstructRooms(this.DeadEnds, Vector3.back, this.chosenTemplate.deadEndRooms);
        ConstructRooms(this.Shops, Vector3.back, this.chosenTemplate.shops);
    }

    private void ConstructRooms(List<KeyValuePair<DungeonRoom, float>> rooms, Vector3 connectionDirection, List<DungeonRoom> roomList)
    {
        foreach (DungeonRoom room in roomList)
        {
            List<Transform> dungeonRoomDoorways = room.GetDooorways();
            foreach (Transform doorway in dungeonRoomDoorways)
            {
                float angleBetweenVectors = Vector3.SignedAngle(doorway.forward, connectionDirection, Vector3.up);
                rooms.Add(new KeyValuePair<DungeonRoom, float>(room, angleBetweenVectors));
            }
        }
    }

    private int mapRange(float s, float a1, float a2, float b1, float b2)
    {
        return (int)(b1 + (s - a1) * (b2 - b1) / (a2 - a1));
    }
}
