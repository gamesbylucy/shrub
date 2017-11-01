using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief Builds the landscape of the Shrub play area.
 */
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
    private MeshCollider m_meshCollider;
    private float m_stepSpeed;

    /****************************************************************************************************
     * Constant Members
     ****************************************************************************************************/


    /****************************************************************************************************
     * Static Members
     ****************************************************************************************************/
    /**
     * @brief A 2D array of values representing directions on a grid of nodes.
     */
    private static int[][] directions = new int[][] 
    {
        new int[]{ -1, -1 }, //SW
        new int[] { -1, 0 }, //W
        new int[] { -1, 1 }, //NW
        new int[] { 0, 1 },  //N
        new int[] { 1, 1 },  //NE
        new int[] { 1, 0 },  //E
        new int[] { 1, -1 }, //SE
        new int[] { 0, -1 }  //S
    };


    /****************************************************************************************************
     * Public Methods
     ****************************************************************************************************/

    /**
     * @brief Calls private methods to build the components of the landscape.
     */
    public void build(int width, int height, float baseSeedProbability, float initialStepSpeed)
    {
        m_mapWidth = width;
        m_mapHeight = height;
        m_stepSpeed = initialStepSpeed;
        initializeMeshData();
        calculateSquareMeshData();
        assignNodeNeighbors();
        checkForLandscapeBorders(m_nodes);
        setSeedProbability(m_nodes, baseSeedProbability);
        assignMeshData();
        assignSharedMesh();
        seedPopulation(Enumerations.SeedTypes.Random);
        setPopulationMesh();
        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateTangents();
    }

    /**
     * @brief Displays a Debug.Log of the node grid. Displays each nodes neighbors listed clockwise from south to southwest.
     */
    public void nodeReport()
    {
        string arrayString = "";
        string neighborString = "";
        for (int i = m_nodes.GetLength(0) - 1 ; i >= 0; i--)
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
        InvokeRepeating("step", m_stepSpeed, m_stepSpeed);
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
        m_meshCollider = gameObject.AddComponent<MeshCollider>();
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
     * @brief Calculates the vertices, node values, UV mapping, tangents, and triangles of a square mesh.
     */
    private void calculateSquareMeshData()
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
                m_triangles[currentTriangle] = currentVertex;
                m_triangles[currentTriangle + 3] = m_triangles[currentTriangle + 2] = currentVertex + 1;
                m_triangles[currentTriangle + 4] = m_triangles[currentTriangle + 1] = currentVertex + m_mapWidth + 1; 
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
     * @brief Assigns the landscape mesh to the mesh colliders shared mesh.
     */
    private void assignSharedMesh()
    {
        m_meshCollider.sharedMesh = m_mesh;
    }

    /**
     * @brief Gets the neighbors of a node in a 2D grid of nodes.
     * @param nodes A two dimensional list of nodes which represents a grid of nodes.
     * @param y The y coordinate of the node.
     * @param x The x coordinate of the node.
     */
    private List<Node> getNeighbors(Node[ , ] nodes, int y, int x)
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

    private void checkForLandscapeBorders(Node[ , ] landscape)
    {
        foreach (Node node in landscape)
        {
            if (node.neighbors.Count < 8)
            {
                node.state = Enumerations.States.Border;
            }
        }
    }

    private void setSeedProbability(Node[ , ] landscape, float baseSeedProbability)
    {
        foreach (Node node in landscape)
        {
            bool hasLandscapeBorderNeighbor = false;
            foreach (Node neighbor in node.neighbors)
            {
                if (neighbor.state == Enumerations.States.Border)
                {
                    hasLandscapeBorderNeighbor = true;
                    break;
                }
            }

            if (node.state == Enumerations.States.Border)
            {
                node.initialSeedProbability = 0;
            }
            else if (hasLandscapeBorderNeighbor == true)
            {
                node.initialSeedProbability = baseSeedProbability / 2;
            }
            else
            {
                node.initialSeedProbability = baseSeedProbability;
            }
        }
    }

    /**
     * @brief Seeds the initial distribution of populated nodes.
     */
    private void seedPopulation(Enumerations.SeedTypes seedType)
    {
        switch (seedType)
        {
            case Enumerations.SeedTypes.Random:
                foreach (Node node in m_nodes)
                {
                    if ((float)ShrubUtils.random.NextDouble() < node.initialSeedProbability)
                    {
                        node.state = Enumerations.States.Populated;
                    }
                }
                break;
            case Enumerations.SeedTypes.Stable:
                break;
            case Enumerations.SeedTypes.Chaotic:
                break;
        }
    }

    private void step()
    {
        setCurrentFlags();
        setPopulationMesh();
        setDecals();
        setNextStates();
        setNextComplexes();
    }

    public void setCurrentFlags()
    {
        foreach (Node node in m_nodes)
        {
            node.setFlags();
        }
    }

    public void setDecals()
    {
        foreach (Node node in m_nodes)
        {
            node.setDecal();
        }
    }

    public void setNextStates()
    {
        foreach (Node node in m_nodes)
        {
            node.setNextState();
        }
    }

    /**
    * @Place all nodes in the Potential_Complex state in a list, scan over them, and determine the initial complexity and placement.
    */
    public void setNextComplexes()
    {
        List<Node> potentialComplexes = new List<Node>();
        /**
        * Add all the potential complex nodes to a list.
        */
        foreach (Node node in m_nodes)
        {
            if (node.state == Enumerations.States.Potential_Complex)
            {
                potentialComplexes.Add(node);
            }
        }

        /**
         * Give each potential complex an random ranking.
         */
        foreach (Node potentialComplex in potentialComplexes)
        {
            potentialComplex.rank = (float)ShrubUtils.random.NextDouble();
        }

        /**
         * Scan over all potential complexes.
         */
        foreach (Node potentialComplex in potentialComplexes)
        {
            /**
            * For each neighbor of the potential complex.
            */
            foreach (Node neighbor in potentialComplex.neighbors)
            {
                /**
                * If the neighbor is complex...
                */
                if (neighbor.state == Enumerations.States.Potential_Complex)
                {
                    /**
                    * ...if the neighbor is a higher rank, continue to the next neighbor without incrementing the complexity.
                    */
                    if (neighbor.rank > potentialComplex.rank)
                    {
                        continue;
                    }
                    /**
                    * ..else if the neighbor is of an equal rank...
                    */
                    else if (neighbor.rank == potentialComplex.rank)
                    {
                        /**
                        * ...50 percent chance to move to the next neighbor without incrementing the complexity.
                        * Otherwise, increment the complexity by 2.
                        */
                        if (.5f > (float)ShrubUtils.random.NextDouble())
                        {
                            continue;
                        }
                        else
                        {
                            potentialComplex.complexity += 2;
                        }
                    }
                    /**
                     * ...else if the neighbor is of a lower rank, increment the complexity by 2.
                     */
                    else
                    {
                        potentialComplex.complexity += 2;
                    }
                }
                /**
                 * Else if the neighbor is stable, but not complex, increment the complexity by 1.
                 */
                else if (neighbor.state == Enumerations.States.Stable)
                {
                    potentialComplex.complexity++;
                }
            }

            /**
            * If the complexity has not been incremented, return out of the method without modifying the node.
            */
            if (potentialComplex.complexity == 0)
            {
                continue;
            }
            /**
             * If the complexity has been incremented, set the nodes state to complex, and set its neighbors states to border.
             */
            else
            {
                potentialComplex.state = Enumerations.States.Complex;

                foreach (Node neighbor in potentialComplex.neighbors)
                {
                    if (neighbor != null)
                    {
                        neighbor.state = Enumerations.States.Border;
                    }
                }
            }
        }
    }


    private void setPopulationMesh()
    {
        Vector3[] theVertices = new Vector3[m_vertices.Length];
        foreach (Node node in m_nodes)
        {
            if (node.isPopulated == true)
            {
                Vector3 theVertex = m_vertices[node.vertexIndex];
                theVertex.y = 1;
                theVertices[node.vertexIndex] = theVertex;
            }
            else
            {
                Vector3 theVertex = m_vertices[node.vertexIndex];
                theVertex.y = -1;
                theVertices[node.vertexIndex] = theVertex;
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
}
