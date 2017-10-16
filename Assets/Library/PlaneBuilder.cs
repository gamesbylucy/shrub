using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBuilder : LandscapeBuilder {

    private int m_planeCount;
	// Use this for initialization
	void Start ()
    {
        m_planeCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public override Landscape get()
    {
        return null;
    }

    private void buildPlane()
    {
        GameObject newPlaneObject = new GameObject("Plane_" + m_planeCount++);
        MeshFilter  meshFilter = newPlaneObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newPlaneObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = newPlaneObject.AddComponent<MeshCollider>();

        Vector3[] vertices = new Vector3[new Vector3(0, 0, 0), new Vector3(Globals.LANDSCAPE_SIZE, 0, 0), new Vector3(0,0,Globals.LANDSCAPE_SIZE)];
    }
}
