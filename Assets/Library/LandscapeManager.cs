using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {

    /**
     * @brief   The builder class which LandscapeManager uses to get Landscapes
     */
    private PlaneBuilder m_planeBuilder;

    // Use this for initialization
    void Start () {
        m_planeBuilder = new PlaneBuilder();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
