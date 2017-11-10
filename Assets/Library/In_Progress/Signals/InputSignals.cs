using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputSignals {
    public static event Action<KeyCode> onInteractButtonDown;
    public static void interactButtonDown(KeyCode button)
    {
        if (onInteractButtonDown != null)
        {
            onInteractButtonDown(button);
        }
    }

    public static event Action<float, float> onPlayerMovement;
    public static void playerMovement(float xMagnitude, float zMagnitude)
    {
        if (onPlayerMovement != null)
        {
            onPlayerMovement(xMagnitude, zMagnitude);
        }
    }

    public static event Action<float, float> onPlayerRotation;
    public static void playerRotation(float xMagnitude, float zMagnitude)
    {
        if (onPlayerRotation != null)
        {
            onPlayerRotation(xMagnitude, zMagnitude);
        }
    }
}
