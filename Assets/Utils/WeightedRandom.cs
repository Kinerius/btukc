using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Utils
{
    public class WeightedObject<T>
    {
        public T Value { get; }
        public int Weight { get; }

        public WeightedObject(T value, int weight)
        {
            Value = value;
            Weight = weight;
        }
    }
    
    public static class WeightedRandom
    {
        public static T PickOne<T>(WeightedObject<T>[] objects, Random random = null)
        {
            if (random == null) random = new Random();
            var totalWeight = objects.Sum(obj => obj.Weight);
            var randIndex = random.NextDouble() * totalWeight;
            var currentWeight = 0;
            
            foreach (var item in objects )
            {
                currentWeight += item.Weight;
                if (randIndex <= currentWeight)
                    return item.Value;
            }

            return default;
        }
        
        public static T PickOne<T>(T[] objects, Func<T, int> weightAction,  Random random = null )
        {
            if (random == null) random = new Random();
            var totalWeight = objects.Sum(weightAction);
            var randIndex = random.NextDouble() * totalWeight;
            var currentWeight = 0;
            
            foreach (var item in objects )
            {
                currentWeight += weightAction(item);
                if (randIndex <= currentWeight)
                    return item;
            }

            return default;
        }

        /*public static void PrintChances(WeightedObject<MonsterData>[] weightedObjects)
        {
            var totalWeight = weightedObjects.Sum(obj => obj.Weight);

            foreach (WeightedObject<MonsterData> weightedObject in weightedObjects)
            {
                Debug.Log($"[({weightedObject.Value.cost}) {weightedObject.Value.name}] {(weightedObject.Weight*100f/totalWeight):#.##}%");
            }
        }*/
    }
}