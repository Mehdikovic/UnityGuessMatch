using System;
using System.Collections.Generic;

public static class DictionaryExtension {
    public static void AddRangeOverride<TKey, TValue>(this IDictionary<TKey, TValue> dic, in Dictionary<TKey, TValue> dicToAdd) {
        dicToAdd.ForEach(x => dic[x.Key] = x.Value);
    }

    public static void AddRangeNewOnly<TKey, TValue>(this IDictionary<TKey, TValue> dic, IDictionary<TKey, TValue> dicToAdd) {
        dicToAdd.ForEach(x => { if (!dic.ContainsKey(x.Key)) dic.Add(x.Key, x.Value); });
    }

    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dic, IDictionary<TKey, TValue> dicToAdd) {
        dicToAdd.ForEach(x => dic.Add(x.Key, x.Value));
    }

    public static bool ContainsKeys<TKey, TValue>(this IDictionary<TKey, TValue> dic, IEnumerable<TKey> keys) {
        bool result = false;
        keys.ForEachOrBreak((x) => { result = dic.ContainsKey(x); return result; });
        return result;
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
        foreach (var item in source)
            action(item);
    }

    public static void ForEachOrBreak<T>(this IEnumerable<T> source, Func<T, bool> func) {
        foreach (var item in source) {
            bool result = func(item);
            if (result) break;
        }
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key) {
        if (dic == null) { return default; }

        if (dic.TryGetValue(key, out TValue value)) {
            return value;
        }

        return default;
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue) {
        if (dic == null) { return defaultValue; }

        if (dic.TryGetValue(key, out TValue value)) {
            return value;
        }

        return defaultValue;
    }

    public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> dic) {
        if (dic == null) { return new Dictionary<TKey, TValue>(); }
        return new Dictionary<TKey, TValue>(dic);
    }
}
