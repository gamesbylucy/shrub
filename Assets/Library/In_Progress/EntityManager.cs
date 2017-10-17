using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    /**
     * @brief   The builder class which EntityManager uses to get Entities
     */
    private EntityBuilder m_entityBuilder;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void initialize()
    {
        m_entityBuilder = new EntityBuilder();
    }

    public void setupInitialEntities()
    {
        //check to see if the world has finished being generated.
        //determine the nodes which are clear for spawning entities
        //spawn the initial entities
        //initalize the entities behavior
    }

    /**
     * @param type The type of entity to be generated.
     * @param location The location at which to generate the entity.
     */
    public void generateEntity(Enumerations.EntityTypes type, Vector3 location)
    {

    }

    /**
     * @brief WIP - will likely make a special Cluster object to pass into this method
     * Planned cluster object should be a pure data object which can accept a maximum size,
     * the amount of types of entites, and the percentage of the group that will compose
     * the cluster.
     */
    public void generateHomogeneousEntityCluster()
    {

    }

    /**
     * @brief WIP - will likely make a special Cluster object to pass into this method
     * Planned cluster object should be a pure data object which can accept a maximum size,
     * the amount of types of entites, and the percentage of the group that will compose
     * the cluster.
     */
    public void generateHeterogenousEntityCluster()
    {

    }
}
