using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public KeyCode moveForward;
    public KeyCode moveBackward;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode rotateLeft;
    public KeyCode rotateRight;
    public KeyCode rotateUp;
    public KeyCode rotateDown;
    public KeyCode highlight;
    public KeyCode toggleSelect;

    private KBMInput m_kbmInput;

	// Use this for initialization
	void Start () {
        m_kbmInput = new KBMInput();
        m_kbmInput.isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
