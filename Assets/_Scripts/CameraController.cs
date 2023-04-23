using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] GameObject camBase;

    [SerializeField] float speed;

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

    float deltaZoom = 0f;
    [SerializeField] float zoomSpeed = 25f;

    float deltaX;
    float deltaY;

    // Camera Rotator
    private float startYAngle = 45f;
    private bool isRotting = false;

    // Start is called before the first frame update
    void Start()
    {
        InitRotator();

        mainCam = Camera.main;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
        CameraZoom();
        CameraRotate();
    }
    private void CameraMove() 
    {
        // Cam Speed Relative to Camera Zoom
        speed = 5f + Mathf.Abs(mainCam.transform.localPosition.z / 10f);

        // Camera Position Controls
        deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        speed = Mathf.Abs(mainCam.transform.localPosition.z / 3f);
        camBase.gameObject.transform.Translate(new Vector3(deltaX, 0, deltaY), Space.Self);
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

    private void InitRotator() 
    {
        camBase.transform.eulerAngles = new Vector3(0f, startYAngle, 0f);
    }

    // Update function for camera rotation input
    private void CameraRotate() 
    {
        // Rotate right
        if (Input.GetKeyDown(KeyCode.E) && !isRotting)
        {
            isRotting = true;
            StartCoroutine(CameraRotator(-90f));
        }
        // Rotate left
        else if (Input.GetKeyDown(KeyCode.Q) && !isRotting) 
        {
            isRotting = true;
            StartCoroutine(CameraRotator(90f));
        }
    }

    // Coroutine to smooth camera rotate
    IEnumerator CameraRotator(float target) 
    {
        float start = camBase.transform.localEulerAngles.y;

        float totTime = .4f;
        float curTime = 0f;

        while (curTime < totTime) 
        {
            curTime += Time.deltaTime;

            camBase.transform.localEulerAngles = new Vector3(0, Mathf.SmoothStep(start, start + target, curTime / totTime), 0);

            yield return null; 
        }

        isRotting = false;
    }
}
