using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    /**
    * @brief Stores data about nodes in the mesh to facilitate landscape generation algorithms.
    */
public class WorldMapVertex
{
    /****************************************************************************************************
    * Public Members
    ****************************************************************************************************/
    public bool isLand = false;
    public bool isStableLand = false;
    public bool isStableLandNextTick = false;
    public bool isComplexLand = false;
    public bool isLocked = false;
    public float rank;
    public Enumerations.States state = Enumerations.States.Ocean;
    public int vertexIndex; //index of associated vertex
    public int complexity = 0;
    public float initialSeedProbability;
    public List<WorldMapVertex> neighbors; //TBD neighbor ordering
    public Vector3 position;
    public GameObject nodeDecal;


    /****************************************************************************************************
    * Private Members
    ****************************************************************************************************/
    private static Vector3 m_baseDecalScale = new Vector3(1, 5, 1);
    private static float m_baseDecalYOffset = .2f;
    private int m_stabilizationCount = 0;
    private int m_decalScaleAdjustment = 4;
    private int m_stabilizationPeriod;
    private int m_decalScaleModifier;

    /****************************************************************************************************
    * Constants
    ****************************************************************************************************/


    /****************************************************************************************************
    * Public Methods
    ****************************************************************************************************/
    public void init(int theStabilizationPeriod, int decalScale)
    {
        m_stabilizationPeriod = theStabilizationPeriod;
        m_decalScaleModifier = decalScale;
    }

    public void setNextState()
    {
        if (isLocked == false)
        {
            if (isLand == true)
            {
                if (!isStableLand && (getNumPopulatedNeighbors() < 2 || getNumPopulatedNeighbors() > 3))
                {
                    m_stabilizationCount = 0;
                    state = Enumerations.States.Ocean;
                    return;
                }

                if (isComplexLand)
                {
                    state = Enumerations.States.Complex_Land;
                    return;
                }
                else
                {
                    if (isStableLand)
                    {
                        if (getNumStableNeighbors() >= 3)
                        {
                            state = Enumerations.States.Potential_Complex_Land;
                            return;
                        }
                        else
                        {
                            state = Enumerations.States.StableLand;
                            return;
                        }
                    }
                    else
                    {
                        if (m_stabilizationCount >= m_stabilizationPeriod)
                        {
                            state = Enumerations.States.StableLand;
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
                state = Enumerations.States.UnstableLand;
                return;
            }
            else
            {
                if (getNumPopulatedNeighbors() == 3)
                {
                    m_stabilizationCount++;
                    state = Enumerations.States.UnstableLand;
                    return;
                }
            }
            m_stabilizationCount = 0;
            state = Enumerations.States.Ocean;
            return;
        }
    }

    public void setFlags()
    {
        Enumerations.States theState = state;
        switch (theState)
        {
            case Enumerations.States.Ocean:
                isLand = false;
                isStableLand = false;
                isComplexLand = false;
                isLocked = false;
                break;
            case Enumerations.States.UnstableLand:
                isLand = true;
                isStableLand = false;
                isComplexLand = false;
                isLocked = false;
                break;
            case Enumerations.States.StableLand:
                isLand = true;
                isStableLand = true;
                isComplexLand = false;
                isLocked = false;
                break;
            case Enumerations.States.Potential_Complex_Land:
                isLand = true;
                isStableLand = true;
                isComplexLand = false;
                isLocked = false;
                break;
            case Enumerations.States.Complex_Land:
                isLand = true;
                isStableLand = true;
                isComplexLand = true;
                isLocked = false;
                break;
            case Enumerations.States.Border:
                isLand = false;
                isStableLand = false;
                isComplexLand = false;
                isLocked = true;
                break;
        }
    }


    public void setDecal()
    {
        Enumerations.States theState = state;
        switch (theState)
        {
            case Enumerations.States.Ocean:
                nodeDecal = null;
                break;
            case Enumerations.States.UnstableLand:
                nodeDecal = null;
                break;
            case Enumerations.States.StableLand:
                nodeDecal = null;
                break;
            case Enumerations.States.Potential_Complex_Land:
                nodeDecal = null;
                break;
            case Enumerations.States.Complex_Land:
                if (nodeDecal == null) {
                    nodeDecal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    nodeDecal.transform.position = new Vector3(position.x, m_baseDecalScale.y - (m_baseDecalScale.y * m_baseDecalYOffset), position.z);
                    nodeDecal.name = "Decal for node @ " + nodeDecal.transform.position;
                    nodeDecal.transform.localScale = m_baseDecalScale;
                    if (complexity == 0)
                    {
                        Debug.Log("The complexity for node " + nodeDecal.transform.position + " was 0.");
                        setDecalColor(Color.red);
                    }
                    
                }
                else
                {
                    nodeDecal.transform.position = new Vector3(position.x, m_baseDecalScale.y - (m_baseDecalScale.y * m_baseDecalYOffset), position.z);
                    nodeDecal.transform.localScale = m_baseDecalScale;
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
        foreach (WorldMapVertex neighbor in neighbors)
        {
            if (neighbor.isStableLand == true)
            {
                numStableNeighbors++;
            }
        }
        return numStableNeighbors;
    }

    public int getNumComplexNeighbors()
    {
        int numComplexNeighbors = 0;
        foreach (WorldMapVertex neighbor in neighbors)
        {
            if (neighbor.isComplexLand == true)
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
        foreach (WorldMapVertex neighbor in neighbors)
        {
            if (neighbor.isLand == true)
            {
                numPopulated++;
            }
        }
        return numPopulated;
    }
}
