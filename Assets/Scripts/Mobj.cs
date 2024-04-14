using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mobj : MonoBehaviour {
    // private GridManager gm;
    // private List<Spot> path = new List<Spot>();
    //
    // // pathfinds and returns the next position we want to move to
    // Vector2Int FindNextPosition(Vector2Int destination) {
    //     Vector3Int gridPos = GM.tilemap.WorldToCell(transform.position);
    //     Vector2Int myPosition = new Vector2Int(gridPos.x, gridPos.y);
    //
    //     path = GM.CreatePath(myPosition, destination);
    //     if (path.Count < 2) return Vector2Int.zero; // we don't have a proper path
    //     return new Vector2Int(path[path.Count-2].X, path[path.Count-2].Y);
    // }
    //
    //
    //
    // public Color markerColor = Color.yellow;
    // public float markerSize = 0.2f;
    //
    // void OnDrawGizmosSelected() {
    //     if (GM) {
    //         Gizmos.color = markerColor;
    //         Grid grid = GM.tilemap.layoutGrid;
    //
    //         foreach (Spot s in path) {
    //             Vector3 worldPosition = grid.GetCellCenterWorld(new Vector3Int(s.X, s.Y, 0));
    //             Gizmos.DrawSphere(worldPosition, markerSize);
    //         }
    //     }
    // }

    public enum Team {
        PLAYER,
        MONSTER,
        ITEM,
    }

    public Vector2Int mapPos;

    public abstract Team team();
    public abstract void DoTurn();
    public abstract bool TurnReady();

    public bool isItem() => team() == Team.ITEM;
}
