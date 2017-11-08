using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    /**
     * Public Members
     */
    public int mapSize;
    public float secondsToStabilize;
    public float baseSeedProbability;
    public float stepsPerSecond;

    /**
     * Private Members
     */
    private WorldMap m_worldMap;
    private float m_stepSpeed;
    private int m_stabilizationPeriod;

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
        m_worldMap.init(mapSize, baseSeedProbability, m_stepSpeed, m_stabilizationPeriod);
    }

    public void pause()
    {

    }

    public void end()
    {

    }
}
