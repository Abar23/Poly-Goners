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

    public bool IsPositionFilled(Vector2Int position)
    {
        bool isFilled = true;

        if(position.x >= 0 && position.x < this.tableDimension && position.y >= 0 && position.y < this.tableDimension)
        {
            isFilled = this.roomPlacementMap[position.x, position.y];
        }
        
        return isFilled;
    }

    public void fillPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < this.tableDimension && position.y >= 0 && position.y < this.tableDimension)
        {
            this.roomPlacementMap[position.x, position.y] = true;
        }
    }
}
