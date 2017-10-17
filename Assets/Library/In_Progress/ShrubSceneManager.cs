using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrubSceneManager : MonoBehaviour {
    /**
     * brief    The manager class which SceneManager uses to get entities for the scene.
     */
    private EntityManager m_entityManager;

    /**
     * @brief   The manager class which SceneManager uses to get landscapes for the scene.
     */
    private LandscapeManager m_landscapeManager;

	// Use this for initialization
	void Start () {
        /**
         * Call DontDestroyOnLoad to prevent this script and the attached empty GameObject
         * from being destroyed between scene changes.
         */
        DontDestroyOnLoad(this);

        m_entityManager = new EntityManager();
        m_landscapeManager = new LandscapeManager();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Subscribes the manager to events in the EventSignals messenger class. For a detailed description
     * of events, see the EventSignals class.
     * 
     * @return  Returns true if all events were successfully subscribed to. Otherwise returns false.
     */
    public bool subscribeToEvents()
    {
        //method stub
        return true;
    }
}
