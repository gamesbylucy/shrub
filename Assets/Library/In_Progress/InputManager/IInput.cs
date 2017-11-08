using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonInput {
    void processInputDown(KeyCode input);
    void processInputHeld(KeyCode input);
    void processInputUp(KeyCode input);
}
