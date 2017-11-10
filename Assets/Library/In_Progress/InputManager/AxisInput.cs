using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisInput : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void processLeftAxis(float horizontalMagnitude, float verticalMagnitude)
    {
        /**
         * Depending on the magnitude of the two values, trigger a different input event.
         */
        //Debug.Log("Horz movement: " + horizontalMagnitude + ", Vert movement: " + verticalMagnitude);
        InputSignals.playerMovement(horizontalMagnitude, verticalMagnitude);
    }

    public void processRightAxis(float horizontalMagnitude, float verticalMagnitude)
    {
        InputSignals.playerRotation(horizontalMagnitude, verticalMagnitude);
    }
}
