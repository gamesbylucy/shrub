using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EntityBuilderPlayModeUnitTesting {

	[Test]
	public void EntityBuilderPlayModeUnitTestingSimplePasses() {
        // Use the Assert class to test conditions.

        //setup
        GameObject manager = new GameObject();
        manager.AddComponent<EntityBuilder>();
        manager.GetComponent<EntityBuilder>().initialize();
        Entity animal = manager.GetComponent<EntityBuilder>().get(Enumerations.EntityTypes.Animal);
        Entity human = manager.GetComponent<EntityBuilder>().get(Enumerations.EntityTypes.Human);
        Entity house = manager.GetComponent<EntityBuilder>().get(Enumerations.EntityTypes.House);

        //assert that objects are their intended subclasses
        Assert.That(animal is Animal, "animal is not an instance of Animal.");
        Assert.That(human is Human, "human is not an instance of Human.");
        Assert.That(house is House, "house is not an instance of House.");

        //assert that objects are not also unintended subclasses
        Assert.False(animal is Human, "animal is an instance of Human.");
        Assert.False(animal is House, "animal is an instance of House.");
        Assert.False(human is Animal, "human is an instance of Animal");
        Assert.False(human is House, "human is an instance of House");
        Assert.False(house is Animal, "house is an instance of Animal");
        Assert.False(house is Human, "house is an instance of Human");
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator EntityBuilderPlayModeUnitTestingWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
