using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    /**
     * Public Members
     */
    public int mapSize;
    public int stabilizationPeriod;
    public float baseSeedProbability;
    public float stepSpeed;

    /**
     * Private Members
     */
    private WorldMap m_worldMap;

	// Use this for initialization
	void Start () {
        load();
        start();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void load()
    {
        GameObject theWorldMap = new GameObject();
        theWorldMap.name = "WorldMap";
        m_worldMap = theWorldMap.AddComponent<WorldMap>();
    }

    public void start()
    {
        m_worldMap.init(mapSize, baseSeedProbability, stepSpeed, stabilizationPeriod);
    }

    public void pause()
    {

    }

    public void end()
    {

    }
}
