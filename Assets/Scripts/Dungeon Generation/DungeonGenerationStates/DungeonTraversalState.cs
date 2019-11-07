using System.Collections.Generic;
using UnityEngine;
using static DungeonRoom.RoomRotationAngle;

public class DungeonTraversalState : IDungeonGenerationState
{
    private int numberOfEndRoomsSet;
    private DungeonTemplate template;
    private DungeonNode dungeonTree;
    private Stack<DungeonNode> nodeStack;
    private List<DungeonNode> endingRooms;

    public DungeonTraversalState(DungeonTemplate template, DungeonNode dungeonTree)
    {
        this.template = template;
        this.dungeonTree = dungeonTree;
        this.numberOfEndRoomsSet = 0;
        this.endingRooms = new List<DungeonNode>();
        this.nodeStack = new Stack<DungeonNode>();
        this.nodeStack.Push(this.dungeonTree);
    }

    public IDungeonGenerationState Update()
    {
        IDungeonGenerationState newState = null;

        if(this.nodeStack.Count > 0)
        {
            GetEndRooms();
        }
        else if(this.numberOfEndRoomsSet > 0)
        {
            SetEndingRooms();
        }
        else
        {
            newState = new FinishedDungeonState();
        }

        return newState;
    }

    private void GetEndRooms()
    {
        DungeonNode node = this.nodeStack.Pop();

        if(node.ParentNode != null)
        {
            node.Depth = node.ParentNode.Depth + 1;
        }

        if(IsEndingRoom(node))
        {
            this.endingRooms.Add(node);
            this.numberOfEndRoomsSet++;
        }

        if (node.TopNode != null)
        {
            this.nodeStack.Push(node.TopNode);
        }

        if(node.BottomNode != null)
        {
            this.nodeStack.Push(node.BottomNode);
        }

        if (node.RightNode != null)
        {
            this.nodeStack.Push(node.RightNode);
        }

        if (node.LeftNode != null)
        {
            this.nodeStack.Push(node.LeftNode);
        }
    }

    private void SetEndingRooms()
    {
        List<DungeonNode> deepestEndingRooms = new List<DungeonNode>();
        int minDepth = int.MaxValue;

        foreach (DungeonNode node in this.endingRooms)
        {
            if (node.Depth < minDepth)
            {
                minDepth = node.Depth;
            }
        }

        foreach(DungeonNode node in this.endingRooms)
        {
            if(minDepth == node.Depth)
            {
                deepestEndingRooms.Add(node);
            }
        }

        int endingRoomPosition = Random.Range(0, deepestEndingRooms.Count);
        DungeonNode replacementNode = deepestEndingRooms[endingRoomPosition];
        AddSpecialDeadEndRoom(replacementNode);
        this.endingRooms.Remove(replacementNode);
    }

    private void AddSpecialDeadEndRoom(DungeonNode nodeToReplace)
    {
        DungeonNode newNode = null;
        List<DungeonRoom> roomList;
        Vector3 directon = (nodeToReplace.GetPosition() - nodeToReplace.ParentNode.GetPosition()).normalized;
        Vector2 connectionDirection = new Vector2(directon.x, directon.z);

        if(this.numberOfEndRoomsSet == 1)
        {
            roomList = this.template.GoalRooms;
        }
        else if(this.numberOfEndRoomsSet == 2)
        {
            roomList = this.template.Shops;
        }
        else
        {
            roomList = this.template.DeadEndRooms;
        }

        // Top entrance
        if (connectionDirection == Vector2.up)
        {
            newNode = DungeonGenerator.AddDeadEnd(roomList,
                nodeToReplace.lookUpPosition,
                nodeToReplace.ParentNode,
                0.0f,
                this.template.tileDimension,
                DO_NOT_ROTATE);

            nodeToReplace.ParentNode.TopNode = newNode;
        }
        // Bottom entrance
        else if (connectionDirection == Vector2.down)
        {
            newNode = DungeonGenerator.AddDeadEnd(roomList,
                nodeToReplace.lookUpPosition,
                nodeToReplace.ParentNode,
                0.0f,
                -this.template.tileDimension,
                TURN_AROUND);

            nodeToReplace.ParentNode.BottomNode = newNode;
        }
        // Right entrance
        else if (connectionDirection == Vector2.right)
        {
            newNode = DungeonGenerator.AddDeadEnd(roomList,
                nodeToReplace.lookUpPosition,
                nodeToReplace.ParentNode,
                this.template.tileDimension,
                0.0f,
                TURN_CLOCKWISE);

            nodeToReplace.ParentNode.RightNode = newNode;
        }
        // Left entrance
        else if (connectionDirection == Vector2.left)
        {
            newNode = DungeonGenerator.AddDeadEnd(roomList,
                nodeToReplace.lookUpPosition,
                nodeToReplace.ParentNode,
                -this.template.tileDimension,
                0.0f,
                TURN_COUNTER_CLOCKWISE);

            nodeToReplace.ParentNode.LeftNode = newNode;
        }

        nodeToReplace.ParentNode = null;
        Object.Destroy(nodeToReplace.DungeonRoom.prefab);

        this.numberOfEndRoomsSet--;
    }

    private bool IsEndingRoom(DungeonNode node)
    {
        bool isEndingRoom = true;

        if(node.TopNode != null || node.BottomNode != null || node.RightNode != null || node.LeftNode != null)
        {
            isEndingRoom = false;
        }

        return isEndingRoom;
    }
}
