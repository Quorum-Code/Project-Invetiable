using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCursorController : MonoBehaviour
{
    private Camera MainCam;

    private Ray CameraToCursor;
    private RaycastHit CursoredHit;
    private GameObject CursoredObject;
    private Tile CursoredTile;

    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CameraToCursor = MainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(CameraToCursor, out CursoredHit)) 
        {
            CursoredObject = CursoredHit.collider.gameObject;

            CursoredTile = CursoredObject.GetComponent<Tile>();
            //if ()
            {

            }
        }
    }
}
