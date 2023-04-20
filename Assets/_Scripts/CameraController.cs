using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] GameObject camBase;

    [SerializeField] float speed = 3f;

    float currZoom = 1f;
    float lastZoom = 0f;
    float timeZoom = .4f;
    float zoomTimer = 0f;

    Vector3 lastPos;
    Vector3 targPos;
    Vector3 lastRot;
    Vector3 targRot;

    [SerializeField] Vector3 posIn;
    [SerializeField] Vector3 rotIn;
    [SerializeField] Vector3 posOut;
    [SerializeField] Vector3 rotOut;

    float oldY;
    float targetY;

    float deltaRot = 0;
    float rotSpeed = 3f;

    float deltaZoom = 0f;
    float zoom = 0f;
    [SerializeField] float zoomSpeed = 25f;

    float deltaX;
    float deltaY;
    float deltaZ;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // Camera Position Controls
        deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        speed = Mathf.Abs(mainCam.transform.localPosition.z / 3f);
        camBase.gameObject.transform.Translate(new Vector3(deltaX, 0, deltaY), Space.Self);

        // Camera Rotation Controls

        CameraZoom();

        // Cam Speed Relative to Camera Zoom
        speed = Mathf.Abs(mainCam.transform.localPosition.z / 5f);

        // Zoom In/Out
        //deltaZ = Input.GetAxis("Mouse ScrollWheel") * 25f;
        //zoom += deltaZ;
        //Debug.Log(deltaZ);

        // Rotate Freely
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift)) { }
        else if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift)) { }
        // Rotate Snap
        else if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (deltaRot > 0f)
                deltaRot = deltaRot % 90f;
            deltaRot -= 90f;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if(deltaRot < 0f)
                deltaRot = deltaRot % 90f;
            deltaRot += 90f;
        }

        // Camera Rotator
        if (deltaRot != 0f) 
        {
            float change = Mathf.Min(Mathf.Abs(deltaRot), 90f * Time.deltaTime * rotSpeed) * Mathf.Sign(deltaRot);
            camBase.transform.eulerAngles = new Vector3(camBase.transform.eulerAngles.x, camBase.transform.eulerAngles.y - change, camBase.transform.eulerAngles.z);
            deltaRot -= change;
        }

        // Camera Zoom
        if (zoom != 0) 
        {
            float change = Mathf.Min(Mathf.Abs(zoom), 3f * Time.deltaTime * zoomSpeed) * Mathf.Sign(zoom);
            zoom -= change;

            if (mainCam.transform.localPosition.z - zoom > -8f)
                zoom = 0;

            mainCam.transform.localPosition = new Vector3(mainCam.transform.localPosition.x, mainCam.transform.localPosition.y, Mathf.Min(-8, mainCam.transform.localPosition.z + change));
        }
    }

    // Camera Zoom Controls
    private void CameraZoom()
    {
        // Camera Zoom Starter
        deltaZoom = -Input.GetAxis("Mouse ScrollWheel");
        lastZoom = currZoom;
        currZoom += deltaZoom;
        currZoom = Mathf.Clamp01(currZoom);

        if (deltaZoom != 0 && lastZoom != currZoom)
        {
            // Store old pos/rot
            lastPos = this.transform.localPosition;
            lastRot = this.transform.localEulerAngles;

            // Calc target pos/rot
            targPos = new Vector3(0f, posIn.y + currZoom * (posOut.y - posIn.y), posIn.z + currZoom * (posOut.z - posIn.z));
            targRot = new Vector3(rotIn.x + currZoom * (rotOut.x - rotIn.x), 0f, 0f);

            // Set timer to timeZoom
            zoomTimer = timeZoom;
        }

        // Camera Zoom Update
        if (zoomTimer != 0)
        {
            // Update timeZoom timer
            zoomTimer -= Time.deltaTime;
            if(zoomTimer < 0f)
                zoomTimer = 0f;

            // Smoothstep position
            this.transform.localPosition = new Vector3(0f, Mathf.SmoothStep(lastPos.y, targPos.y, 1f - zoomTimer / timeZoom), Mathf.SmoothStep(lastPos.z, targPos.z, 1f - zoomTimer / timeZoom));

            // Smoothstep rotation
            this.transform.localEulerAngles = new Vector3(Mathf.SmoothStep(lastRot.x, targRot.x, 1f - zoomTimer / timeZoom), 0, 0);
        }
    }

    private void CameraRotate() 
    {
        if (Input.GetButtonDown()) 
        {

        }
    }
}
