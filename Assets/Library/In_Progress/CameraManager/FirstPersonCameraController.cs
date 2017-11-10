using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour, ICameraController {

    public float cameraSpeed;

    private float m_xMovement;
    private float m_zMovement;
    private float m_xRotation;
    private float m_yRotation;

    private void Awake()
    {

    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * m_zMovement;
        transform.position += transform.right * m_xMovement;
        transform.Rotate(Vector3.right, m_xRotation);
        transform.Rotate(Vector3.up, m_yRotation, 0);
        
	}

    public void processLeftAxis(float xMagnitude, float zMagnitude)
    {
        m_xMovement = xMagnitude;
        m_zMovement = zMagnitude;
    }

    public void processRightAxis(float xMagnitude, float yMagnitude)
    {
        m_xRotation = -yMagnitude;
        m_yRotation = xMagnitude;
    }
}
