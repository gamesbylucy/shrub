using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    /**
     * @brief   Member first person camera class used by camera manager.
     */
    FirstPersonCamera m_firstPersonCamera;

    /**
     * @brief   Member isometric camera class used by camera manager.
     */
    IsometricCamera m_isometricCamera;

    /**
     * @brief   Member cinematic camera class used by camera manager.
     */
    CinematicCamera m_cinematicCamera;

    /**
     * @brief   Member third person camera class used by camera manager.
     */
    ThirdPersonCamera m_thirdPersonCamera;

    // Use this for initialization
    void Start () {
        m_firstPersonCamera = new FirstPersonCamera();
        m_thirdPersonCamera = new ThirdPersonCamera();
        m_isometricCamera = new IsometricCamera();
        m_cinematicCamera = new CinematicCamera();
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
