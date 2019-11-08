using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonRoom.RoomRotationAngle;

public class DungeonCreationState : IDungeonGenerationState
{
    private int numberOfRooms;
    private float tileDimensions;
    private DungeonNode dungeonTree;
    private DungeonLookUpTable lookUpTable;
    private Queue<DungeonNode> nodeQueue;
    private DungeonGenerator generator;

    public DungeonCreationState(
        float tileDimensions, 
        DungeonLookUpTable lookUpTable,
        DungeonNode dungeonTree,
        int numberOfRooms,
        DungeonGenerator generator)
    {
        this.tileDimensions = tileDimensions;
        this.lookUpTable = lookUpTable;
        this.dungeonTree = dungeonTree;
        this.numberOfRooms = numberOfRooms;
        this.generator = generator;

        // Queue used for BFS dungeon generation
        this.nodeQueue = new Queue<DungeonNode>();

        // Add root node to queue to start generation
        this.nodeQueue.Enqueue(this.dungeonTree);
    }

    public IDungeonGenerationState Update()
    {
        IDungeonGenerationState newState = null;

        if (this.nodeQueue.Count > 0)
        {
            GenerateDungeon();
        }
        else
        {
            newState = new DungeonTraversalState(this.tileDimensions, this.dungeonTree, this.generator);
        }

        return newState;
    }

    private void GenerateDungeon()
    {
        DungeonNode currentNode = this.nodeQueue.Dequeue();
        List<Transform> dungeonRoomDoorways = currentNode.DungeonRoom.GetScrambledDoorways();

        if (!IsDungeonNodeValid(currentNode, currentNode.ParentNode, dungeonRoomDoorways))
        {
            if (currentNode.ParentNode != null)
            {
                currentNode = ResolveInvalidNode(currentNode, currentNode.ParentNode);

                dungeonRoomDoorways = currentNode.DungeonRoom.GetScrambledDoorways();
            }
        }

        // Decrement number of rooms by the doorways of the current node. 
        // Those doorways will have rooms attached to them at some point.
        if (currentNode.ParentNode == null)
        {
            this.numberOfRooms -= dungeonRoomDoorways.Count; // Start node has no connections
        }
        else
        {
            this.numberOfRooms -= dungeonRoomDoorways.Count - 1; // Further nodes wil always have one entrance connected to a door
        }

        foreach (Transform doorway in dungeonRoomDoorways)
        {
            DungeonNode newNode = null;
            Vector2Int nextPosition = new Vector2Int();

            // Top entrance
            if (doorway.forward == Vector3.forward)
            {
                nextPosition.Set(currentNode.lookUpPosition.x, currentNode.lookUpPosition.y - 1);
                if (!this.lookUpTable.IsPositionFilled(nextPosition))
                {
                    if(this.numberOfRooms > 0)
                    {
                        newNode = DungeonGenerator.AddNewRoom(this.generator.TopRooms,
                            nextPosition,
                            currentNode,
                            0.0f,
                            this.tileDimensions,
                            this.numberOfRooms);
                    }

                    if (this.numberOfRooms == 0 || newNode == null)
                    {
                        newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                            nextPosition,
                            currentNode,
                            0.0f,
                            this.tileDimensions,
                            DO_NOT_ROTATE);
                    }

                    if(newNode != null)
                    {
                        currentNode.TopNode = newNode;

                        // Fill look up table position for the new dungeon room that was added
                        this.lookUpTable.fillPosition(nextPosition);

                        // Add the new node to the queue to continue BFS dungeon generation
                        nodeQueue.Enqueue(newNode);
                    }
                }
            }

            // Bottom entrance
            else if (doorway.forward == Vector3.back)
            {
                nextPosition.Set(currentNode.lookUpPosition.x, currentNode.lookUpPosition.y + 1);
                if (!this.lookUpTable.IsPositionFilled(nextPosition))
                {
                    if (this.numberOfRooms > 0)
                    {
                        newNode = DungeonGenerator.AddNewRoom(this.generator.BottomRooms,
                            nextPosition,
                            currentNode,
                            0.0f,
                            -this.tileDimensions,
                            this.numberOfRooms);
                    }

                    if (this.numberOfRooms == 0 || newNode == null)
                    {
                        newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                            nextPosition,
                            currentNode,
                            0.0f,
                            -this.tileDimensions,
                            TURN_AROUND);
                    }

                    if (newNode != null)
                    {
                        currentNode.BottomNode = newNode;

                        // Fill look up table position for the new dungeon room that was added
                        this.lookUpTable.fillPosition(nextPosition);

                        // Add the new node to the queue to continue BFS dungeon generation
                        nodeQueue.Enqueue(newNode);
                    }
                }
            }

