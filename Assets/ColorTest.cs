using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        //MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        //if (mr == null) Debug.Log("No renderer");
        //mr.material.SetColor("Red", Color.red);
        //mr.material.color = Color.red;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
