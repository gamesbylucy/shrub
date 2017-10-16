using System.Collections;
using System.Collections.Generic;

public static class Enumerations {

    /**
     * @brief   Enumeration to support selection of entity types for methods
     */
    public enum EntityTypes
    {
        Human = 0,
        Animal = 1,
        House = 2
    }

    /**
     * @brief   Enumeration to support selection of entity types for methods
     */
    public enum LandscapeTypes
    {
        River = 0,
        Mountain = 1,
        Plain = 2,
        Forest = 3
    }

    public enum CardinalDirections
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public enum Elevations
    {
        Valley = 0,
        Sea_Level = 1,
        Alpine = 2,
        Above_Alpine = 3,
        Sky = 4
    }
}
