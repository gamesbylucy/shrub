using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {
    public int landscapeSize;
    public float baseSeedProbability;
    public float stepSpeed;
    /**
     * @brief   The builder class which LandscapeManager uses to get Landscapes
     */
    private LandscapeBuilder m_landscapeBuilder;
    private GameObject m_baseCylinder;

    // Use this for initialization
    void Start () {
        m_baseCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_baseCylinder.transform.position = transform.position + new Vector3(landscapeSize/2, -1f ,landscapeSize/2);
        m_baseCylinder.transform.localScale = new Vector3(landscapeSize * 1.5f, 1, landscapeSize * 1.5f);
        m_landscapeBuilder = gameObject.AddComponent<LandscapeBuilder>();
        m_landscapeBuilder.build(landscapeSize, landscapeSize, baseSeedProbability, stepSpeed);
        m_landscapeBuilder.nodeReport();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
