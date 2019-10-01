using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonTemplate template;

    private const int lookUpTableDimensions = 20;

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
        this.lookUpTable.fillPosition(this.dungeonTree.lookUpPosition);
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

        if(!IsDungeonNodeValid(node, node.ParentNode, dungeonRoomDoorways))
        {
            node = ResolveInvalidNode(node, node.ParentNode);
            if(node == null)
            {
                dungeonRoomDoorways.Clear();
            }
            else
            {
                dungeonRoomDoorways = node.DungeonRoom.GetDooorways();
            }
        }

        foreach (Transform doorway in dungeonRoomDoorways)
        {
            Vector2Int nextPosition = new Vector2Int();
            
            // Top entrance
            if (doorway.forward == Vector3.forward)
            {
                nextPosition.Set(node.lookUpPosition.x, node.lookUpPosition.y - 1);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonNode newNode = CreateNewNode(this.template.TopEntranceRooms,
                        nextPosition,
                        node,
                        0.0f,
                        this.template.tileDimension);

                    node.TopNode = newNode;
                    // Add the new node to the queue to continue BFS dungeon generation
                    nodeQueue.Enqueue(newNode);
                }
            }
           
            // Bottom entrance
            else if (doorway.forward == Vector3.back)
            {
                nextPosition.Set(node.lookUpPosition.x, node.lookUpPosition.y + 1);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonNode newNode = CreateNewNode(this.template.BottomEntranceRooms,
                        nextPosition,
                        node,
                        0.0f,
                        -this.template.tileDimension);

                    node.BottomNode = newNode;
                    // Add the new node to the queue to continue BFS dungeon generation
                    nodeQueue.Enqueue(newNode);
                }
            }
            
            // Right entrance
            else if (doorway.forward == Vector3.right)
            {
                nextPosition.Set(node.lookUpPosition.x + 1, node.lookUpPosition.y);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonNode newNode = CreateNewNode(this.template.RightEntranceRooms,
                        nextPosition,
                        node,
                        this.template.tileDimension,
                        0.0f);

                    node.RightNode = newNode;
                    // Add the new node to the queue to continue BFS dungeon generation
                    nodeQueue.Enqueue(newNode);
                }
            }
            
            // Left entrance
            else if (doorway.forward == Vector3.left)
            {
                nextPosition.Set(node.lookUpPosition.x - 1, node.lookUpPosition.y);
                if (this.lookUpTable.IsPositionOpen(nextPosition))
                {
                    DungeonNode newNode = CreateNewNode(this.template.LeftEntranceRooms, 
                        nextPosition,
                        node,
                        -this.template.tileDimension,
                        0.0f);

                    node.LeftNode = newNode;
                    // Add the new node to the queue to continue BFS dungeon generation
                    nodeQueue.Enqueue(newNode);
                }
            }
        }
        if (this.nodeQueue.Count == 0)
        {
            this.dungeonGenerationState++;
        }
    }

    private DungeonNode CreateNewNode(List<DungeonRoom> roomList,
        Vector2Int lookUpTablePosition, 
        DungeonNode parentNode, 
        float roomXPositionOffset, 
        float roomYPositionOffset)
    {
        // Get random room from list of rooms
        DungeonRoom room = roomList[Random.Range(0, roomList.Count - 1)];

        // Create copy of the randomly chosen room
        DungeonRoom newRoom = new DungeonRoom(Instantiate(room.prefab), room.rotation);

        // Get the parent node position
        Vector3 parentNodePosition = parentNode.GetPosition();
        
        // Set the new dungeon room to the proper position and rotation
        newRoom.SetPosition(new Vector3(parentNodePosition.x + roomXPositionOffset, 0.0f, parentNodePosition.z + roomYPositionOffset));
        newRoom.SetRotation(room.rotation);

        // Add the new dungeon node to the dungeon tree
        DungeonNode newNode = new DungeonNode(newRoom, lookUpTablePosition, parentNode);

        // Fill look up table position for the new dungeon room that was added
        this.lookUpTable.fillPosition(lookUpTablePosition);

        return newNode;
    }

    private bool IsDungeonNodeValid(DungeonNode node, DungeonNode parentNode, List<Transform> doorways)
    {
        bool isValid = true;

        foreach (Transform doorway in doorways)
        {
            Vector2Int checkPosition = new Vector2Int();
            // Top entrance
            if (doorway.forward == Vector3.forward)
            {
                checkPosition.Set(node.lookUpPosition.x, node.lookUpPosition.y - 1);
                if (parentNode != null)
                {
                    if (parentNode.lookUpPosition == checkPosition)
                    {
                        continue;
                    }
                }
                isValid = this.lookUpTable.IsPositionOpen(checkPosition);
            }

            // Bottom entrance
            else if (doorway.forward == Vector3.back)
            {
                checkPosition.Set(node.lookUpPosition.x, node.lookUpPosition.y + 1);
                if (parentNode != null)
                {
                    if (parentNode.lookUpPosition == checkPosition)
                    {
                        continue;
                    }
                }
                isValid = this.lookUpTable.IsPositionOpen(checkPosition);
            }

            // Right entrance
            else if (doorway.forward == Vector3.right)
            {
                checkPosition.Set(node.lookUpPosition.x + 1, node.lookUpPosition.y);
                if (parentNode != null)
                {
                    if (parentNode.lookUpPosition == checkPosition)
                    {
                        continue;
                    }
                }
                isValid = this.lookUpTable.IsPositionOpen(checkPosition);
            }

            // Left entrance
            else if (doorway.forward == Vector3.left)
            {
                checkPosition.Set(node.lookUpPosition.x - 1, node.lookUpPosition.y);
                if (parentNode != null)
                {
                    if (parentNode.lookUpPosition == checkPosition)
                    {
                        continue;
                    }
                }
                else
                isValid = this.lookUpTable.IsPositionOpen(checkPosition);
            }

            if (!isValid)
            {
                break;
            }
        }
        return isValid;
    }

    public DungeonNode ResolveInvalidNode(DungeonNode invalidNode, DungeonNode parentNode)
    {
        DungeonNode newNode = null;
        List<DungeonRoom> dungeonList;
        Vector2Int connectionDirection = invalidNode.lookUpPosition - parentNode.lookUpPosition;
        
        // Top entrance
        if (connectionDirection == Vector2Int.up)
        {
            dungeonList = this.template.TopEntranceRooms;
        }
        // Bottom entrance
        else if (connectionDirection == Vector2Int.down)
        {
            dungeonList = this.template.BottomEntranceRooms;
        }
        // Right entrance
        else if (connectionDirection == Vector2Int.right)
        {
            dungeonList = this.template.RightEntranceRooms;
        }
        // Left entrance
        else
        {
            dungeonList = this.template.LeftEntranceRooms;
        }

        List<DungeonRoom> ValidNodeList = new List<DungeonRoom>();
        foreach(DungeonRoom room in dungeonList)
        {
            room.SetRotation(room.rotation);
            DungeonNode potentialNode = new DungeonNode(room, invalidNode.lookUpPosition);
            bool isValid = IsDungeonNodeValid(potentialNode, parentNode, room.GetDooorways());
            room.SetRotation(0);
            if (isValid)
            {
                ValidNodeList.Add(room);
            }
        }

        if (ValidNodeList.Count > 0)
        {
            invalidNode.ParentNode = null;
            Destroy(invalidNode.DungeonRoom.prefab);
            Debug.Log(invalidNode.lookUpPosition);

            // Top entrance
            if (connectionDirection == Vector2Int.up)
            {
                newNode = CreateNewNode(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    0.0f,
                    this.template.tileDimension);

                parentNode.TopNode = newNode;
            }
            // Bottom entrance
            else if (connectionDirection == Vector2Int.down)
            {
                newNode = CreateNewNode(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    0.0f,
                    -this.template.tileDimension);

                parentNode.BottomNode = newNode;
            }
            // Right entrance
            else if (connectionDirection == Vector2Int.right)
            {
                newNode = CreateNewNode(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    this.template.tileDimension,
                    0.0f);

                parentNode.RightNode = newNode;
            }
            // Left entrance
            else
            {
                newNode = CreateNewNode(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    -this.template.tileDimension,
                    0.0f);

                parentNode.LeftNode = newNode;
            }
        }

        return newNode;
    }
}
