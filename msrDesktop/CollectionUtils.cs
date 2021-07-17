using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace msrDesktop
{
    public static class CollectionUtils
    {
        /// <summary>
        /// Add a collection to set and return result set (self).
        /// </summary>
        /// <param name="set">The input and destination of merging.</param>
        /// <param name="collectionToAdd">Collection to add.</param>
        /// <returns>Merged result, which is also the input set.</returns>
        public static HashSet<T> AddCollection<T>(this HashSet<T> set, IEnumerable<T> collectionToAdd)
        {
            if (collectionToAdd == null || ReferenceEquals(set, collectionToAdd))
            {
                return set;
            }

            foreach (var ele in collectionToAdd)
            {
                set.Add(ele);
            }

            return set;
        }

        /// <summary>
        /// Add a collection to list and return result list (self).
        /// </summary>
        /// <param name="list">The input and destination of merging.</param>
        /// <param name="collectionToAdd">Collection to add.</param>
        /// <returns>Merged result, which is also the input set.</returns>
        public static List<T> AddCollection<T>(this List<T> list, IEnumerable<T> collectionToAdd)
        {
            if (collectionToAdd == null || ReferenceEquals(list, collectionToAdd))
            {
                return list;
            }

            foreach (var ele in collectionToAdd)
            {
                list.Add(ele);
            }

            return list;
        }

        /// <summary>
        /// Split a text collection (like users) to <see cref="HashSet{T}"/>; If null or empty, return an empty <see cref="HashSet{T}"/>
        /// </summary>
        /// <param name="collectionText">Item collection text, separated by splitPattern.</param>
        /// <param name="ignoreCase">Whether ignore case for result <see cref="HashSet{T}"/></param>
        /// <param name="removeEmpty">Whether skip empty text items.</param>
        /// <param name="splitPattern">Split pattern for multiple items.</param>
        /// <returns>Item set.</returns>
        public static HashSet<string> ToHashSet(this string collectionText, bool ignoreCase = false, bool removeEmpty = true, string splitPattern = @"\s*[,;]\s*")
        {
            if (string.IsNullOrEmpty(collectionText))
            {
                return new HashSet<string>(ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
            }

            var items = Regex.Split(collectionText, splitPattern);
            return new HashSet<string>(
                removeEmpty ? items.Where(e => !string.IsNullOrEmpty(e)) : items,
                ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        /// <summary>
        /// Add a collection to map and return result map (self).
        /// </summary>
        /// <param name="map">The input and destination of merging.</param>
        /// <param name="collectionToAdd">New key-values to be added into map.</param>
        /// <param name="overwriteExisting">Whether to overwrite existing values in map if found same keys.</param>
        /// <returns>Merged result, which is also the input map.</returns>
        public static Dictionary<TKey, TValue> AddCollection<TKey, TValue>(this Dictionary<TKey, TValue> map, IEnumerable<KeyValuePair<TKey, TValue>> collectionToAdd, bool overwriteExisting = true)
        {
            return AddCollection(map as IDictionary<TKey, TValue>, collectionToAdd, overwriteExisting) as Dictionary<TKey, TValue>;
        }

        /// <summary>
        /// Add a collection to map and return result map (self).
        /// </summary>
        /// <param name="map">The input and destination of merging.</param>
        /// <param name="collectionToAdd">New key-values to be added into map.</param>
        /// <param name="overwriteExisting">Whether to overwrite existing values in map if found same keys.</param>
        /// <returns>Merged result, which is also the input map.</returns>
        public static SortedDictionary<TKey, TValue> AddCollection<TKey, TValue>(this SortedDictionary<TKey, TValue> map, IEnumerable<KeyValuePair<TKey, TValue>> collectionToAdd, bool overwriteExisting = true)
        {
            return AddCollection(map as IDictionary<TKey, TValue>, collectionToAdd, overwriteExisting) as SortedDictionary<TKey, TValue>;
        }

        /// <summary>
        /// Add a collection to map and return result map (self).
        /// </summary>
        /// <param name="map">The input and destination of merging.</param>
        /// <param name="collectionToAdd">New key-values to be added into map.</param>
        /// <param name="overwriteExisting">Whether to overwrite existing values in map if found same keys.</param>
        /// <returns>Merged result, which is also the input map.</returns>
        public static IDictionary<TKey, TValue> AddCollection<TKey, TValue>(this IDictionary<TKey, TValue> map, IEnumerable<KeyValuePair<TKey, TValue>> collectionToAdd, bool overwriteExisting = true)
        {
            if (collectionToAdd == null || ReferenceEquals(map, collectionToAdd))
            {
                return map;
            }

            foreach (var (key, value) in collectionToAdd)
            {
                if (overwriteExisting)
                {
                    map[key] = value;
                }
                else
                {
                    if (!map.ContainsKey(key))
                    {
                        map.Add(key, value);
                    }
                }
            }

            return map;
        }

        public static Dictionary<string, object> PropertyNameValueMap(this object obj, bool skipEmptyValueText = false, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            if (obj == null)
            {
                return null;
            }

            var properties = obj.GetType().GetProperties(flags);
            if (skipEmptyValueText)
            {
                properties = properties.Where(a => !string.IsNullOrEmpty(a.GetValue(obj)?.ToString())).ToArray();
            }

            return properties.ToDictionary(a => a.Name, a => a.GetValue(obj));
        }
    }
}
