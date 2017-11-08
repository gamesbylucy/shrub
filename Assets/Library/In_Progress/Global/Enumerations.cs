using System.Collections;
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
        Unpopulated = 0,
        Populated = 1,
        Stable = 2,
        Potential_Complex = 3,
        Complex = 4,
        Border = 5,
    }

    public enum InputModes
    {
        Menu = 0,
        Game = 1,
        Suspend = 2
    }
}
