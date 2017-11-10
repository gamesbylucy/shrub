using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    /**
     * Public Members
     */
    public int mapSize;
    public int mapScale;
    public float secondsToStabilize;
    public float baseSeedProbability;
    public float stepsPerSecond;

    /**
     * Private Members
     */
    private WorldMap m_worldMap;
    private float m_stepSpeed;
    private int m_stabilizationPeriod;
    private const int MAX_MAP_SIZE = 254;

    // Use this for initialization
    void Start () {
        init();
        start();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void init()
    {
        GameObject theWorldMap = new GameObject();
        theWorldMap.name = "WorldMap";
        m_worldMap = theWorldMap.AddComponent<WorldMap>();
        m_stepSpeed = 1 / stepsPerSecond;
        m_stabilizationPeriod =(int)(secondsToStabilize * stepsPerSecond);
    }

    public void load()
    {
        
    }

    public void start()
    {
        if (mapSize > MAX_MAP_SIZE)
        {
            Debug.Log("Map size cannot be greated than 254 in the current implementation. Setting map size to 254");
            mapSize = MAX_MAP_SIZE;
        }
        m_worldMap.init(mapSize, baseSeedProbability, m_stepSpeed, m_stabilizationPeriod, mapScale);
    }

    public void pause()
    {

    }

    public void end()
    {

    }
}
