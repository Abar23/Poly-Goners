using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSU.Tiling;
using OSU.Tiling.TileSets;

public class UnityTile : ITile2D
{
    public string TileSetName { get; private set; }

    public int ID { get; private set; }

    public int leftColor { get; set; }

    public int rightColor { get; private set; }

    public int topColor { get; private set; }

    public int bottomColor { get; private set; }
    internal GameObject Prefab { get; private set; }
    public UnityTile(string tileSetName, int id, GameObject prefab, int left, int top, int right, int bottom)
    {
        TileSetName = tileSetName;ID = id;
        Prefab = prefab;
        leftColor = left;
        topColor = top;
        rightColor = right;
        bottomColor = bottom;
    }
}
