using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LandscapeBuilder : MonoBehaviour{

    /****************************************************************************************************
     * Public Members
     ****************************************************************************************************/

    /****************************************************************************************************
     * Private Members
     ****************************************************************************************************/
    private int m_mapWidth, m_mapHeight;
    private Vector3[] m_vertices;
    private Vector2[] m_uv;
    private Vector4[] m_tangents;
    private Node[,] m_nodes;
    private int[] m_triangles;
    private Mesh m_mesh;
    private Vector4 m_tangent;

    /****************************************************************************************************
     * Static Members
     ****************************************************************************************************/
     /**
      * @brief A 2D array of values representing directions on a grid of nodes.
      */
    private static int[][] directions = new int[][] 
    {
        new int[]{ -1, -1 }, //NW
        new int[] { -1, 0 }, //W
        new int[] { -1, 1 }, //SW
        new int[] { 0, 1 },  //S
        new int[] { 1, 1 },  //SE
        new int[] { 1, 0 },  //E
        new int[] { 1, -1 }, //NE
        new int[] { 0, -1 }  //N
    };


    /****************************************************************************************************
     * Public Methods
     ****************************************************************************************************/

    public void setDimensions(int width, int height)
    {
        m_mapWidth = width;
        m_mapHeight = height;
    }
    public void generate()
    {
        initializeMeshData();
        calculateMeshData();
        assignNodeNeighbors();
        assignMeshData();
        randomizeElevation(m_nodes, -1f, 1f);
        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateTangents();
    }

    /**
     * Generates a text based visualization of the node grid and displays a list of each nodes neighbors.
     */
    public void nodeReport()
    {
        string arrayString = "";
        string neighborString = "";
        for (int i = 0; i < m_nodes.GetLength(0); i++)
        {
            for (int j = 0; j < m_nodes.GetLength(1); j++)
            {
                if (m_nodes[i, j].vertexIndex < 10)
                {
                    arrayString += "[" + m_nodes[i, j].vertexIndex + " ]";
                }
                else
                {
                    arrayString += "[" + m_nodes[i, j].vertexIndex + "]";
                }
                neighborString += "Node " + m_nodes[i, j].vertexIndex + "'s neighbors:\n";
                foreach (Node node in m_nodes[i, j].neighbors)
                {
                    neighborString += "[" + node.vertexIndex + "]";
                }
                neighborString += "\n";
            }
            arrayString += "\n";
        }
        Debug.Log(arrayString);
        Debug.Log(neighborString);
    }

    /****************************************************************************************************
     * Private Methods
     ****************************************************************************************************/

    /****************************************************************************************************
     * -Unity Standard methods
     ****************************************************************************************************/

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    /****************************************************************************************************
     * -Shrub Methods
     ****************************************************************************************************/
    /**
     * @brief Creates the landscape mesh and initializes all the values associated with the landscape.
     */
    private void initializeMeshData()
    {
        //create the mesh
        GetComponent<MeshFilter>().mesh = m_mesh = new Mesh();
        m_mesh.name = "Landscape";

        //initialize local variables
        m_vertices = new Vector3[(m_mapWidth + 1) * (m_mapHeight + 1)];
        m_uv = new Vector2[m_vertices.Length];
        m_tangents = new Vector4[m_vertices.Length];
        m_nodes = new Node[m_mapHeight + 1, m_mapWidth + 1];
        m_triangles = new int[m_mapWidth * m_mapHeight * 6];
        m_tangent = new Vector4(1f, 0f, 0f, -1f);
    }

    /**
     * @brief Calculates the vertices, node values, UV mapping, tangents, and triangles of the mesh.
     */
    private void calculateMeshData()
    {
        //calculate vertices, node values, UV mapping, and tangents
        for (int i = 0, y = 0; y <= m_mapHeight; y++)
        {
            for (int x = 0; x <= m_mapWidth; x++, i++)
            {
                m_vertices[i] = new Vector3(x, 0, y);
                Node theNode = new Node();
                theNode.vertexIndex = i;
                theNode.vertex = m_vertices[i];
                m_nodes[y, x] = theNode;
                m_uv[i] = new Vector2((float)x / m_mapWidth, (float)y / m_mapHeight);
                m_tangents[i] = m_tangent;
            }
        }

        //calculate triangles
        for (int currentTriangle = 0, currentVertex = 0, y = 0; y < m_mapHeight; y++, currentVertex++)
        {
            for (int x = 0; x < m_mapWidth; x++, currentTriangle += 6, currentVertex++)
            {
                m_triangles[currentTriangle] = currentVertex; //southwest vertex (vertex 1)
                m_triangles[currentTriangle + 3] = m_triangles[currentTriangle + 2] = currentVertex + 1; //southeast vertex (vertex 2)
                m_triangles[currentTriangle + 4] = m_triangles[currentTriangle + 1] = currentVertex + m_mapWidth + 1; //northwest vertex (vertex 3)
                m_triangles[currentTriangle + 5] = currentVertex + m_mapWidth + 2;
            }
        }
    }

    /**
     * @brief Updates the neighbor list of the nodes.
     */
    private void assignNodeNeighbors()
    {
        //update node neighbors
        for (int y = 0; y < m_nodes.GetLength(0); y++)
        {
            for (int x = 0; x < m_nodes.GetLength(1); x++)
            {
                m_nodes[y, x].neighbors = getNeighbors(m_nodes, y, x);
            }
        }
    }

    /**
     * @brief Assigns all local values to the mesh.
     */
    private void assignMeshData()
    {
        //assign local values to mesh
        m_mesh.vertices = m_vertices;
        m_mesh.uv = m_uv;
        m_mesh.tangents = m_tangents;
        m_mesh.triangles = m_triangles;
    }

    /**
     * @brief Gets the neighbors of a node in a 2D grid of nodes.
     * @param nodes A two dimensional list of nodes which represents a grid of nodes.
     * @param y The y coordinate of the node.
     * @param x The x coordinate of the node.
     */
    private static List<Node> getNeighbors(Node[ , ] nodes, int y, int x)
    {
        List<Node> theNodes = new List<Node>();

        foreach (int[] direction in directions)
        {
            int cx = x + direction[0];
            int cy = y + direction[1];
            if (cy >= 0 && cy < nodes.GetLength(0))
            {
                if (cx >= 0 && cx < nodes.GetLength(1))
                {
                    theNodes.Add(nodes[cy, cx]);
                }
            }
        }
        return theNodes;
    }

    /**
     * Randomizes the elevation of all landscape nodes.
     */
    private void randomizeElevation(Node[,] nodes, float elevationLowerBound, float elevationUpperBound)
    {
        Vector3[] theVertices = new Vector3[m_vertices.Length];
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                theVertices[nodes[i, j].vertexIndex] = (nodes[i, j].vertex += new Vector3(0, Random.Range(elevationLowerBound, elevationUpperBound), 0));
            }
        }
        m_mesh.vertices = theVertices;
    }



    /****************************************************************************************************
     * Shrub Event Handlers
     ****************************************************************************************************/

    /****************************************************************************************************
     * Unity Event Handlers
     ****************************************************************************************************/
    private void OnDrawGizmos()
    {
        //return if vertices have not been initialized to prevent an error
        if (m_vertices == null || m_triangles == null)
        {
            return;
        }

        //draw all vertices
        for (int i = 0; i < m_vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(m_vertices[i], 0.1f);
        }
    }

    /****************************************************************************************************
     * Inner Classes
     ****************************************************************************************************/
    /**
     * Stores data about nodes in the mesh to facilitate landscape generation algorithms.
     */
    private class Node
    {
        public int vertexIndex; //index of associated vertex
        public List<Node> neighbors; //TBD neighbor ordering
        public Vector3 vertex;
    }
}
