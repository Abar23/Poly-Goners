using UnityEngine;

public class DungeonNode
{
    public DungeonRoom DungeonRoom { get; set; }
    public DungeonNode LeftNode { get; set; }
    public DungeonNode RightNode { get; set; }
    public DungeonNode TopNode { get; set; }
    public DungeonNode BottomNode { get; set; }
    public DungeonNode ParentNode { get; set; }
    public Vector2Int lookUpPosition { get; set; }
    public int Depth { get; set; }

    public DungeonNode(DungeonRoom dungeonRoom, Vector2Int DungeonPosition, DungeonNode parentNode = null)
    {
        this.DungeonRoom = dungeonRoom;
        this.lookUpPosition = DungeonPosition;
        this.ParentNode = parentNode;
        this.Depth = 0;
        this.LeftNode = null;
        this.RightNode = null;
        this.TopNode = null;
        this.BottomNode = null;
    }

    public Vector3 GetPosition()
    {
        return this.DungeonRoom.prefab.transform.position;
    }
}
