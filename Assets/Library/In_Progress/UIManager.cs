using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    /**
     * @brief   Member keyboard and mouse input class used by UI manager.
     */
    private KBMInput m_kbmInput;

    /**
     * @brief   Member touch input class used by UI manager.
     */
    private TouchInput m_touchInput;

    /**
     * @brief   Member gamepad and mouse input class used by UI manager.
     */
    private GamepadInput m_gamepadInput;

	// Use this for initialization
	void Start () {
        m_kbmInput = new KBMInput();
        m_touchInput = new TouchInput();
        m_gamepadInput = new GamepadInput();
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