            // Right entrance
            else if (doorway.forward == Vector3.right)
            {
                nextPosition.Set(currentNode.lookUpPosition.x + 1, currentNode.lookUpPosition.y);
                if (!this.lookUpTable.IsPositionFilled(nextPosition))
                {
                    if (this.numberOfRooms > 0)
                    {
                        newNode = DungeonGenerator.AddNewRoom(this.generator.RightRooms,
                            nextPosition,
                            currentNode,
                            this.tileDimensions,
                            0.0f,
                            this.numberOfRooms);
                    }
                    
                    if(this.numberOfRooms == 0 || newNode == null)
                    {
                        newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                            nextPosition,
                            currentNode,
                            this.tileDimensions,
                            0.0f,
                            TURN_CLOCKWISE);
                    }

                    if (newNode != null)
                    {
                        currentNode.RightNode = newNode;

                        // Fill look up table position for the new dungeon room that was added
                        this.lookUpTable.fillPosition(nextPosition);

                        // Add the new node to the queue to continue BFS dungeon generation
                        nodeQueue.Enqueue(newNode);
                    }
                }
            }

            // Left entrance
            else if (doorway.forward == Vector3.left)
            {
                nextPosition.Set(currentNode.lookUpPosition.x - 1, currentNode.lookUpPosition.y);
                if (!this.lookUpTable.IsPositionFilled(nextPosition))
                {
                    if (this.numberOfRooms > 0)
                    {
                        newNode = DungeonGenerator.AddNewRoom(this.generator.LeftRooms,
                            nextPosition,
                            currentNode,
                            -this.tileDimensions,
                            0.0f,
                            this.numberOfRooms);
                    }

                    if (this.numberOfRooms == 0 || newNode == null)
                    {
                        newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                            nextPosition,
                            currentNode,
                            -this.tileDimensions,
                            0.0f,
                            TURN_COUNTER_CLOCKWISE);
                    }

                    if (newNode != null)
                    {
                        currentNode.LeftNode = newNode;

                        // Fill look up table position for the new dungeon room that was added
                        this.lookUpTable.fillPosition(nextPosition);

                        // Add the new node to the queue to continue BFS dungeon generation
                        nodeQueue.Enqueue(newNode);
                    }
                }
            }
        }
    }

    private bool IsDungeonNodeValid(DungeonNode node, DungeonNode parentNode, List<Transform> doorways)
    {
        bool isValid = true;

        if(doorways.Count - 1 <= this.numberOfRooms)
        {
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
                    isValid = !this.lookUpTable.IsPositionFilled(checkPosition);
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
                    isValid = !this.lookUpTable.IsPositionFilled(checkPosition);
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
                    isValid = !this.lookUpTable.IsPositionFilled(checkPosition);
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
                    isValid = !this.lookUpTable.IsPositionFilled(checkPosition);
                }

                if (!isValid)
                {
                    break;
                }
            }
        }
        else
        {
            isValid = false;
        }
        
        return isValid;
    }

    public DungeonNode ResolveInvalidNode(DungeonNode invalidNode, DungeonNode parentNode)
    {
        DungeonNode newNode = null;
        List<KeyValuePair<DungeonRoom, float>> dungeonList = new List<KeyValuePair<DungeonRoom, float>>();
        Vector3 directon = (invalidNode.GetPosition() - parentNode.GetPosition()).normalized;
        Vector2 connectionDirection = new Vector2(directon.x, directon.z);

        // Top entrance
        if (connectionDirection == Vector2.up)
        {
            dungeonList = this.generator.TopRooms;
        }
        // Bottom entrance
        else if (connectionDirection == Vector2.down)
        {
            dungeonList = this.generator.BottomRooms;
        }
        // Right entrance
        else if (connectionDirection == Vector2.right)
        {
            dungeonList = this.generator.RightRooms;
        }
        // Left entrance
        else if (connectionDirection == Vector2.left)
        {
            dungeonList = this.generator.LeftRooms;
        }

        List<KeyValuePair<DungeonRoom, float>> ValidNodeList = new List<KeyValuePair<DungeonRoom, float>>();
        Quaternion originalRoomRotation;
        foreach (KeyValuePair<DungeonRoom, float> room in dungeonList)
        {
            originalRoomRotation = room.Key.prefab.transform.rotation;
            // Rotate Prefab
            room.Key.RotateRoom();

            DungeonNode potentialNode = new DungeonNode(room.Key, invalidNode.lookUpPosition);
            if (IsDungeonNodeValid(potentialNode, parentNode, room.Key.GetDooorways()))
            {
                ValidNodeList.Add(room);
            }

            // Undo rotation on Prefab
            room.Key.OverrideRotation(originalRoomRotation);
        }

        invalidNode.ParentNode = null;
        if (ValidNodeList.Count > 0)
        {
            // Top entrance
            if (connectionDirection == Vector2.up)
            {
                newNode = DungeonGenerator.AddNewRoom(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    0.0f,
                    this.tileDimensions,
                    this.numberOfRooms);

                parentNode.TopNode = newNode;
            }
            // Bottom entrance
            else if (connectionDirection == Vector2.down)
            {
                newNode = DungeonGenerator.AddNewRoom(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    0.0f,
                    -this.tileDimensions,
                    this.numberOfRooms);

                parentNode.BottomNode = newNode;
            }
            // Right entrance
            else if (connectionDirection == Vector2.right)
            {
                newNode = DungeonGenerator.AddNewRoom(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    this.tileDimensions,
                    0.0f,
                    this.numberOfRooms);

                parentNode.RightNode = newNode;
            }
            // Left entrance
            else if (connectionDirection == Vector2.left)
            {
                newNode = DungeonGenerator.AddNewRoom(ValidNodeList,
                    invalidNode.lookUpPosition,
                    parentNode,
                    -this.tileDimensions,
                    0.0f,
                    this.numberOfRooms);

                parentNode.LeftNode = newNode;
            }

            if(this.numberOfRooms == 0 && newNode != null)
            {
                this.numberOfRooms += newNode.DungeonRoom.GetDooorways().Count - 1;
            }
        }

        if(ValidNodeList.Count == 0 || newNode == null)
        {
            // Top entrance
            if (connectionDirection == Vector2.up)
            {
                newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                    invalidNode.lookUpPosition,
                    parentNode,
                    0.0f,
                    this.tileDimensions,
                    DO_NOT_ROTATE);

                parentNode.TopNode = newNode;
            }
            // Bottom entrance
            else if (connectionDirection == Vector2.down)
            {
                newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                    invalidNode.lookUpPosition,
                    parentNode,
                    0.0f,
                    -this.tileDimensions,
                    TURN_AROUND);

                parentNode.BottomNode = newNode;
            }
            // Right entrance
            else if (connectionDirection == Vector2.right)
            {
                newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                    invalidNode.lookUpPosition,
                    parentNode,
                    this.tileDimensions,
                    0.0f,
                    TURN_CLOCKWISE);

                parentNode.RightNode = newNode;
            }
            // Left entrance
            else if (connectionDirection == Vector2.left)
            {
                newNode = DungeonGenerator.AddDeadEnd(this.generator.DeadEnds,
                    invalidNode.lookUpPosition,
                    parentNode,
                    -this.tileDimensions,
                    0.0f,
                    TURN_COUNTER_CLOCKWISE);

                parentNode.LeftNode = newNode;
            }
        }
        Object.Destroy(invalidNode.DungeonRoom.prefab);

        return newNode;
    }
}
