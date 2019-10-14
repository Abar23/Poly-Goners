using System.Collections.Generic;
using UnityEngine;

public class DungeonTraversalState : IDungeonGenerationState
{
    private DungeonTemplate template;
    private DungeonNode dungeonTree;
    private Queue<DungeonNode> nodeStack;

    public DungeonTraversalState(DungeonTemplate template, DungeonNode dungeonTree)
    {
        this.template = template;
        this.dungeonTree = dungeonTree;
        this.nodeStack = new Queue<DungeonNode>();
        this.nodeStack.Enqueue(this.dungeonTree);
    }

    public IDungeonGenerationState Update()
    {
        IDungeonGenerationState newState = null;

        if(this.nodeStack.Count > 0)
        {
            TraverseDungeon();
        }
        else
        {
            newState = new FinishedDungeonState();
        }

        return newState;
    }

    private void TraverseDungeon()
    {
        DungeonNode node = this.nodeStack.Dequeue();

        //Debug.Log(node.lookUpPosition);

        if (node.TopNode != null)
        {
            this.nodeStack.Enqueue(node.TopNode);
        }

        if(node.BottomNode != null)
        {
            this.nodeStack.Enqueue(node.BottomNode);
        }

        if (node.RightNode != null)
        {
            this.nodeStack.Enqueue(node.RightNode);
        }

        if (node.LeftNode != null)
        {
            this.nodeStack.Enqueue(node.LeftNode);
        }
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
