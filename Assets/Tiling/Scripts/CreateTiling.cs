using OSU.Tiling;
using OSU.Tiling.TileSets;
using OSU.Tiling.TilingBuilders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTiling : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public UnityTileSetBuilder tileSetBuilder = null;
    //private ITileSet tileSet;
    private ITiling2D tiling;
    //private UnityTileSetBuilder unityTileSet;
    void Start()
    {
        //CreateTileSet();
        CreateRandomTiling();
        //RemapTilingToPrefabs();
        InstantiateTiles();
        this.gameObject.isStatic = true;
    }

    private void InstantiateTiles()
    {
        int tileWidth = 10;
        int tileHeight = 10;
        for (int j = 0; j < tiling.Height; j++)
        {
            for(int i=0; i < tiling.Width; i++)
            {
                ITile2D tile = tiling.Tile(i, j);
                UnityTile unityTile = tile as UnityTile;
                GameObject prefab = unityTile.Prefab;
                GameObject instance = Instantiate<GameObject>(prefab, this.gameObject.transform);
                instance.transform.Translate(i * tileWidth, 0, -j * tileHeight, Space.World);
            }
        }
    }

    //private void CreateTileSet()
    //{
    //    //tileSet = new SimpleTestTileSet();
    //    tileSet = new AbstractMazeTileSet();
    //    tileSet = TileDatabase.Instance.GetTileSet("Abstract Maze");
    //}
    private void CreateRandomTiling()
    {
        ITilingBuilder tilingBuilder;
        //tilingBuilder = new RandomTilingBuilder(width, height);
        tilingBuilder = new MatchingTilingBuilder(width, height);
        //SequentialTileSelector tileSelector = new SequentialTileSelector(tileSet);
        RandomTileSelector tileSelector = new RandomTileSelector();
        tileSelector.DefaultTile = tileSetBuilder.DefaultTile;
        ITilingEnumerator tilingEnumerator;
        tilingEnumerator = new RandomTilingEnumerator();
        //tilingEnumerator = new SequentialTilingEnumerator();
        //tilingEnumerator = new SequentialTilingEnumerator(width, height);
        tilingBuilder.TileSelector = tileSelector;
        
        tilingBuilder.TilingEnumerator = tilingEnumerator;

        tilingBuilder.UpdateTiling(tileSetBuilder.UnityTileSet);
        tiling = tilingBuilder.GetTiling();
    }
    void RemapTilingToPrefabs()
    {
        ITileSet newTileSet = tileSetBuilder.UnityTileSet;
        TilingRemapper remapper = new TilingRemapper(tiling, newTileSet);
        foreach (var tile in newTileSet)
        {
            for (int i = 0; i < 16; i++)
                remapper.AddMapping(i, tile.ID);
        }
        ITiling2D newTiling = remapper.Remap(new RandomTilingEnumerator());
        tiling = newTiling;

    }
}
