using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraController
{
    void processLeftAxis(float xMagnitude, float zMagnitude);
    void processRightAxis(float xMagnitude, float yMagnitude);
}
