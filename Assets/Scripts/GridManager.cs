using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridManager : MonoBehaviour {
    public Tilemap tilemap;
    BoundsInt bounds;
    public Vector3Int[,] spots;
    private Astar astar;
    public int maxSteps = 1000;

    private void Start() {
        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;
        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
    }

    private void CreateGrid() {
        spots = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < bounds.size.x; x++, i++) {
            for (int y = bounds.yMin, j = 0; j < bounds.size.y; y++, j++) {
                Vector3Int myGridPos = new Vector3Int(x, y, 0);
                TileBase myTile = tilemap.GetTile(myGridPos);
                if (myTile && myTile is TileData { walkable: true }) {
                    spots[i, j] = new Vector3Int(x, y, 0);
                } else {
                    spots[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

    public List<Spot> CreatePath(Vector2Int startCell, Vector2Int endCell) {
        return astar.CreatePath(spots, startCell, endCell, maxSteps);
    }
}
