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
    
    public override GameObject get()
    {
        GameObject theObject = createGameObject();
        setVertices(theObject);
        setTriangles(theObject);
        setUV(theObject);
        return theObject;
    }

    /**
     * @brief Construct a basic plane gameobject without special textures.
     */
    private GameObject createGameObject()
    {
        GameObject newPlaneObject = new GameObject("Plane_" + m_planeCount++);
        MeshFilter  meshFilter = newPlaneObject.AddComponent<MeshFilter>();
        Mesh mesh = newPlaneObject.GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.Clear();
        MeshRenderer meshRenderer = newPlaneObject.AddComponent<MeshRenderer>();
        MeshCollider meshCollider = newPlaneObject.AddComponent<MeshCollider>();
        return newPlaneObject;
    }

    /**
     * @brief Sets the vertices of a a GameObject to a flat triangle shape and recalculates bounds, normals, and tangents.
     */
    private void setVertices(GameObject theObject)
    {
        if (theObject != null && theObject)
        {
            if(theObject.GetComponent<MeshFilter>() != null && theObject.GetComponent<Mesh>() != null)
            {
                Mesh mesh = theObject.GetComponent<MeshFilter>().mesh;
                mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
                theObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                theObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
                theObject.GetComponent<MeshFilter>().mesh.RecalculateTangents();
            }
            else
            {
                Debug.Log("Cannot set the vertices of a gameobject without a mesh and mesh filter");
            }
        }
        else
        {
            Debug.Log("Cannot set the vertices of a null object.");
        }
    }

    private void setTriangles(GameObject theObject)
    {
        if (theObject != null && theObject)
        {
            if (theObject.GetComponent<MeshFilter>() != null && theObject.GetComponent<Mesh>() != null)
            {
                Mesh mesh = theObject.GetComponent<MeshFilter>().mesh;
                mesh.triangles = new int[] { 0, 1, 2 };
                theObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                theObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
                theObject.GetComponent<MeshFilter>().mesh.RecalculateTangents();
            }
            else
            {
                Debug.Log("Cannot set the vertices of a gameobject without a mesh and mesh filter");
            }
        }
        else
        {
            Debug.Log("Cannot set the vertices of a null object.");
        }
    }

    private void setUV(GameObject theObject)
    {
        if (theObject != null && theObject)
        {
            if (theObject.GetComponent<MeshFilter>() != null && theObject.GetComponent<Mesh>() != null)
            {
                Mesh mesh = theObject.GetComponent<MeshFilter>().mesh;
                mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
                theObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                theObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
                theObject.GetComponent<MeshFilter>().mesh.RecalculateTangents();
            }
            else
            {
                Debug.Log("Cannot set the vertices of a gameobject without a mesh and mesh filter");
            }
        }
        else
        {
            Debug.Log("Cannot set the vertices of a null object.");
        }
    }
}
