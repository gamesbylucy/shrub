using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Experimental_Landscape : MonoBehaviour {

    public int mapWidth, mapHeight;
    public Vector3[] vertices;
    public Vector2[] uv;
    public Vector4[] tangents;

    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private MeshCollider m_meshCollider;
    private Mesh m_mesh;

    private void Awake()
    {
        reposition();
        generate();
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void generate()
    {
        //make the generation process visible
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        //create the mesh
        GetComponent<MeshFilter>().mesh = m_mesh = new Mesh();
        m_mesh.name = "Experimental_Landscape";
        
        //create a new array using the public parameters and assign it to vertices
        vertices = new Vector3[(mapWidth + 1) * (mapHeight + 1)];
        uv = new Vector2[vertices.Length];
        tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        //fill the vertices array with Vector3s according to their positions on the grid
        for (int i = 0, currentHeight = 0; currentHeight <= mapHeight; currentHeight++)
        {
            for (int currentWidth = 0; currentWidth <= mapWidth; currentWidth++, i++)
            {
                vertices[i] = new Vector3(currentWidth, currentHeight);
                uv[i] = new Vector2((float)currentWidth / mapWidth, (float)currentHeight / mapHeight);
                tangents[i] = tangent;
            }
        }

        m_mesh.vertices = vertices;
        m_mesh.uv = uv;
        m_mesh.tangents = tangents;

        int[] triangles = new int[mapWidth * mapHeight * 6];
        for (int currentTriangle = 0, currentVertex = 0, y = 0; y < mapHeight; y++, currentVertex++)
        {
            for (int x = 0; x < mapWidth; x++, currentTriangle += 6, currentVertex++)
            {
                triangles[currentTriangle] = currentVertex;
                triangles[currentTriangle + 3] = triangles[currentTriangle + 2] = currentVertex + 1;
                triangles[currentTriangle + 4] = triangles[currentTriangle + 1] = currentVertex + mapWidth + 1;
                triangles[currentTriangle + 5] = currentVertex + mapWidth + 2;
            }
        }

        m_mesh.triangles = triangles;
        m_mesh.RecalculateNormals();
    }

    private void reposition()
    {

    }

    private void OnDrawGizmos()
    {
        //return if vertices have not been initialized to prevent an error
        if (vertices == null)
        {
            return;
        }

        //draw all vertices
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
