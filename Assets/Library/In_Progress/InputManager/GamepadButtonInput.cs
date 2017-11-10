using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadButtonInput : MonoBehaviour, IButtonInput {

    public KeyCode interact
    {
        set
        {
            m_interact = value;
        }
    }

    private KeyCode m_interact;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void processInputDown(KeyCode input)
    {
        if (input == m_interact)
        {
            Debug.Log("Interact was pressed on the keyboard.");
            /**
             * Fire event "OnPlayerInteract" here.
             */
        }
    }

    public void processInputHeld(KeyCode input)
    {

    }

    public void processInputUp(KeyCode input)
    {

    }
}
