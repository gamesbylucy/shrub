using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    /**
     * @brief Member first person camera class used by camera manager.
     */
    private Camera m_firstPersonCamera;

    /**
     * @brief Member isometric camera class used by camera manager.
     */
    private Camera m_isometricCamera;

    /**
     * @brief Member cinematic camera class used by camera manager.
     */
    private Camera m_cinematicCamera;

    /**
     * @brief Member third person camera class used by camera manager.
     */
    private Camera m_thirdPersonCamera;

    /**
     * @brief The list of all cameras.
     */
    private List<Camera> m_cameras;

    /**
     * @brief The currently active camera.
     */
    private Camera activeCamera;

    // Use this for initialization
    void Start () {
        GameObject firstPersonCamera = new GameObject();
        firstPersonCamera.name = "FirstPersonCamera";
        m_firstPersonCamera = firstPersonCamera.AddComponent<Camera>();
        m_cameras.Add(m_firstPersonCamera);

        GameObject thirdPersonCamera = new GameObject();
        thirdPersonCamera.name = "ThirdPersonCamera";
        m_thirdPersonCamera = thirdPersonCamera.AddComponent<Camera>();
        m_cameras.Add(m_thirdPersonCamera);

        GameObject isometricCamera = new GameObject();
        isometricCamera.name = "IsometricCamera";
        m_isometricCamera = isometricCamera.AddComponent<Camera>();
        m_cameras.Add(m_isometricCamera);

        GameObject cinematicCamera = new GameObject();
        cinematicCamera.name = "CinematicCamera";
        m_cinematicCamera = cinematicCamera.AddComponent<Camera>();
        m_cameras.Add(m_cinematicCamera);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Subscribes the manager to events in the EventSignals messenger class. For a detailed description
     * of events, see the EventSignals class.
     * 
     * @return  Returns true if all events were successfully subscribed to. Otherwise returns false.
     */
    public bool subscribeToEvents()
    {
        //method stub
        return true;
    }
}
