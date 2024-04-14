using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Code from: https://bitbucket.org/Sniffle6/tilemaps-with-astar
public class Astar {
    public Spot[,] spots;

    public Astar(Vector3Int[,] grid, int columns, int rows) {
        spots = new Spot[columns, rows];
    }

    private bool IsValidPath(Vector3Int[,] grid, Spot start, Spot end) {
        if (end == null) {
            return false;
        }

        if (start == null) {
            return false;
        }

        return end.height < 1;
    }

    public List<Spot> CreatePath(Vector3Int[,] grid, Vector2Int start, Vector2Int end, int length) {
        //if (!IsValidPath(grid, start, end))
        //     return null;

        Spot endSpot = null;
        Spot startSpot = null;
        int columns = spots.GetUpperBound(0) + 1;
        int rows = spots.GetUpperBound(1) + 1;
        spots = new Spot[columns, rows];

        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                spots[i, j] = new Spot(grid[i, j].x, grid[i, j].y, grid[i, j].z);
            }
        }

        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                spots[i, j].AddNeighboors(spots, i, j);
                if (spots[i, j].x == start.x && spots[i, j].y == start.y) {
                    startSpot = spots[i, j];
                } else if (spots[i, j].x == end.x && spots[i, j].y == end.y) {
                    endSpot = spots[i, j];
                }
            }
        }

        if (!IsValidPath(grid, startSpot, endSpot)) {
            return null;
        }

        List<Spot> openSet = new List<Spot>();
        List<Spot> closedSet = new List<Spot>();

        openSet.Add(startSpot);

        while (openSet.Count > 0) {
            //Find the shortest step distance in the direction of your goal within the open set
            int winner = 0;
            for (int i = 0; i < openSet.Count; i++) {
                if (openSet[i].f < openSet[winner].f) {
                    winner = i;
                } else if (openSet[i].f == openSet[winner].f) //tie breaking for faster routing
                {
                    if (openSet[i].h < openSet[winner].h) {
                        winner = i;
                    }
                }
            }

            Spot current = openSet[winner];

            //Found the path, creates and returns the path
            if (endSpot != null && openSet[winner] == endSpot) {
                List<Spot> path = new List<Spot>();
                var temp = current;
                path.Add(temp);
                while (temp.previous != null) {
                    path.Add(temp.previous);
                    temp = temp.previous;
                }

                if (length - (path.Count - 1) < 0) {
                    path.RemoveRange(0, (path.Count - 1) - length);
                }

                return path;
            }

            openSet.Remove(current);
            closedSet.Add(current);


            //Finds the next closest step on the grid
            List<Spot> neighboors = current.neighboors;
            for (int i = 0;
                 i < neighboors.Count;
                 i++) //look threw our current spots neighboors (current spot is the shortest F distance in openSet
            {
                var n = neighboors[i];
                if (!closedSet.Contains(n) &&
                    n.height < 1) //Checks to make sure the neighboor of our current tile is not within closed set, and has a height of less than 1
                {
                    var tempG = current.g +
                                1; //gets a temp comparison integer for seeing if a route is shorter than our current path

                    bool newPath = false;
                    if (openSet.Contains(n)) //Checks if the neighboor we are checking is within the openset
                    {
                        if (tempG < n.g) //The distance to the end goal from this neighboor is shorter so we need a new path
                        {
                            n.g = tempG;
                            newPath = true;
                        }
                    } else //if its not in openSet or closed set, then it IS a new path and we should add it too openset
                    {
                        n.g = tempG;
                        newPath = true;
                        openSet.Add(n);
                    }

                    if (newPath) //if it is a newPath caclulate the H and F and set current to the neighboors previous
                    {
                        n.h = Heuristic(n, endSpot);
                        n.f = n.g + n.h;
                        n.previous = current;
                    }
                }
            }
        }

        return null;
    }

    private int Heuristic(Spot a, Spot b) {
        //manhattan
        int dx = Math.Abs(a.x - b.x);
        int dy = Math.Abs(a.y - b.y);
        return 1 * (dx + dy);

        #region diagonal

        //diagonal
        // Chebyshev distance
        //var D = 1;
        // var D2 = 1;
        //octile distance
        //var D = 1;
        //var D2 = 1;
        //var dx = Math.Abs(a.X - b.X);
        //var dy = Math.Abs(a.Y - b.Y);
        //var result = (int)(1 * (dx + dy) + (D2 - 2 * D));
        //return result;// *= (1 + (1 / 1000));
        //return (int)Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        #endregion
    }
}

public class Spot {
    public int x;
    public int y;
    public int f;
    public int g;
    public int h;
    public int height = 0;
    public List<Spot> neighboors;
    public Spot previous = null;

    public Spot(int x, int y, int height) {
        this.x = x;
        this.y = y;
        f = 0;
        g = 0;
        h = 0;
        neighboors = new List<Spot>();
        this.height = height;
    }

    public void AddNeighboors(Spot[,] grid, int x, int y) {
        if (x < grid.GetUpperBound(0)) {
            neighboors.Add(grid[x + 1, y]);
        }

        if (x > 0) {
            neighboors.Add(grid[x - 1, y]);
        }

        if (y < grid.GetUpperBound(1)) {
            neighboors.Add(grid[x, y + 1]);
        }

        if (y > 0) {
            neighboors.Add(grid[x, y - 1]);
        }

        #region diagonal

        //if (X > 0 && Y > 0)
        //    Neighboors.Add(grid[X - 1, Y - 1]);
        //if (X < Utils.Columns - 1 && Y > 0)
        //    Neighboors.Add(grid[X + 1, Y - 1]);
        //if (X > 0 && Y < Utils.Rows - 1)
        //    Neighboors.Add(grid[X - 1, Y + 1]);
        //if (X < Utils.Columns - 1 && Y < Utils.Rows - 1)
        //    Neighboors.Add(grid[X + 1, Y + 1]);

        #endregion
    }
}
