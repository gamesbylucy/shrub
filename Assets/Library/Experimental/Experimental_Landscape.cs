using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Experimental_Landscape : MonoBehaviour {

    public int mapWidth, mapHeight;
    public Vector3[] vertices;
    public Vector2[] uv;
    public Vector4[] tangents;
    public Node[,] nodes;
    public int[] triangles;

    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private MeshCollider m_meshCollider;
    private Mesh m_mesh;


    public struct Node
    {
        public int vertexIndex; //index of associated vertex
        public List<Node> neighbors; //TBD neighbor ordering
        public int latitude; //0 to mapHeight
        public int longitude; //0 to mapWidth
    }

    private static int[][] directions = new int[][] 
    {
        new int[]{ -1, -1 },
        new int[] { -1, 0 },
        new int[] { -1, 1 },
        new int[] { 0, 1 },
        new int[] { 1, 1 },
        new int[] { 1, 0 },
        new int[] { 1, -1 },
        new int[] { 0, -1 }
    };

    

    private void Awake()
    {
        generate();
        nodeReport();
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
        nodes = new Node[mapWidth + 1, mapHeight + 1];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        //fill the vertices array with Vector3s according to their positions on the grid
        for (int i = 0, currentHeight = 0; currentHeight <= mapHeight; currentHeight++)
        {
            for (int currentWidth = 0; currentWidth <= mapWidth; currentWidth++, i++)
            {
                vertices[i] = new Vector3(currentWidth, 0, currentHeight);
                Node theNode = new Node();
                theNode.vertexIndex = i;
                theNode.latitude = currentWidth;
                theNode.longitude = currentHeight;
                nodes[currentHeight, currentWidth] = theNode;
                uv[i] = new Vector2((float)currentWidth / mapWidth, (float)currentHeight / mapHeight);
                tangents[i] = tangent;
            }
        }

        for(int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                nodes[i, j].neighbors = getNeighbors(nodes, i, j);
            }
        }

        m_mesh.vertices = vertices;
        m_mesh.uv = uv;
        m_mesh.tangents = tangents;

        triangles = new int[mapWidth * mapHeight * 6];
        for (int currentTriangle = 0, currentVertex = 0, y = 0; y < mapHeight; y++, currentVertex++)
        {
            for (int x = 0; x < mapWidth; x++, currentTriangle += 6, currentVertex++)
            {
                triangles[currentTriangle] = currentVertex; //southwest vertex (vertex 1)
                triangles[currentTriangle + 3] = triangles[currentTriangle + 2] = currentVertex + 1; //southeast vertex (vertex 2)
                triangles[currentTriangle + 4] = triangles[currentTriangle + 1] = currentVertex + mapWidth + 1; //northwest vertex (vertex 3)
                triangles[currentTriangle + 5] = currentVertex + mapWidth + 2;
                /*LEFT TRIANGLE
                 * vertex 3
                 * |
                 * v
                 * O
                 * xx
                 * xxx
                 * xxxx
                 * xxxxx
                 * OxxxxO<- vertex 2
                 * ^
                 * |
                 * vertex 1
                 *
                 * RIGHT TRIANGLE
                 * 
                 * vertex 2
                 * |
                 * v
                 * OxxxxO <-vertex 1
                 *  xxxxx
                 *   xxxx
                 *    xxx
                 *     xx
                 *      O
                 *      ^
                 *      |
                 *      vertex 3
                 * 
                 * GRID NEIGHBORS
                 * [[20][21][22][23][24]
                 * [15][16][17][18][19]
                 * [10][11][12][13][14]
                 * [5 ][6 ][7 ][8 ][9 ]
                 * [0 ][1 ][2 ][3 ][4 ]]
                 */
            }
        }

        m_mesh.triangles = triangles;
        m_mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        //return if vertices have not been initialized to prevent an error
        if (vertices == null || triangles == null)
        {
            return;
        }

        //draw all vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    private static List<Node> getNeighbors(Node[ , ] nodes, int x, int y)
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

    private void nodeReport()
    {
        string arrayString = "";
        string neighborString = "";
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (nodes[i, j].vertexIndex < 10)
                {
                    arrayString += "[" + nodes[i, j].vertexIndex + " ]";
                }
                else
                {
                    arrayString += "[" + nodes[i, j].vertexIndex + "]";
                }
                neighborString += "Node " + nodes[i, j].vertexIndex + "'s neighbors:\n";
                foreach (Node node in nodes[i, j].neighbors)
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
}
