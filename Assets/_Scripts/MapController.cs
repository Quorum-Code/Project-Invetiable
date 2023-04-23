using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapController : MonoBehaviour
{
    [SerializeField] GameObject prefabFullWall;
    [SerializeField] GameObject prefabHalfWall;

    [SerializeField] GameObject prefabFullCover;
    [SerializeField] GameObject prefabHalfCover;

    Tile[,] tiles;
    Wall[,] walls;

    [SerializeField] int mapSize = 5;
    float tileSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateMap() 
    {
        int instCounter = 0;

        tiles = new Tile[mapSize, mapSize];
        walls = new Wall[mapSize + 1, mapSize * 2 + 1];

        float yOffsetTile = 0f;
        float yOffsetWall = .5f;

        GameObject tmp;
        float r = 0f;

        GenerateRoom(3, 3, 6, 6);
        GenerateRoom(8, 10, 12, 13);
        GenerateRoom(20, 3, 25, 6);
        GenerateRoom(8, 5, 12, 9);

        //Visualize tiles
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i,j] != null) 
                {
                    //tmp = Instantiate(prefab1, new Vector3(i * tileSize, yOffsetTile, j * tileSize), Quaternion.Euler(0, 0, 0), this.gameObject.transform);
                    instCounter++;
                }
            }
        }

        // Visualize walls
        for (int i = 0; i < walls.GetLength(0); i++) 
        {
            for (int j = 0; j < walls.GetLength(1); j++) 
            {
                if (walls[i,j] != null) 
                {
                    Vector3 wallPos = new Vector3(i * tileSize - (j % 2) * tileSize / 2, yOffsetWall, j * tileSize / 2f - 0.5f);
                    Quaternion wallAngle = Quaternion.Euler(0, (j % 2) * 90f, 0);

                    tmp = Instantiate(prefabFullWall, wallPos, wallAngle, this.gameObject.transform);

                    instCounter++;
                }
            }
        }

        Debug.Log(instCounter);
    }

    private void TryGenerateRoom() 
    {
        
    }

    /// <summary>
    /// Generates room within parameters
    /// </summary>
    private void GenerateRoom(int x1, int z1, int x2, int z2)
    {

        if (x1 < 0 || x1 >= walls.GetLength(0) || x2 < 0 || x2 >= walls.GetLength(0) || z1 < 0 || z1 >= walls.GetLength(1) || z2 < 0 || z2 >= walls.GetLength(1)) 
        {
            return;
        }

        // Choose index to door
        int totalWalls = (x2 + 1 - x1) * 2 + (z2 + 1 - z1) * 2;
        int wallIndex = 0;
        int doorIndex = Random.Range(0, totalWalls);

        // Visualize floor
        for (int i = x1; i <= x2; i++) 
        {
            for (int j = z1; j <= z2; j++) 
            {
                tiles[i, j] = new Tile(true, 1f);
            }
        }

        // Normalize coords for wall grid
        z1 = z1 * 2 + 1;
        z2 = z2 * 2 + 1;

        // Generate top/bottom wall
        for (int i = x1; i <= x2; i++)
        {
            if (wallIndex == doorIndex)
            {
                wallIndex++;
            }
            else 
            {
                walls[i, z1 - 1] = new Wall(false, 1);
                wallIndex++;
            }

            if (wallIndex == doorIndex)
            {
                wallIndex++;
            }
            else
            {
                walls[i, z2 + 1] = new Wall(false, 1);
                wallIndex++;
            }
        }

        // Generate left/right walls
        for (int i = z1; i <= z2; i += 2)
        {
            if (wallIndex == doorIndex)
            {
                wallIndex++;
            }
            else
            {
                walls[x1, i] = new Wall(false, 1);
                wallIndex++;
            }

            if (wallIndex == doorIndex)
            {
                wallIndex++;
            }
            else
            {
                walls[x2 + 1, i] = new Wall(false, 1);
                wallIndex++;
            }
        }
    }

    private bool isTilePassable(Vector3 a, Vector3 b) 
    {
        // Check if -a- within bounds
        if (a.x < 0 || a.x > tiles.GetLength(0) || a.z < 0 || a.z > tiles.GetLength(1)) 
        {
            return false;
        }

        // Check if -b- within bounds
        if (b.x < 0 || b.x > tiles.GetLength(0) || b.z < 0 || b.z > tiles.GetLength(1))
        {
            return false;
        }

        // Check if b isWalkable




        return true;
    }
}
