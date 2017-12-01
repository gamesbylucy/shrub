﻿using System.Collections;
using System.Collections.Generic;

public static class Enumerations {

    /**
     * @brief Enumeration to support the seeding of the node population.
     */
    public enum SeedTypes
    {
        Random = 0,
        Stable = 1,
        Chaotic = 2
    }

    public enum CardinalDirections
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public enum States
    {
        Ocean = 0,
        UnstableLand = 1,
        StableLand = 2,
        Potential_Complex_Land = 3,
        Complex_Land = 4,
        Border = 5,
    }

    public enum InputModes
    {
        Menu = 0,
        Game = 1,
        Suspend = 2
    }
}
