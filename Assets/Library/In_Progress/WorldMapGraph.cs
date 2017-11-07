/**
 * @brief T Luciano. A undirected graph which is read in row major order, which represents the shrub world map.
 * 
 * The graph will always be initialized as a square n x n LandscapeVertex grid where n is the mapSize argument in the
 * public init method. Each LandscapeVertex has a current state and a next state, which is calculated by the public
 * step method.
 * 
 * Two interfaces into the method, one in and one out. init accepts a size and probability that any given
 * vertex starts populated. step returns the graph after its next state has been calculated. This data can be used
 * to update the graphical components of the game or to calculate the behavior of agents in the world.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapGraph : MonoBehaviour {
    /**
     * Public Members
     */

    /**
     * Private Members
     */
    private WorldMapVertex[,] m_worldMapVertices;
    
    /**
     * Static
     */
    private static int[][] directions = new int[][]
    {
        new int[]{ -1, -1 }, //SW
        new int[] { -1, 0 }, //W
        new int[] { -1, 1 }, //NW
        new int[] { 0, 1 },  //N
        new int[] { 1, 1 },  //NE
        new int[] { 1, 0 },  //E
        new int[] { 1, -1 }, //SE
        new int[] { 0, -1 }  //S
    };

    /**
     * Public Methods
     */
    public void init(int mapSize, float baseSeedProbability)
    {
        setVertices(mapSize);
        assignVertexNeighbors(m_worldMapVertices);
        checkForLandscapeBorders(m_worldMapVertices);
        setSeedProbability(m_worldMapVertices, baseSeedProbability);
        seedPopulation(Enumerations.SeedTypes.Random);
    }

    public WorldMapVertex[ , ] step()
    {
        updateFlags();
        setNextStates();
        setNextComplexes();
        updateDecals();
        return m_worldMapVertices;
    }

    /**
     * Private Members
     */
    private void setVertices(int mapSize)
    {
        m_worldMapVertices = new WorldMapVertex[mapSize + 1, mapSize + 1];
        for (int i = 0, y = 0; y <= mapSize; y++)
        {
            for (int x = 0; x <= mapSize; x++, i++)
            {
                WorldMapVertex theVertex = new WorldMapVertex();
                theVertex.vertexIndex = i;
                theVertex.position = new Vector3(x, 0, y);
                m_worldMapVertices[y, x] = theVertex;
            }
        }
    }

    private void updateFlags()
    {
        foreach (WorldMapVertex node in m_worldMapVertices)
        {
            node.setFlags();
        }
    }

    private void setNextStates()
    {
        foreach (WorldMapVertex node in m_worldMapVertices)
        {
            node.setNextState();
        }
    }

    private void setNextComplexes()
    {
        List<WorldMapVertex> potentialComplexes = new List<WorldMapVertex>();
        /**
        * Add all the potential complex nodes to a list.
        */
        foreach (WorldMapVertex vertex in m_worldMapVertices)
        {
            if (vertex.state == Enumerations.States.Potential_Complex)
            {
                potentialComplexes.Add(vertex);
            }
        }

        /**
         * Give each potential complex an random ranking.
         */
        foreach (WorldMapVertex potentialComplex in potentialComplexes)
        {
            potentialComplex.rank = (float)ShrubUtils.random.NextDouble();
        }

        /**
         * Scan over all potential complexes.
         */
        foreach (WorldMapVertex potentialComplex in potentialComplexes)
        {
            foreach (WorldMapVertex neighbor in potentialComplex.neighbors)
            {
                if (neighbor.state == Enumerations.States.Potential_Complex)
                {
                    if (neighbor.rank > potentialComplex.rank)
                    {
                        continue;
                    }
                    else if (neighbor.rank == potentialComplex.rank)
                    {
                        if (.5f > (float)ShrubUtils.random.NextDouble())
                        {
                            continue;
                        }
                        else
                        {
                            potentialComplex.complexity += 2;
                        }
                    }
                    else
                    {
                        potentialComplex.complexity += 2;
                    }
                }
                else if (neighbor.state == Enumerations.States.Stable)
                {
                    potentialComplex.complexity++;
                }
            }

            if (potentialComplex.complexity == 0)
            {
                continue;
            }
            else
            {
                potentialComplex.state = Enumerations.States.Complex;
                foreach (WorldMapVertex neighbor in potentialComplex.neighbors)
                {
                    neighbor.state = Enumerations.States.Border;
                }
            }
        }
    }

    private void updateDecals()
    {
        foreach (WorldMapVertex node in m_worldMapVertices)
        {
            node.setDecal();
        }
    }

    private void assignVertexNeighbors(WorldMapVertex[ , ] theVertices)
    {
        for (int y = 0; y < theVertices.GetLength(0); y++)
        {
            for (int x = 0; x < theVertices.GetLength(1); x++)
            {
                theVertices[y, x].neighbors = getNeighbors(theVertices, y, x);
            }
        }
    }

    private List<WorldMapVertex> getNeighbors(WorldMapVertex [ , ] nodes, int y, int x)
    {
        List<WorldMapVertex> theNodes = new List<WorldMapVertex>();

        foreach (int[] direction in directions)
        {
            int cx = x + direction[0];
            int cy = y + direction[1];
            if (cy >= 0 && cy < nodes.GetLength(0))
            {
                if (cx >= 0 && cx < nodes.GetLength(1))
                {
                    theNodes.Add(nodes[cy, cx]);
                }
            }
        }
        return theNodes;
    }

    private void checkForLandscapeBorders(WorldMapVertex[ , ] landscape)
    {
        foreach (WorldMapVertex node in landscape)
        {
            if (node.neighbors.Count < 8)
            {
                node.state = Enumerations.States.Border;
            }
        }
    }

    private void setSeedProbability(WorldMapVertex[ , ] landscape, float baseSeedProbability)
    {
        foreach (WorldMapVertex node in landscape)
        {
            bool hasLandscapeBorderNeighbor = false;
            foreach (WorldMapVertex neighbor in node.neighbors)
            {
                if (neighbor.state == Enumerations.States.Border)
                {
                    hasLandscapeBorderNeighbor = true;
                    break;
                }
            }

            if (node.state == Enumerations.States.Border)
            {
                node.initialSeedProbability = 0;
            }
            else if (hasLandscapeBorderNeighbor == true)
            {
                node.initialSeedProbability = baseSeedProbability / 2;
            }
            else
            {
                node.initialSeedProbability = baseSeedProbability;
            }
        }
    }

    private void seedPopulation(Enumerations.SeedTypes seedType)
    {
        switch (seedType)
        {
            case Enumerations.SeedTypes.Random:
                foreach (WorldMapVertex node in m_worldMapVertices)
                {
                    if ((float)ShrubUtils.random.NextDouble() < node.initialSeedProbability)
                    {
                        node.state = Enumerations.States.Populated;
                    }
                }
                break;
            case Enumerations.SeedTypes.Stable:
                break;
            case Enumerations.SeedTypes.Chaotic:
                break;
        }
    }
}
