using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {

    /**
     * @brief   The builder class which LandscapeManager uses to get Landscapes
     */
    private LandscapeBuilder m_landscapeBuilder;

    // Use this for initialization
    void Start () {
        m_landscapeBuilder = new LandscapeBuilder();
        m_landscapeBuilder.generate();
        m_landscapeBuilder.nodeReport();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
