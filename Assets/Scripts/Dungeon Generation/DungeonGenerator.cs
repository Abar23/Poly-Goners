using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonTemplate template;

    private const int lookUpTableDimensions = 5;

    private DungeonNode dungeonTree;
    private DungeonLookUpTable lookUpTable;
    private Queue<DungeonNode> nodeQueue;

    private int dungeonGenerationState = 0;

    void Start()
    {
        this.lookUpTable = new DungeonLookUpTable(lookUpTableDimensions);

        // Randomly choose starting room
        DungeonRoom startingRoom = this.template.StartRooms[Random.Range(0, this.template.StartRooms.Count - 1)];
        // Set dungeon tree root to the starting room
        this.dungeonTree = new DungeonNode(startingRoom, new Vector2Int(lookUpTableDimensions / 2, lookUpTableDimensions / 2));
        // Fill look up tabel position with starting room position
        this.lookUpTable.fillPosition(this.dungeonTree.DungeonPosition);
        // Create starting room gameobject
        Instantiate(startingRoom.prefab, Vector3.zero, Quaternion.AngleAxis(startingRoom.rotation, Vector3.up));

        // Queue used for BFS dungeon generation
        this.nodeQueue = new Queue<DungeonNode>();
        // Add root node to queue to start generation
        this.nodeQueue.Enqueue(this.dungeonTree);
    }

    void Update()
    {
        if(dungeonGenerationState == 0)
        {
            GenerateDungeon();
        }
    }

    private void GenerateDungeon()
    {
        DungeonNode node = this.nodeQueue.Dequeue();
        List<Transform> dungeonRoomDoorways = node.DungeonRoom.GetDooorways();

        foreach (Transform transform in dungeonRoomDoorways)
        {
            Vector2Int nextPosition = new Vector2Int();
            // Top entrance
            if (transform.forward == Vector3.forward)
            {
                nextPosition.Set(node.DungeonPosition.x, node.DungeonPosition.y - 1);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonRoom room = this.template.TopEntranceRooms[Random.Range(0, this.template.TopEntranceRooms.Count - 1)];
                    DungeonRoom newRoom = new DungeonRoom();
                    newRoom.rotation = room.rotation;
                    newRoom.prefab = Instantiate(room.prefab);
                    Vector3 currentNodePosition = node.GetPosition();
                    newRoom.prefab.transform.position = new Vector3(currentNodePosition.x, 0.0f, currentNodePosition.z + this.template.tileDimension);
                    newRoom.prefab.transform.rotation = Quaternion.AngleAxis(room.rotation, Vector3.up);
                    DungeonNode newNode = new DungeonNode(newRoom, nextPosition, node);
                    this.lookUpTable.fillPosition(nextPosition);
                    nodeQueue.Enqueue(newNode);
                }
            }
            // Bottom entrance
            else if (transform.forward == Vector3.back)
            {
                nextPosition.Set(node.DungeonPosition.x, node.DungeonPosition.y + 1);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonRoom room = this.template.BottomEntranceRooms[Random.Range(0, this.template.BottomEntranceRooms.Count - 1)];
                    DungeonRoom newRoom = new DungeonRoom();
                    newRoom.rotation = room.rotation;
                    newRoom.prefab = Instantiate(room.prefab);
                    Vector3 currentNodePosition = node.GetPosition();
                    newRoom.prefab.transform.position = new Vector3(currentNodePosition.x, 0.0f, currentNodePosition.z - this.template.tileDimension);
                    newRoom.prefab.transform.rotation = Quaternion.AngleAxis(room.rotation, Vector3.up);
                    DungeonNode newNode = new DungeonNode(newRoom, nextPosition, node);
                    this.lookUpTable.fillPosition(nextPosition);
                    nodeQueue.Enqueue(newNode);
                }
            }
            // Right entrance
            if (transform.forward == Vector3.right)
            {
                nextPosition.Set(node.DungeonPosition.x + 1, node.DungeonPosition.y);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonRoom room = this.template.RightEntranceRooms[Random.Range(0, this.template.RightEntranceRooms.Count - 1)];
                    DungeonRoom newRoom = new DungeonRoom();
                    newRoom.rotation = room.rotation;
                    newRoom.prefab = Instantiate(room.prefab);
                    Vector3 currentNodePosition = node.GetPosition();
                    newRoom.prefab.transform.position = new Vector3(currentNodePosition.x + this.template.tileDimension, 0.0f, currentNodePosition.z);
                    newRoom.prefab.transform.rotation = Quaternion.AngleAxis(room.rotation, Vector3.up);
                    DungeonNode newNode = new DungeonNode(newRoom, nextPosition, node);
                    this.lookUpTable.fillPosition(nextPosition);
                    nodeQueue.Enqueue(newNode);
                }
            }
            // Left entrance
            else if (transform.forward == Vector3.left)
            {
                nextPosition.Set(node.DungeonPosition.x - 1, node.DungeonPosition.y);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonRoom room = this.template.LeftEntranceRooms[Random.Range(0, this.template.LeftEntranceRooms.Count - 1)];
                    DungeonRoom newRoom = new DungeonRoom();
                    newRoom.rotation = room.rotation;
                    newRoom.prefab = Instantiate(room.prefab);
                    Vector3 currentNodePosition = node.GetPosition();
                    newRoom.prefab.transform.position = new Vector3(currentNodePosition.x - this.template.tileDimension, 0.0f, currentNodePosition.z);
                    newRoom.prefab.transform.rotation = Quaternion.AngleAxis(room.rotation, Vector3.up);
                    DungeonNode newNode = new DungeonNode(newRoom, nextPosition, node);
                    this.lookUpTable.fillPosition(nextPosition);
                    nodeQueue.Enqueue(newNode);
                }
            }
        }
        if (this.nodeQueue.Count == 0)
        {
            this.dungeonGenerationState++;
        }
    }
}
