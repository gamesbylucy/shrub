/**
 * @brief Instantiates an n x n mesh representing populated vertices where n and updates it based on the state of a list of vertices.
 * 
 * There are two interfaces into the method. init initializes the mesh based on a given map size. Update mesh data updates the state
 * of the mesh based on a list of WorldMapVerticies passed as an argument.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapLandMesh : MonoBehaviour {
    /**
    * Public Members
    */

    /**
     * Private Members
     */
    private Vector3[] m_vertices;
    private Vector2[] m_uv;
    private Vector4[] m_tangents;
    private int[] m_triangles;
    private Mesh m_mesh;
    private MeshCollider m_meshCollider;
    private Vector4 m_tangent;
    private float m_seaLevelAdjustment = 1f;

    /**
     * Public Methods
     */
    public void init(int mapSize, int mapScale)
    {
        instantiateMesh(mapSize);
        setMeshGeometryData(mapSize, mapScale);
        updateMesh();
        updateMeshCollider();
        recalculateMesh();
    }

    public void updateMeshData(WorldMapVertex[ , ] vertices, int mapScale)
    {
        Vector3[] theVertices = new Vector3[m_vertices.Length];
        Vector3 theVertex;
        foreach (WorldMapVertex vertex in vertices)
        {
            if (vertex.isLand)
            {
                theVertex = m_vertices[vertex.vertexIndex];
                theVertex.y = 1.7f;
                theVertices[vertex.vertexIndex] = theVertex;
            }
            else
            {
                bool hasComplexNeighbor = false;

                foreach (WorldMapVertex neighbor in vertex.neighbors)
                {
                    if (neighbor.isComplexLand)
                    {
                        hasComplexNeighbor = true;
                        break;
                    }
                }

                if (hasComplexNeighbor)
                {
                    theVertex = m_vertices[vertex.vertexIndex];
                    theVertex.y = 1;
                    theVertices[vertex.vertexIndex] = theVertex;
                }
                else
                {
                    theVertex = m_vertices[vertex.vertexIndex];
                    theVertex.y = -1;
                    theVertices[vertex.vertexIndex] = theVertex;
                }
            }
        }
        m_mesh.vertices = theVertices;
    }

    /**
     * Private Methods
     */
    private void instantiateMesh(int mapSize)
    {
        GetComponent<MeshFilter>().mesh = m_mesh = new Mesh();
        m_meshCollider = gameObject.AddComponent<MeshCollider>();
        m_meshCollider.convex = true;
        m_mesh.name = "WorldMapPopulationMesh";
        Material newMat = Resources.Load("Materials/Grass", typeof(Material)) as Material;
        GetComponent<Renderer>().material = newMat;
    }

    private void setMeshGeometryData(int mapSize, int mapScale)
    {
        m_vertices = new Vector3[(mapSize + 1) * (mapSize + 1)];
        m_uv = new Vector2[m_vertices.Length];
        m_tangents = new Vector4[m_vertices.Length];
        m_triangles = new int[mapSize * mapSize * 6];
        m_tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= mapSize; y++)
        {
            for (int x = 0; x <= mapSize; x++, i++)
            {
                m_vertices[i] = new Vector3(x * mapScale, 0, y * mapScale);
                m_uv[i] = new Vector2((float)x / mapSize, (float)y / mapSize);
                m_tangents[i] = m_tangent;
            }
        }

        for (int currentTriangle = 0, currentVertex = 0, y = 0; y < mapSize; y++, currentVertex++)
        {
            for (int x = 0; x < mapSize; x++, currentTriangle += 6, currentVertex++)
            {
                m_triangles[currentTriangle] = currentVertex;
                m_triangles[currentTriangle + 3] = m_triangles[currentTriangle + 2] = currentVertex + 1;
                m_triangles[currentTriangle + 4] = m_triangles[currentTriangle + 1] = currentVertex + mapSize + 1;
                m_triangles[currentTriangle + 5] = currentVertex + mapSize + 2;
            }
        }
    }

    private void updateMesh()
    {
        m_mesh.vertices = m_vertices;
        m_mesh.uv = m_uv;
        m_mesh.tangents = m_tangents;
        m_mesh.triangles = m_triangles;
    }

    private void updateMeshCollider()
    {
        m_meshCollider.sharedMesh = m_mesh;
    }

    private void recalculateMesh()
    {
        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateTangents();
    }
}
