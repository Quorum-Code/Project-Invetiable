using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMapGenerator : MonoBehaviour
{
    Camera MainCam;

    GameObject[,] tiles;
    float[,] MoveCostGrid;

    [SerializeField] GameObject TileParent;

    [SerializeField] GameObject TilePrefab;

    [SerializeField] bool Generate = false;

    [SerializeField] int GridW;
    [SerializeField] int GridL;

    float TileSize = 1f;

    [SerializeField] GameObject PlayerPawnPrefab;
    [SerializeField] GameObject FullCoverPrefab;
    [SerializeField] GameObject HalfCoverPrefab;
    GameObject playerPawn;
    Pawn player;

    [SerializeField] Pathfinder pf;

    List<int[]> path = new List<int[]>();

    private void Start()
    {
        MainCam = Camera.main;

        InitializeMoveCostGrid();
        GenerateSimpleMap();
        InstanceCharacter();

        

        //ColorTilesFromCostGrid();
    }

    private void FixedUpdate()
    {
        if (Generate) 
        {
            Generate = false;
            GenerateSimpleMap();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject g = hit.collider.gameObject;

                Tile t = g.GetComponent<Tile>();
                if (t != null) 
                {
                    Debug.Log("*** ATTEMPTING MOVE ***");
                    Debug.Log("From: " + player.x + " " + player.z + " To: " + t.x + " " + t.z);

                    if (TryMovePawn(new int[] { player.x, player.z }, new int[] { t.x, t.z })) 
                    {
                        MovePlayer(new int[] { t.x, t.z });
                    }
                    Debug.Log("******** END *********");
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            // Get tile coords
            Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject g = hit.collider.gameObject;

                Tile t = g.GetComponent<Tile>();
                if (t != null)
                {
                    // Change tile color
                    g.GetComponent<MeshRenderer>().material.color = Color.black;

                    // Change move value
                    int x = t.x;
                    int z = t.z;
                    MoveCostGrid[x,z] = 99f;
                }
            }
        }
    }

    private bool TryMovePawn(int[] start, int[] goal) 
    {
        // Check pathfinder exists
        if (pf == null)
            return false;

        // Check pathfinder initialized
        if (!pf.initialized)
        {
            pf.initialize(MoveCostGrid);
        }

        if (MoveCostGrid == null) 
        {
            
        }

        // Try to get path
        path = pf.Pathfind(MoveCostGrid, player, start, goal);

        // Check that path list is not null
        if (path.Count == 0) 
        {
            return false;
        }

        //Debug.Log("Path Count: " + path.Count);

        // Color path on grid
        resetGridColor();
        colorGridPath(path);

        // Move player to target square

        return true;
    }

    private void MovePlayer(int[] tile) 
    {
        playerPawn.transform.position = new Vector3(tile[0], 0, tile[1]);
        player.setPosition(tile);
    }

    private void GenerateSimpleMap() 
    {
        // Calculate and store grid offset
        float xOffset = (GridW * TileSize) / 2f;
        float zOffset = (GridL * TileSize) / 2f;

        tiles = new GameObject[GridW, GridL];

        float chance;

        for (int i = 0; i < GridW; i++) 
        {
            for (int j = 0; j < GridL; j++) 
            {
                GameObject g = Instantiate(TilePrefab);

                tiles[i, j] = g;

                g.transform.SetParent(TileParent.transform);
                g.transform.position = new Vector3(i * TileSize /*- (xOffset - TileSize / 2)*/, 0, j * TileSize /*- (zOffset - TileSize / 2)*/);
                Tile t = g.GetComponent<Tile>();
                t.setCoords(new Vector3(i, 0, j));
                //t.setX(i);
                //t.setZ(j);

                if ((i + j) % 2 == 1)
                {
                    //g.GetComponentInChildren<MeshRenderer>().material.SetColor("Grey", Color.grey);
                    g.GetComponent<MeshRenderer>().material.color = Color.gray;
                }

                else
                {
                    //g.GetComponentInChildren<MeshRenderer>().material.SetColor("White", Color.white);
                    g.GetComponent<MeshRenderer>().material.color = Color.white;
                }


                // Determine if piece of random cover should spawn here
                chance = Random.Range(0f, 1f);

                // Spawn full
                if (chance > .97f)
                {
                    GameObject c = Instantiate(FullCoverPrefab);
                    c.transform.position = new Vector3(i * TileSize, .5f, j * TileSize);
                    MoveCostGrid[i, j] *= 99f;
                }
                // Spawn half
                else if (chance > .93f) 
                {
                    GameObject c = Instantiate(HalfCoverPrefab);
                    c.transform.position = new Vector3(i * TileSize, .5f, j * TileSize);
                    MoveCostGrid[i, j] *= 99f;
                }
            }
        }
    }

    public void ColorTilesFromCostGrid(float[,] moveCostGrid) 
    {
        for (int i = 0; i < moveCostGrid.GetLength(0); i++) 
        {
            for (int j = 0; j < moveCostGrid.GetLength(1); j++) 
            {
                tiles[i, j].GetComponent<MeshRenderer>().material.color = new Color(moveCostGrid[i,j] * 5, 0, 0);
            }
        }
    }

    // Add player character object to game board
    private void InstanceCharacter() 
    {
        player = new Pawn(1, 0, 1, 10);
        //player = new Pawn(GridW / 2, 0, GridL / 2, 10);
        playerPawn = Instantiate(PlayerPawnPrefab);

        playerPawn.transform.position = new Vector3(player.x, player.y, player.z);
    }

    private void InitializeMoveCostGrid() 
    {
        MoveCostGrid = new float[GridW, GridL];

        for (int i = 0; i < GridW; i++) 
        {
            for (int j = 0; j < GridL; j++) 
            {
                MoveCostGrid[i, j] = 1f;
            }
        }
    }

    private void resetGridColor() 
    {
        for (int i = 0; i < tiles.GetLength(0); i++) 
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (MoveCostGrid[i, j] == 99f)
                {
                    tiles[i, j].GetComponent<MeshRenderer>().material.color = Color.black;
                }
                else if ((i + j) % 2 == 1)
                {
                    tiles[i, j].GetComponent<MeshRenderer>().material.color = Color.gray;
                }
                else
                {
                    tiles[i, j].GetComponent<MeshRenderer>().material.color = Color.white;
                }

            }
        }
    }

    private void colorGridPath(List<int[]> _path) 
    {
        for (int i = 0; i < _path.Count; i++) 
        {
            tiles[_path[i][0], _path[i][1]].GetComponent<MeshRenderer>().material.color = Color.blue;
            if (i == _path.Count - 1)
                tiles[_path[i][0], _path[i][1]].GetComponent<MeshRenderer>().material.color = Color.cyan;
        }
    }

    private void meshTest() 
    {
        Mesh mesh = new Mesh();
    }
}
