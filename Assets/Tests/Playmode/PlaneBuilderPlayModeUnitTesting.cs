using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlaneBuilderPlayModeUnitTesting {

	[Test]
	public void PlaneBuilderPlayModeUnitTestingSimplePasses() {
        // Use the Assert class to test conditions.
        PlaneBuilder builder = new PlaneBuilder();
        GameObject thePlane = builder.get();
        thePlane.GetComponent<Renderer>().material = Resources.Load("Materials/Animal") as Material;

        Assert.That(thePlane != null, "thePlane was null");
        Assert.That(thePlane.GetComponent<MeshFilter>() != null);
        Assert.That(thePlane.GetComponent<Renderer>() != null);
        Assert.That(thePlane.GetComponent<MeshCollider>() != null);
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator PlaneBuilderPlayModeUnitTestingWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
