using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Camera m_firstPersonCamera;
    private Camera m_isometricCamera;
    private Camera m_cinematicCamera;
    private Camera m_thirdPersonCamera;
    private FirstPersonCameraController m_firstPersonCameraController;
    private ThirdPersonCameraController m_thirdPersonCameraController;
    private IsometricCameraController m_isometricCameraController;
    private CinematicCameraController m_cinematicCameraController;
    private List<Camera> m_cameras;
    private List<ICameraController> m_cameraControllers;
    private Camera activeCamera;

    private void Awake()
    {
        subscribeToEvents();
    }
    // Use this for initialization
    void Start () {
        
        m_cameras = new List<Camera>();
        m_cameraControllers = new List<ICameraController>();

        GameObject firstPersonCamera = new GameObject();
        firstPersonCamera.transform.position = new Vector3(0, 10, 0);
        firstPersonCamera.name = "FirstPersonCamera";
        m_firstPersonCamera = firstPersonCamera.AddComponent<Camera>();
        m_firstPersonCameraController = firstPersonCamera.AddComponent<FirstPersonCameraController>();
        m_cameraControllers.Add(m_firstPersonCameraController);
        m_cameras.Add(m_firstPersonCamera);
        m_firstPersonCamera.enabled = false;
        m_firstPersonCamera.nearClipPlane = .01f;
        m_firstPersonCamera.farClipPlane = 6000;

        GameObject thirdPersonCamera = new GameObject();
        thirdPersonCamera.name = "ThirdPersonCamera";
        m_thirdPersonCamera = thirdPersonCamera.AddComponent<Camera>();
        m_thirdPersonCameraController = thirdPersonCamera.AddComponent<ThirdPersonCameraController>();
        m_cameraControllers.Add(m_thirdPersonCameraController);
        m_cameras.Add(m_thirdPersonCamera);
        m_thirdPersonCamera.enabled = false;

        GameObject isometricCamera = new GameObject();
        isometricCamera.name = "IsometricCamera";
        m_isometricCamera = isometricCamera.AddComponent<Camera>();
        m_isometricCameraController = isometricCamera.AddComponent<IsometricCameraController>();
        m_cameraControllers.Add(m_isometricCameraController);
        m_cameras.Add(m_isometricCamera);
        m_isometricCamera.enabled = false;

        GameObject cinematicCamera = new GameObject();
        cinematicCamera.name = "CinematicCamera";
        m_cinematicCamera = cinematicCamera.AddComponent<Camera>();
        m_cinematicCameraController = cinematicCamera.AddComponent<CinematicCameraController>();
        m_cameraControllers.Add(m_cinematicCameraController);
        m_cameras.Add(m_cinematicCamera);
        m_cinematicCamera.enabled = false;

        activeCamera = m_firstPersonCamera;
        m_firstPersonCamera.enabled = true;
        
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void subscribeToEvents()
    {
        InputSignals.onPlayerMovement += handleLeftAxis;
        InputSignals.onPlayerRotation += handleRotation;
    }

    private void handleLeftAxis(float xMagnitude, float zMagnitude)
    {
        Debug.Log("Left axis was handled by camera manager.");
        activeCamera.GetComponent<ICameraController>().processLeftAxis(xMagnitude, zMagnitude);
    }

    private void handleRotation(float xMagnitude, float yMagnitude)
    {
        Debug.Log("Right axis was handled by camera manager.");
        activeCamera.GetComponent<ICameraController>().processRightAxis(xMagnitude, yMagnitude);
    }
}
