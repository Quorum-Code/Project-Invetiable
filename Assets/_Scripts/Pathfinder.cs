using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public bool initialized { get; private set; }

    List<int[]> generatedPath = new List<int[]>();
    List<Tile> generatedTilePath = new List<Tile>();
    Tile[,] tileGrid;
    float[,] MoveCostGrid;


    // NEEDS TO BE WORKED ON
    public bool initialize(Tile[,] levelTiles) 
    {
        // Check if _moveCostGrid is null
        if (levelTiles == null)
        {
            initialized = false;
        }
        else
        {
            // Import new moveCostGrid
            tileGrid = levelTiles;
            initialized = true;
        }

        return initialized;
    }

    public bool initialize(float[,] _moveCostGrid) 
    {
        // Check if _moveCostGrid is null
        if (_moveCostGrid == null)
        {
            initialized = false;
        }
        else 
        {
            // Import new moveCostGrid
            MoveCostGrid = _moveCostGrid;
            initialized = true;
        }
            
        return initialized;
    }

    // NEEDS TO BE WORK ON
    public List<Tile> Pathfind(Pawn pawn, int[] start, int[] goal) 
    {
        return generatedTilePath;
    }
    public List<int[]> Pathfind(float[,] _moveCostGrid, Pawn _pawn, int[] _start, int[] _goal) 
    {
        if (generatedPath == null)
        {
            generatedPath = new List<int[]>();
        }
        else if (generatedPath.Count > 0) 
        {
            generatedPath.RemoveRange(0, generatedPath.Count);
        }

        float[,] pathableGrid = GeneratePath(_moveCostGrid, _pawn, _start, _goal);
        printPath(pathableGrid);

        assemblePath(pathableGrid, _start, _goal);

        //Debug.Log("*** PRINTING PATHABLE GRID ***");
        //printCostGrid(pathableGrid);
        //Debug.Log("*** PRINTED PATHABLE GRID ***");

        return generatedPath;
    }

    /*private void Start()
    {
        int[,] tileGrid = new int[10, 10];
        int[] start = { 4, 4};
        int[] goal =  { 7, 6 };
        int distance = 10;

        

        GeneratePath(tileGrid, start, goal, distance);
    }*/

    private float[,] GeneratePath(float[,] _moveCostGrid, Pawn _pawn, int[] _start, int[] _goal) 
    {
        // Initialize new path cost grid
        float[,] pathableGrid = new float[_moveCostGrid.GetLength(0), _moveCostGrid.GetLength(1)];

        // Populate path cost grid from start towards goal

        int sizeX = pathableGrid.GetLength(0);
        int sizeZ = pathableGrid.GetLength(1);
        float distance = _pawn.speed + 1;

        List<int[]> tileQueue = new List<int[]>();
        tileQueue.Add(_start);

        pathableGrid[_start[0], _start[1]] = 1f;
        addSurroundingTiles(pathableGrid, _moveCostGrid, tileQueue, distance);

        int iterations = 0;

        while (tileQueue.Count != 0 && iterations < 1000) 
        {
            iterations++;
            addSurroundingTiles(pathableGrid, _moveCostGrid, tileQueue, distance);
        }

        return pathableGrid;
    }

    public void GeneratePath(int[,] tileGrid, int[] start, int[] goal, int distance) 
    {
        int sizeX = tileGrid.GetLength(0);
        int sizeY = tileGrid.GetLength(1);

        float[,] moveCostGrid = new float[sizeX, sizeY];

        List<int[]> tileQueue = new List<int[]>();

        moveCostGrid[start[0], start[1]] = 1f;
        tileQueue.Add(start);
        // Offset distance so initial tile is free but not 0 cost to avoid problems
        distance += 1;
        addSurroundingTiles(tileGrid, sizeX, sizeY, distance, moveCostGrid, start, goal, tileQueue);

        int iterations = 0;

        while (tileQueue.Count != 0 && iterations < 100) 
        {
            iterations++;
            Debug.Log("Got this far?");
            addSurroundingTiles(tileGrid, sizeX, sizeY, distance, moveCostGrid, start, goal, tileQueue);
        }

        if (iterations == 100)
            Debug.Log("infinite loop :(");

        assemblePath(moveCostGrid, start, goal);
        //printPath(moveCostGrid);
        printCostGrid(moveCostGrid);
    }

    private void addSurroundingTiles(int[,] tileGrid, int sizeX, int sizeY, int distance, float[,] moveCostGrid, int[] curTile, int[] goalTile, List<int[]> tileQueue) 
    {
        int[] offsetX = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
        int[] offsetY = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int x = tileQueue[0][0] + offsetX[i];
            int y = tileQueue[0][1] + offsetY[i];

            bool isDiagonal = (i % 2 == 1);

            

            // If adjacent tile is within bounds of array
            if (x >= 0 && x < sizeX && y >= 0 && y < sizeY)
            {
                float tileDelta = 1f;

                // if corner tile, add more distance
                if (i % 2 == 1)
                    tileDelta = 1.4f;

                // True tile distance
                tileDelta += moveCostGrid[tileQueue[0][0], tileQueue[0][1]];

                // If not processed, add
                if (tileDelta <= distance && moveCostGrid[x, y] == 0f || moveCostGrid[x,y] > tileDelta)
                {
                    moveCostGrid[x, y] = tileDelta;
                    tileQueue.Add(new int[] { x, y });
                }
            }
        }

        Debug.Log(tileQueue[0][0] + " " + tileQueue[0][1]);
        tileQueue.Remove(tileQueue[0]);
    }

    private void addSurroundingTiles(float[,] _pathableGrid, float[,] _moveCostGrid, List<int[]> _tileQueue, float _distance) 
    {
        int sizeX = _moveCostGrid.GetLength(0);
        int sizeZ = _moveCostGrid.GetLength(1);

        int[] offsetX = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
        int[] offsetY = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int x = _tileQueue[0][0] + offsetX[i];
            int y = _tileQueue[0][1] + offsetY[i];

            // If adjacent tile is within bounds of array
            if (x >= 0 && x < sizeX && y >= 0 && y < sizeZ)
            {
                float tileDelta = 1f;

                // if corner tile, add more distance
                if (i % 2 == 1)
                    tileDelta = 1.4f;

                // Get tile move speed modifier
                tileDelta *= _moveCostGrid[x, y];

                // True tile distance
                tileDelta += _pathableGrid[_tileQueue[0][0], _tileQueue[0][1]];

                // If not processed, add
                if (tileDelta <= _distance && (_pathableGrid[x, y] == 0f || _pathableGrid[x, y] > tileDelta))
                {
                    _pathableGrid[x, y] = tileDelta;
                    _tileQueue.Add(new int[] { x, y });
                }
            }
        }

        _tileQueue.RemoveAt(0);
    }

    private void assemblePath(float[,] moveCostGrid, int[] start, int[] goal) 
    {
        int[] offsetX = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
        int[] offsetY = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

        int curX = -1;
        int curY = -1;

        int iterations = 0;

        generatedPath.Add(goal);
        while (!(curX == start[0] && curY == start[1]) && iterations < 100) 
        {
            iterations++;

            int li = generatedPath.Count - 1;
            curX = generatedPath[li][0];
            curY = generatedPath[li][1];

            float minCost = 0f;

            int lowX = -1;
            int lowY = -1;

            for (int i = 0; i < 8; i++)
            {
                int x = curX + offsetX[i];
                int y = curY + offsetY[i];

                if (inBounds(x, y, moveCostGrid) && moveCostGrid[x,y] != 0 && (minCost == 0f || moveCostGrid[x,y] < minCost))
                {
                    minCost = moveCostGrid[x, y];

                    lowX = x;
                    lowY = y;
                }
            }

            if (lowX != -1 && lowY != -1 && minCost != 0f && !(curX == start[0] && curY == start[1]))
            {
                generatedPath.Add(new int[] { lowX, lowY });
            }
        }

        /*while ((curX != start[0] || curY != start[1]) && iterations < 100)
        {
            iterations++;

            float cost = 0f;
            int lowX = -1;
            int lowY = -1;

            for (int i = 0; i < 8; i++)
            {
                int x = curX + offsetX[i];
                int y = curY + offsetY[i];

                if (inBounds(x, y, moveCostGrid) && moveCostGrid[x,y] < cost && cost == 0f) 
                {
                    cost = moveCostGrid[x, y];
                    lowX = x;
                    lowY = y;
                }
            }

            generatedPath.Add(new int[] { curX, curY });
            curX = lowX;
            curY = lowY;
        }
        if (curX == start[0] && curY == start[1]) 
        {
            generatedPath.Add(new int[] { curX, curY } );
        }*/
    }

    private void printPath(float[,] moveCostGrid) 
    {
        for (int i = 0; i < generatedPath.Count; i++) 
        {
            Debug.Log(i + ": " + generatedPath[i][0] + " " + generatedPath[i][1] + ", Cost: " + moveCostGrid[generatedPath[i][0], generatedPath[i][1]]);
        }
    }

    private void printCostGrid(float[,] moveCostGrid) 
    {
        for (int i = 0; i < moveCostGrid.GetLength(0); i++) 
        {
            string concat = "";

            for (int j = 0; j < moveCostGrid.GetLength(1); j++) 
            {
                concat += moveCostGrid[i, j] + " ";
            }

            Debug.Log(concat);
        }
    }

    private bool inBounds(int _x, int _y, float[,] _grid) 
    {
        if (!(_x >= 0 && _x < _grid.GetLength(0))) 
        {
            return false;
        }
        if (!(_y >= 0 && _y < _grid.GetLength(1)))
        {
            return false;
        }

        return true;
    }
}
