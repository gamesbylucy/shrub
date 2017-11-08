using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WorldMap : MonoBehaviour {
    /**
     *Public Members
     */

    /**
     *Private Members
     */
    private WorldMapPopulationMesh m_worldMapPopulationMesh;
    private OceanMesh m_oceanMesh;
    private WorldMapGraph m_worldMapGraph;
    
    /**
     *Public Methods
     */
    public void init(int mapSize, float baseSeedProbability, float stepSpeed, int stabilizationPeriod)
    {
        m_worldMapPopulationMesh = gameObject.AddComponent<WorldMapPopulationMesh>();
        m_worldMapGraph = gameObject.AddComponent<WorldMapGraph>();
        m_worldMapPopulationMesh.init(mapSize);
        m_worldMapGraph.init(mapSize, baseSeedProbability, stabilizationPeriod);
        InvokeRepeating("step", stepSpeed, stepSpeed);

        GameObject ocean = new GameObject();
        ocean.name = "Ocean";
        ocean.transform.parent = transform.parent;
        ocean.AddComponent<MeshRenderer>();
        ocean.AddComponent<MeshFilter>();
        m_oceanMesh = ocean.AddComponent<OceanMesh>();
        m_oceanMesh.init(mapSize);
    }

    /**
     *Private Methods
     */
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

    private void step()
    {
        WorldMapVertex[ , ] worldMapVertices = m_worldMapGraph.step();
        m_worldMapPopulationMesh.updateMeshData(worldMapVertices);
    }

    private void OnDrawGizmos()
    {

    }
}
