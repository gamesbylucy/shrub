using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {
    public int landscapeWidth;
    public int landscapeHeight;
    /**
     * @brief   The builder class which LandscapeManager uses to get Landscapes
     */
    private LandscapeBuilder m_landscapeBuilder;

    // Use this for initialization
    void Start () {
        m_landscapeBuilder = gameObject.AddComponent<LandscapeBuilder>();
        m_landscapeBuilder.build(landscapeWidth, landscapeHeight);
        m_landscapeBuilder.nodeReport();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
