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
    private WorldMapLandMesh m_worldMapPopulationMesh;
    private WorldMapOceanMesh m_oceanMesh;
    private WorldMapGraph m_worldMapGraph;
    private bool m_isStepping;
    
    /**
     *Public Methods
     */
    public void init(int mapSize, float baseSeedProbability, float stepSpeed, int stabilizationPeriod, int mapScale)
    {
        m_worldMapPopulationMesh = gameObject.AddComponent<WorldMapLandMesh>();
        m_worldMapGraph = gameObject.AddComponent<WorldMapGraph>();
        m_worldMapPopulationMesh.init(mapSize, mapScale);
        m_worldMapGraph.init(mapSize, baseSeedProbability, stabilizationPeriod, mapScale);

        m_isStepping = true;
        StartCoroutine(step(stepSpeed, mapScale));

        GameObject ocean = new GameObject();
        ocean.name = "Ocean";
        ocean.transform.parent = transform.parent;
        ocean.AddComponent<MeshRenderer>();
        ocean.AddComponent<MeshFilter>();
        m_oceanMesh = ocean.AddComponent<WorldMapOceanMesh>();
        m_oceanMesh.init(mapSize, mapScale);
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

    private IEnumerator step(float stepSpeed, int mapScale)
    {
        while (m_isStepping == true)
        {
            WorldMapVertex[,] worldMapVertices = m_worldMapGraph.step();
            m_worldMapPopulationMesh.updateMeshData(worldMapVertices, mapScale);
            yield return new WaitForSeconds(stepSpeed);
        }
    }

    private void OnDrawGizmos()
    {

    }
}
