using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    /**
    * @brief Stores data about nodes in the mesh to facilitate landscape generation algorithms.
    */
public class Node
{
    /****************************************************************************************************
    * Public Members
    ****************************************************************************************************/
    public bool isPopulated = false;
    public bool isStable = false;
    public bool isStableNextTick = false;
    public bool isComplex = false;
    public bool isLocked = false;
    public float rank;
    public Enumerations.States state = Enumerations.States.Unpopulated;
    public int vertexIndex; //index of associated vertex
    public int complexity = 0;
    public float initialSeedProbability;
    public List<Node> neighbors; //TBD neighbor ordering
    public Vector3 vertex;
    public static Vector3 decalScale = new Vector3(.5f, 1, .5f);
    public GameObject nodeDecal;


    /****************************************************************************************************
    * Private Members
    ****************************************************************************************************/
    private int m_neighborCount = 0;
    private int m_stableNeighborCount = 0;
    private int m_stabilizationCount = 0;

    /****************************************************************************************************
    * Constants
    ****************************************************************************************************/
    private const int STABLIZATION_PERIOD = 15;

    /****************************************************************************************************
    * Public Methods
    ****************************************************************************************************/

    public void setNextState()
    {
        if (isLocked == false)
        {
            if (isPopulated == true)
            {
                if (!isStable && (getNumPopulatedNeighbors() < 2 || getNumPopulatedNeighbors() > 3))
                {
                    m_stabilizationCount = 0;
                    state = Enumerations.States.Unpopulated;
                    return;
                }

                if (isComplex)
                {
                    state = Enumerations.States.Complex;
                    return;
                }
                else
                {
                    if (isStable)
                    {
                        if (getNumStableNeighbors() >= 3)
                        {
                            state = Enumerations.States.Potential_Complex;
                            return;
                        }
                        else
                        {
                            state = Enumerations.States.Stable;
                            return;
                        }
                    }
                    else
                    {
                        if (m_stabilizationCount >= STABLIZATION_PERIOD)
                        {
                            state = Enumerations.States.Stable;
                            return;
                        }
                    }
                }

                /*if (m_stabilizationCount >= STABLIZATION_PERIOD)
                {
                    if (getNumStableNeighbors() >= 3)
                    {
                        if (!isComplex)
                        {
                            state = Enumerations.States.Potential_Complex;
                            return;
                        }
                        state = Enumerations.States.Complex;
                        return;
                    }
                    state = Enumerations.States.Stable;
                    return;
                }*/

                m_stabilizationCount++;
                state = Enumerations.States.Populated;
                return;
            }
            else
            {
                if (getNumPopulatedNeighbors() == 3)
                {
                    m_stabilizationCount++;
                    state = Enumerations.States.Populated;
                    return;
                }
            }
            m_stabilizationCount = 0;
            state = Enumerations.States.Unpopulated;
            return;
        }
    }

    public void setFlags()
    {
        Enumerations.States theState = state;
        switch (theState)
        {
            case Enumerations.States.Unpopulated:
                isPopulated = false;
                isStable = false;
                isComplex = false;
                isLocked = false;
                break;
            case Enumerations.States.Populated:
                isPopulated = true;
                isStable = false;
                isComplex = false;
                isLocked = false;
                break;
            case Enumerations.States.Stable:
                isPopulated = true;
                isStable = true;
                isComplex = false;
                isLocked = false;
                break;
            case Enumerations.States.Potential_Complex:
                isPopulated = true;
                isStable = true;
                isComplex = false;
                isLocked = false;
                break;
            case Enumerations.States.Complex:
                isPopulated = true;
                isStable = true;
                isComplex = true;
                isLocked = false;
                break;
            case Enumerations.States.Border:
                isPopulated = false;
                isStable = false;
                isComplex = false;
                isLocked = true;
                break;
        }
    }


    public void setDecal()
    {
        Enumerations.States theState = state;
        switch (theState)
        {
            case Enumerations.States.Unpopulated:
                nodeDecal = null;
                break;
            case Enumerations.States.Populated:
                nodeDecal = null;
                break;
            case Enumerations.States.Stable:
                if (nodeDecal == null)
                {
                    nodeDecal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    nodeDecal.transform.position = new Vector3(vertex.x, 1, vertex.z);
                    nodeDecal.transform.localScale = decalScale;
                    nodeDecal.name = "Decal for node @ " + nodeDecal.transform.position;
                }
                else
                {
                    nodeDecal.transform.position = new Vector3(vertex.x, 1, vertex.z);
                    nodeDecal.transform.localScale = decalScale;
                }
                break;
            case Enumerations.States.Potential_Complex:
                if (nodeDecal == null)
                {
                    nodeDecal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    nodeDecal.transform.position = new Vector3(vertex.x, 1, vertex.z);
                    nodeDecal.transform.localScale = decalScale;
                    nodeDecal.name = "Decal for node @ " + nodeDecal.transform.position;
                    setDecalColor(Color.blue);
                }
                else
                {
                    nodeDecal.transform.position = new Vector3(vertex.x, 1, vertex.z);
                    nodeDecal.transform.localScale = decalScale;
                    setDecalColor(Color.green);
                }
                break;
            case Enumerations.States.Complex:
                if (nodeDecal == null) {
                    nodeDecal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    nodeDecal.transform.position = new Vector3(vertex.x, 1, vertex.z);
                    nodeDecal.name = "Decal for node @ " + nodeDecal.transform.position;
                    nodeDecal.transform.localScale = new Vector3(1, complexity, 1);
                    if (complexity == 0)
                    {
                        Debug.Log("The complexity for node " + nodeDecal.transform.position + " was 0.");
                        setDecalColor(Color.red);
                    }
                    
                }
                else
                {
                    nodeDecal.transform.position = new Vector3(vertex.x, 1, vertex.z);
                    nodeDecal.transform.localScale = new Vector3(1, complexity, 1);
                    if (complexity == 0)
                    {
                        Debug.Log("The complexity for node " + nodeDecal.transform.position + " was 0 and the node already existed.");
                        setDecalColor(Color.black);
                    }
                }
                break;
            case Enumerations.States.Border:
                if (nodeDecal != null)
                {
                    GameObject.Destroy(nodeDecal);
                    Debug.Log("A node decal at " + nodeDecal.transform.position + " was destroyed because it became a border.");
                    nodeDecal = null;
                }
                break;
        }
    }

    public int getNumStableNeighbors()
    {
        int numStableNeighbors = 0;
        foreach (Node neighbor in neighbors)
        {
            if (neighbor.isStable == true)
            {
                numStableNeighbors++;
            }
        }
        return numStableNeighbors;
    }

    public int getNumComplexNeighbors()
    {
        int numComplexNeighbors = 0;
        foreach (Node neighbor in neighbors)
        {
            if (neighbor.isComplex == true)
            {
                numComplexNeighbors++;
            }
        }
        return numComplexNeighbors;
    }

    public void setDecalColor(Color color)
    {
        nodeDecal.GetComponent<Renderer>().material.color = color;
    }
    /****************************************************************************************************
    * Private Methods
    ****************************************************************************************************/
    private int getNumPopulatedNeighbors()
    {
        int numPopulated = 0;
        foreach (Node neighbor in neighbors)
        {
            if (neighbor.isPopulated == true)
            {
                numPopulated++;
            }
        }
        return numPopulated;
    }
}
