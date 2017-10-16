using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ShrubUnitTesting
{
    [TestClass]
    public class EntityBuilderUnitTesting
    {
        [TestMethod]
        public void testGet()
        {
            //arrange
            EntityBuilder builder = new EntityBuilder();
            Entity animalEntity;
            Entity humanEntity;
            Entity houseEntity;

            //act
            animalEntity = builder.get(Enumerations.EntityTypes.Animal);
            humanEntity = builder.get(Enumerations.EntityTypes.Human);
            houseEntity = builder.get(Enumerations.EntityTypes.House);

            //assert
            Assert.IsTrue(animalEntity.GetType() == typeof(Animal), "Get method Animal enum returned " + animalEntity.GetType() + ".");
            Assert.IsTrue(humanEntity.GetType() == typeof(Human), "Get method Human enum returned " + humanEntity.GetType() + ".");
            Assert.IsTrue(humanEntity.GetType() == typeof(House), "Get method House enum returned " + houseEntity.GetType() + ".");

        }
    }
}
