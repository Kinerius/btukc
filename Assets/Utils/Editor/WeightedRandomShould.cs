using NSubstitute;
using System;
using System.Linq;
using NUnit.Framework;

namespace Utils.Editor
{
    public class WeightedRandomShould
    {
        [TestCase(0, "a")]
        [TestCase(10, "a")]
        [TestCase(20, "a")]
        [TestCase(21, "b")]
        [TestCase(25, "b")]
        [TestCase(26, "c")]
        [TestCase(32, "c")]
        [TestCase(33, "d")]
        public void Return_Desired_Weighted_Value(int randomIndex, string expectedValue)
        {
            var random = Substitute.For<Random>();
            var weightedObjects = new[]
            {
                new WeightedObject<string>("a", 20),
                new WeightedObject<string>("b", 5),
                new WeightedObject<string>("c", 7),
                new WeightedObject<string>("d", 1),
            };

            var totalWeight = weightedObjects.Sum(v => v.Weight);
            random.NextDouble().Returns((double)randomIndex / totalWeight );

            var value = WeightedRandom.PickOne(weightedObjects, random);

            Assert.AreEqual(expectedValue, value);
        }
    }
}
