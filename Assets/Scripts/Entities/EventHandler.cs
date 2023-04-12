using System;
using System.Collections.Generic;
using Entities.Events;

namespace Entities
{
    /// <summary>
    /// Simple observable pattern for struct events, uses a dictionary of fast access
    /// </summary>
    public class EventHandler
    {
        private readonly Dictionary<Type, List<object>> callbackDictionary = new();

        public void Send<T>(T evt) where T : struct, IEntityEvent
        {
            var type = typeof(T);

            if (!callbackDictionary.ContainsKey(type)) return;

            var callbackList = callbackDictionary[type];
            foreach (var callback in callbackList)
            {
                ((Action<T>)callback).Invoke(evt);
            }
        }
        
        public void On<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!callbackDictionary.ContainsKey(type))
            {
                callbackDictionary.Add(type, new List<object> { callback });
            }
            else
            {
                var callbackList = callbackDictionary[type];
                callbackList.Add(callback);
            }
        }
    }
}