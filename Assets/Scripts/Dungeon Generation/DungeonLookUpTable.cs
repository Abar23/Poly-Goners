using UnityEngine;

public class DungeonLookUpTable
{
    private int tableDimension;

    private bool[,] roomPlacementMap;

    public DungeonLookUpTable(int tableDimension)
    {
        this.tableDimension = tableDimension;
        this.roomPlacementMap = new bool[tableDimension, tableDimension];
    }

    public bool IsPositionOpen(Vector2Int position)
    {
        bool isPositionFilled = false;

        if(position.x >= 0 && position.x < this.tableDimension && position.y >= 0 && position.y < this.tableDimension)
        {
            isPositionFilled = !this.roomPlacementMap[position.x, position.y];
        }
        
        return isPositionFilled;
    }

    public void fillPosition(Vector2Int position)
    {
        this.roomPlacementMap[position.x, position.y] = true;
    }
}
