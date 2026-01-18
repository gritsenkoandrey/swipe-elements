using System;
using System.Collections.Generic;

namespace SwipeElements.Utils
{
    public static class CollectionExtension
    {
        public static T GetRandomElement<T>(this IList<T> collection)
        {
            int count = collection.Count;
            
            if (count == 0)
            {
                throw new InvalidOperationException("LIST IS EMPTY");
            }

            int index = UnityEngine.Random.Range(0, count);
            
            T element = collection[index];

            return element;
        }
    }
}