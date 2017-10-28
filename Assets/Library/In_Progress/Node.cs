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
    public bool isPopulatedNextTick = false;
    public bool isStable = false;
    public bool isStableNextTick = false;
    public bool isStableNodeBorder = false;
    public bool isComplex = false;
    public bool isComplexNextTick = false;
    public bool isComplexMember = false;
    public bool isLandscapeBorder = false;
    public bool isLocked = false;
    public float rank;
    public int vertexIndex; //index of associated vertex
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
    private int m_stablizationCount = 0;



    /****************************************************************************************************
    * Constants
    ****************************************************************************************************/
    private const int STABLIZATION_PERIOD = 15;

    /****************************************************************************************************
    * Public Methods
    ****************************************************************************************************/
    public void tick()
    {
        m_neighborCount = getNeighborCount();
        setNextState(m_neighborCount);
    }

    /**
        * @brief Determine the nodes state the next frame depending on the status of the node.
        */
    public void updateStabilizationStatus()
    {
        if (isStableNextTick && !isLandscapeBorder)
        {
            isStable = true;
            if (nodeDecal == null)
            {
                nodeDecal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                nodeDecal.transform.position = new Vector3(vertex.x, 1, vertex.z);
                nodeDecal.transform.localScale = decalScale;
                nodeDecal.name = "Decal for node @ " + nodeDecal.transform.position;
            }
            isPopulated = true;
            isPopulatedNextTick = true;
        }
        else
        {
            isStable = false;
        }
    }

    /**
        * @brief Update the population status of the node.
        */
    public void updatePopulatedStatus()
    {
        if (isPopulatedNextTick == true)
        {
            isPopulated = true;
            isPopulatedNextTick = false;
        }
        else
        {
            isPopulated = false;
        }
    }

    public void updateComplexMembership()
    {
        if (isComplexNextTick && !isComplex)
        {
            isComplex = true;
            Complex theComplex = new Complex();
            theComplex.add(this);
            setDecalColor(theComplex.color);
            foreach (Node neighbor in neighbors)
            {
                if (neighbor.isStable && !neighbor.isComplex)
                {
                    isComplex = true;
                    theComplex.add(neighbor);
                    neighbor.setDecalColor(theComplex.color);
                }
            }
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




    /**
        * @brief Determine the number of populated neighbors the node has.
        */
    private int getNeighborCount()
    {
        int result = 0;
        foreach (Node neighbor in neighbors)
        {
            if (neighbor.isPopulated)
            {
                result++;
            }
        }

        return result;
    }

    /**
        * @brief Set the state of the node the next tick.
        */
    private void setNextState(int neighborCount)
    {
        if (isPopulated == true)
        {
            if (neighborCount < 2 || neighborCount > 3 && !isStable)
            {
                isPopulatedNextTick = false;
                m_stablizationCount = 0;
            }
            else
            {
                isPopulatedNextTick = true;
                m_stablizationCount++;
            }
        }
        else
        {
            if (neighborCount == 3)
            {
                isPopulatedNextTick = true;
                m_stablizationCount++;
            }
        }

        if (m_stablizationCount >= STABLIZATION_PERIOD)
        {
            isStableNextTick = true;
            m_stablizationCount = 0;
        }

        if (isStable)
        {
            if (getNumStableNeighbors() == 3)
            {
                isComplexNextTick = true;
            }
        }
    }
}
