using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// LevelController is responsible for generating and populating tiles, 
/// positioning characters, and passing that information.
/// 
/// May convert CharacterSheets into pawns to be placed in world space.
/// </summary>
public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject fullCoverPrefab;
    [SerializeField] GameObject halfCoverPrefab;


    [SerializeField] int width = 10;
    [SerializeField] int length = 10;
    [SerializeField] float tileSize = 1f;


    [SerializeField] Pathfinder PF;

    private GameObject tileParent;
    private Tile[,] levelTiles;
    private List<Tile> pathTiles;

    private void Start()
    {
        GenerateLevel();
    }

    public void Initialize()
    {
        levelTiles = new Tile[length,width];
        GenerateLevel();
    }

    public void Update()
    {
        
    }

    /// <summary>
    /// Populates tiles
    /// </summary>
    public void GenerateLevel() 
    {
        // Calculate and store grid offset
        float xOffset = (width * tileSize) / 2f;
        float zOffset = (length * tileSize) / 2f;

        levelTiles = new Tile[length, width];
        tileParent = new GameObject("tileParent");
        tileParent.transform.SetParent(this.gameObject.transform);

        //float chance;

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject g = Instantiate(tilePrefab);

                levelTiles[i, j] = g.GetComponent<Tile>();

                g.transform.SetParent(tileParent.transform);
                g.transform.position = new Vector3(j * tileSize /*- (xOffset - TileSize / 2)*/, 0, i * tileSize /*- (zOffset - TileSize / 2)*/);
                Tile t = g.GetComponent<Tile>();
                t.setCoords(new Vector3(j, 0, i));
                //t.setX(i);
                //t.setZ(j);

                if ((i + j) % 2 == 1)
                {
                    g.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
                }

                else
                {
                    g.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                }


                // Determine if piece of random cover should spawn here
                //chance = Random.Range(0f, 1f);

                /*
                // Spawn full
                if (chance > .97f)
                {
                    GameObject c = Instantiate(fullCoverPrefab);
                    c.transform.position = new Vector3(i * tileSize, .5f, j * tileSize);
                    //levelTiles[i, j].isPassable = false;
                }
                // Spawn half
                else if (chance > .93f)
                {
                    GameObject c = Instantiate(halfCoverPrefab);
                    c.transform.position = new Vector3(i * tileSize, .5f, j * tileSize);
                    //MoveCostGrid[i, j] *= 99f;
                }
                */
            }
        }
    }


    /// <summary>
    /// Attempts to move pawn from start coords to goal coords
    /// </summary>
    /// <param name="pawn"></param>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    public bool TryMove(Pawn pawn, int[] start, int[] goal) 
    {
        bool isMoved = false;

        // Check pathfinder exists
        if (PF == null)
            return false;

        // Check pathfinder initialized
        if (!PF.initialized)
        {
            PF.initialize(levelTiles);
        }

        // Get pathtiles from PF
        pathTiles = PF.Pathfind(pawn, start, goal);

        if (pathTiles.Count == 0)
            return false;

        //

        return isMoved;
    }
}
