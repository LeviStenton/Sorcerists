using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameController gameController;
    PlayerController playerController;
    private Vector3 camPosition;
    private Vector3 newCamPosition;
    private float camSize;
    //public GameObject camTarget;
    public GameObject player;
    public GameObject mainCam;

    public float camSpeed;

    public float minZoom;
    public float maxZoom;
    public float zoomSpeed;
    public float camLerpSpeed;
    public bool trackingPlayerCam;

    private Camera myCam;

    // Use this for initialization
    void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        myCam = GetComponent<Camera>();
        camSize = myCam.orthographicSize;
        camPosition = transform.position;
        trackingPlayerCam = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckBoundary();
        CameraZoom();
        CameraMovement();
        CheckBoundary();

        if (Input.GetButtonDown("TestButton"))
        {
            Debug.Log("Testing");
            trackingPlayerCam = true;
        }
        if (trackingPlayerCam)
            focusTarget(player);
        else if (!trackingPlayerCam)
            focusTarget(mainCam);

    }

    void CameraMovement()
    {
        float moveHorizontal = Input.GetAxis("CameraHorizontal");        
        float moveVertical = Input.GetAxis("CameraVertical");        
        Vector3 movement = new Vector3(moveHorizontal, moveVertical);
        camPosition += movement * camSpeed * Time.deltaTime;
        transform.position = camPosition; 
        if(Input.GetButtonDown("CameraHorizontal") | Input.GetButtonDown("CameraVertical"))
        {
            trackingPlayerCam = false;
        }
    }

    void CheckBoundary()
    {
        float aspectRatio = Screen.width / Screen.height;
        if (camPosition.x - ((myCam.orthographicSize / 2) * aspectRatio) <= -gameController.xBoundary)
            camPosition.x = -gameController.xBoundary + ((myCam.orthographicSize / 2) * aspectRatio);
        else if (camPosition.x + ((myCam.orthographicSize / 2) * aspectRatio) >= gameController.xBoundary)
            camPosition.x = gameController.xBoundary - ((myCam.orthographicSize / 2) * aspectRatio);    
        
        if (camPosition.y - (myCam.orthographicSize / 2) <= -gameController.yBoundary)
            camPosition.y = -gameController.yBoundary + (myCam.orthographicSize / 2);
        else if (camPosition.y + (myCam.orthographicSize / 2) >= gameController.yBoundary)
            camPosition.y = gameController.yBoundary - (myCam.orthographicSize / 2);        
    }

    void CameraZoom()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        camSize -= zoomSpeed * scrollWheel;

        if(camSize >= maxZoom)
        {
            camSize = maxZoom;
        }
        if (camSize <= minZoom)
        {
            camSize = minZoom;
        }

        myCam.orthographicSize = camSize;
    }

    void focusTarget(GameObject camTarget)
    {
        newCamPosition = new Vector3(camTarget.transform.position.x, camTarget.transform.position.y, mainCam.transform.position.z);
        camPosition = Vector3.Lerp(camPosition, newCamPosition, Time.deltaTime * camLerpSpeed);
    }

    /*IEnumerator focusTarget(GameObject camTarget, float delayTime)
    {        
        yield return new WaitForSeconds(delayTime); 
        float startTime = Time.time; 
        while (Time.time - startTime <= 1)
        {
            newCamPosition = new Vector3(camTarget.transform.position.x, camTarget.transform.position.y, mainCam.transform.position.z);
            transform.position = Vector3.Lerp(camPosition, newCamPosition, Time.time - startTime);
            yield return 1; 
        }
    }*/
}
