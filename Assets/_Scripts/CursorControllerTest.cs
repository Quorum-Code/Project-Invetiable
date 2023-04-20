using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControllerTest : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] GameObject highlighter;
    [SerializeField] GameObject sphereNormalizer;

    // Update Variables
    int x, z;

    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        if (highlighter == null) 
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            sphereNormalizer.transform.position = hit.point + hit.normal * sphereNormalizer.transform.localScale.x;

            if (x != (int) hit.point.x || z != (int) hit.point.z) 
            {
                x = (int) hit.point.x;
                z = (int)hit.point.z;

                highlighter.transform.position = new Vector3(x + .5f, .5f, z + .5f);
            }
        }
    }
}
