using System;
using System.Collections.Generic;

namespace TAP.Core.DatastoreORM
{
    public static class CommonExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }

        public static void AddRange<K, V>(this IDictionary<K, V> baseDictionary, IDictionary<K, V> copiedDictionary)
        {
            foreach (var metadata in copiedDictionary)
            {
                if (!baseDictionary.ContainsKey(metadata.Key))
                {
                    baseDictionary.Add(metadata.Key, metadata.Value);
                }
            }
        }
    }
}
