using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBuilder : MonoBehaviour {

    /**
     * @brief   Member dictionary class used by entity builder class to encapsulate the process
     * of returning an entities constituent parts.
     */
    private Dictionary<Enumerations.EntityTypes, Func<Entity>> m_entityDictionary = new Dictionary<Enumerations.EntityTypes, Func<Entity>>();

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void initialize()
    {
        /**initialize the dictionary with EntityType enum keys, and values which return methods corresponding
         * to a given entities build process
         */
        m_entityDictionary.Add(Enumerations.EntityTypes.Animal, () => buildAnimal());
        m_entityDictionary.Add(Enumerations.EntityTypes.Human, () => buildHuman());
        m_entityDictionary.Add(Enumerations.EntityTypes.House, () => buildHouse());
    }

    public Entity get(Enumerations.EntityTypes entityType)
    {
        var factory = m_entityDictionary[entityType];
        return factory();
    }

    /**
     * @return  A fully initialized animal gameobject with attached scripts.
     */
    private Animal buildAnimal()
    {
        Animal theAnimal = new Animal();
        return theAnimal;
    }

    /**
     * @return  A fully initialized animal gameobject with attached scripts.
     */
    private Human buildHuman()
    {
        Human theHuman = new Human();
        return theHuman;
    }

    /**
     * @return  A fully initialized animal gameobject with attached scripts.
     */
    private House buildHouse()
    {
        House theHouse = new House();
        return theHouse;
    }
}
