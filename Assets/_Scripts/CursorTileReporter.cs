using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTileReporter : MonoBehaviour
{
    Camera MainCam;
    Tile LastTile;

    private void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            GameObject g = hit.collider.gameObject;

            Tile t = g.GetComponent<Tile>();
            if (t != null && t != LastTile) 
            {
                LastTile = t;
                Debug.Log(t.x + " " + t.z);
            }
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            // Report tile clicked on to map controller
        }
    }
}
