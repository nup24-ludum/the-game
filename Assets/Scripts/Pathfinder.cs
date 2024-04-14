using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinder : MonoBehaviour {
    GridManager GM;
    List<Spot> path = new List<Spot>();

    void Start() {
        GM = FindObjectOfType<GridManager>();
    }


    // pathfinds and returns the next position we want to move to
    Vector2Int FindNextPosition(Vector2Int destination) {
        Vector3Int gridPos = GM.tilemap.WorldToCell(transform.position);
        Vector2Int myPosition = new Vector2Int(gridPos.x, gridPos.y);

        path = GM.CreatePath(myPosition, destination);
        return path.Count < 2 ? Vector2Int.zero : // we don't have a proper path
            new Vector2Int(path[path.Count - 2].x, path[path.Count - 2].y);
    }


    public Color markerColor = Color.yellow;
    public float markerSize = 0.2f;

    private void OnDrawGizmosSelected() {
        if (!GM) {
            return;
        }

        Gizmos.color = markerColor;
        Grid grid = GM.tilemap.layoutGrid;

        foreach (Spot s in path) {
            Vector3 worldPosition = grid.GetCellCenterWorld(new Vector3Int(s.x, s.y, 0));
            Gizmos.DrawSphere(worldPosition, markerSize);
        }
    }
}
