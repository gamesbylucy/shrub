using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public float lookSensitivity;
    public float moveSensitivity;
    public KeyCode interactController;
    public KeyCode interactKeyboard;

    private KBMButtonInput m_kbmInput;
    private GamepadButtonInput m_gamepadInput;
    private AxisInput m_axisInput;
    private Enumerations.InputModes mode;
    private List<IButtonInput> buttonInputs;

	// Use this for initialization
	void Start () {

        m_kbmInput = new KBMButtonInput();
        m_gamepadInput = new GamepadButtonInput();
        m_axisInput = new AxisInput();

        buttonInputs = new List<IButtonInput>();
        buttonInputs.Add(m_kbmInput);
        buttonInputs.Add(m_gamepadInput);

        mode = Enumerations.InputModes.Game;

        updateGamepadButtons();
        updateKeyboardButtons();
	}
	
	// Update is called once per frame
	void Update () {

        /**
         * On any button down or held.
         */
		if(Input.anyKey || Input.anyKeyDown)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    foreach (IButtonInput buttonInput in buttonInputs)
                    {
                        buttonInput.processInputDown(vKey);
                    }
                }
                
                if (Input.GetKey(vKey))
                {
                    foreach (IButtonInput buttonInput in buttonInputs)
                    {
                        buttonInput.processInputHeld(vKey);
                    }
                }
            }
        }

        /**
         * On any button up.
         */
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyUp(vKey))
            {
                foreach (IButtonInput buttonInput in buttonInputs)
                {
                    buttonInput.processInputUp(vKey);
                }
            }
        }

        /**
         * On left axis input.
         */
        m_axisInput.processLeftAxis(Input.GetAxis("Horizontal") * moveSensitivity, Input.GetAxis("Vertical") * moveSensitivity);

        /**
         * On right axis input from mouse.
         */
        m_axisInput.processRightAxis(Input.GetAxis("Mouse X") * lookSensitivity, Input.GetAxis("Mouse Y") * lookSensitivity);
    }

    public void updateGamepadButtons()
    {
        m_gamepadInput.interact = interactKeyboard;
    }

    public void updateKeyboardButtons()
    {
        m_kbmInput.interact = interactController;
    }
}
