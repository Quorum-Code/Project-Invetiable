using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGenerator
{
    public char[,] GenerateLayout(int size, int seeds) 
    {
        char[,] layout = new char[size, size];
        Point[] seedPoints = new Point[seeds];

        // Default layout
        initLayout(layout, size, '0');

        // Seed points
        seedLayout(layout, seedPoints, '1');

        // Carve paths
        carvePaths(layout, seedPoints, '1', '0');

        return layout;
    }

    private void initLayout(char[,] layout, int size, char def) 
    {
        for (int i = 0; i < size; i++) 
        {
            for (int j = 0; j < size; j++) 
            {
                layout[i, j] = def;
            }
        }
    }

    private void seedLayout(char[,] layout, Point[] seedPoints, char seed) 
    {
        Point point;

        int range = 3;
        int points = seedPoints.GetLength(0);

        for (int i = 0, c = 0; c < points && i < 100; i++)
        {
            int rx = Random.Range(0, layout.GetLength(1));
            int rz = Random.Range(0, layout.GetLength(0));

            point = new Point(rx, 0, rz);

            if (hasBreath(layout, point, range, '0'))
            {
                layout[point.z, point.x] = seed;
                seedPoints[c] = new Point(point.x, point.y, point.z);
                c++;
            }
        }
    }

    private bool hasBreath(char[,] layout, Point point, int range, char empty) 
    {
        Point checkPoint;

        for (int i = point.y - range; i <= point.y + range; i++) 
        {
            for (int j = point.x - range; j <= point.x + range; j++)
            {
                checkPoint = new Point(j, 0, i);

                if (isInBounds(layout, checkPoint)) 
                {
                    if(layout[i, j] != empty)
                        return false;
                }
            }
        }

        return true;
    }

    private bool isInBounds(char[,] layout, Point point) 
    {
        if (point.x >= 0 && point.z >= 0 && point.x < layout.GetLength(1)-1 && point.y < layout.GetLength(0)-1) 
        {
            return true;
        }
        return false;
    }

    private void carvePaths(char[,] layout, Point[] seedPoints, char path, char open) 
    {
        float[,] costs = new float[layout.GetLength(0), layout.GetLength(1)];

        for (int i = 0; i < seedPoints.GetLength(0); i++) 
        {
            connectToNearestPoint(layout, seedPoints[i]);
        }
    }

    private void connectToNearestPoint(char[,] layout, Point start) 
    {
        List<Point> queuedPoints = new List<Point>();
        float[,] costs = new float[layout.GetLength(0), layout.GetLength(1)];
        
        Point currPoint = new Point();
        Point found = new Point(-1, -1, -1);

        queuedPoints.Add(start);
        costs[start.z, start.x] = 1f;

        bool isSeeking = true;
        int c = 0;

        int[] zs = { 1, 0, -1, 0 };
        int[] xs = { 0, 1, 0, -1 };

        // Djikstra's to find nearest point 
        while (isSeeking && c < 9999 && queuedPoints.Count > 0) 
        {
            c++;

            currPoint = queuedPoints[0];

            for (int i = 0; i < 4; i++) 
            {
                int nz = currPoint.z + zs[i];
                int nx = currPoint.x + xs[i];

                // Inbounds check
                if (nz > 0 && nz < layout.GetLength(0) && nx > 0 && nx < layout.GetLength(1)) 
                {
                    if (costs[nz, nx] == 0f || costs[currPoint.z, currPoint.x] + 1 < costs[nz, nx])
                    {
                        if (layout[nz, nx] == '1')
                        {
                            isSeeking = false;
                            found = new Point(nx, 0, nz);
                        }

                        // Assign neighbor costs
                        costs[nz, nx] = costs[currPoint.z, currPoint.x] + 1;
                        // Add neighbor to queue
                        queuedPoints.Add(new Point(nx, 0, nz));
                    }
                } 
            }

            queuedPoints.RemoveAt(0);
        }

        Debug.Log("Start: " + start.x + " " + start.z + " | Found: " + found.x + " " + found.z + " | Count: " + c);

        #region Utilize Pathing

        // Utilize path
        bool isPathing = true;
        c = 0;

        if(found.x == -1 || found.z  == -1)
            isPathing = false;

        while (isPathing) 
        {
            layout[found.z, found.x] = '1';

            found = findLowestNeighbor(costs, found);

            if (found.x == start.x && found.z == start.z)
                isPathing = false;

            if (c > 250)
                isPathing = false;

            c++;
        }

        #endregion
    }

    private Point findLowestNeighbor(float[,] costs, Point start) 
    {
        int[] zs = { 1, 0, -1, 0};
        int[] xs = { 0, 1, 0, -1};

        Point point = new Point();
        Point neighbor = new Point();
        
        float lowest = float.MaxValue;
        float neighborCost = float.MaxValue;

        for (int i = 0; i < 4; i++) 
        {
            neighbor.z = zs[i] + start.z;
            neighbor.x = xs[i] + start.x;

            // TODO Transform index to neighboring point
            //   Trouble figuring out negatives
            // neighbor.y = start.y + (y + 1) % 2;
            // neighbor.x = start.x + x % 2;

            if (neighbor.z > 0 && neighbor.x > 0 && neighbor.z < costs.GetLength(0) && neighbor.x < costs.GetLength(1)) 
            {
                neighborCost = costs[neighbor.z, neighbor.x];

                if (neighborCost != 0 && lowest > neighborCost) 
                {
                    lowest = neighborCost;

                    point.z = neighbor.z;
                    point.x = neighbor.x;
                }
            }
        }

        return point;
    }

    private void connectPoints(char[,] layout, Point pointA, Point pointB)
    {

    }
}
public struct Point
{
    public int x;
    public int y;
    public int z;

    public Point(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
}