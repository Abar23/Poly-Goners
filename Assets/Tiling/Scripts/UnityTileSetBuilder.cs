using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSU.Tiling;
using OSU.Tiling.TileSets;

public class UnityTileSetBuilder : MonoBehaviour
{
    public string Name;
    public string Description = "blank";
    public TileData[] Tiles;
    public TileData DefaultTileInfo;
    private TileSet tileSet;
    public ITileSet UnityTileSet { get; private set; }
    public ITile2D DefaultTile { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        tileSet = new TileSet(Name, Description, 10, 10);
        int id = 0;
        foreach(var data in Tiles)
        {
            GameObject prefab = data.physicalTile;
            UnityTile tile = new UnityTile(prefab.name, id++, prefab, data.leftColor, data.topColor, data.rightColor, data.bottomColor);
            tileSet.AddTile(tile);
        }
        DefaultTile = new UnityTile(DefaultTileInfo.physicalTile.name, id, DefaultTileInfo.physicalTile, DefaultTileInfo.leftColor, DefaultTileInfo.topColor, DefaultTileInfo.rightColor, DefaultTileInfo.bottomColor);
        tileSet.SetDefaultTile(DefaultTile);
        UnityTileSet = tileSet;
    }
}
