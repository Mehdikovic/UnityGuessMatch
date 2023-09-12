using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class Util {
    static public T[] Shuffle<T>(T[] array, int seed) {
        if (array == null || array.Length < 2) { return array; }

        System.Random rand = new(seed);
        for (int i = 0; i < array.Length - 1; ++i) {
            int randomIndex = rand.Next(i, array.Length);
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
        }
        return array;
    }

    static public T Shuffle<T>(T list, int seed) where T : IList {
        if (list == null || list.Count < 2) { return list; }

        System.Random rand = new(seed);
        for (int i = 0; i < list.Count - 1; ++i) {
            int randomIndex = rand.Next(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
        return list;
    }

    static public float Map(float value, float fromMin, float fromMax, float toMin, float toMax) {
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    static public int Map(int value, int fromMin, int fromMax, int toMin, int toMax) {
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    static public float GetVec2Angle(Vector3 vec3) {
        return Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg;
    }

    static public float GetVec2Angle(Vector2 vec2) {
        return Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;
    }

    static public Vector2 GetRandomDirVec2() {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    static public Vector3 GetRandomDirVec3() {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    static public int LayerMaskToLayer(int layerMask) {
        return (int) Mathf.Log(layerMask, 2f);
    }

    static public int LayerMaskToLayer(string layerName) {
        return LayerMask.NameToLayer(layerName);
    }

    static public int LayerToLayerMask(int layer) {
        return (int) Mathf.Pow(2f, layer);
    }

    static public int GetValueWithPercentageShare(int value, float percentage) {
        return value + (int) (value * percentage / 100f);
    }

    static public float GetValueWithPercentageShare(float value, float percentage) {
        return value + (value * percentage / 100f);
    }

    static public T GetByLevel<T>(T[] values, int level) {
        if (level <= 0 || values == null || values.Length == 0) { return default; }
        if (level > values.Length) { return values[^1]; }
        return values[level - 1];
    }

    static public int GetIndexOfLevel<T>(T[] values, int level) {
        if (level <= 0 || values == null || values.Length == 0 || level > values.Length) { throw new System.IndexOutOfRangeException(); }
        return level - 1;
    }

    static public float DivideFloat(float a, float b) {
        return b == 0f ? float.MaxValue : b == float.MaxValue ? 0f : a / b;
    }

    static public float ConvertRateToTime(float rate) {
        return DivideFloat(1f, rate);
    }

    static public float ConvertFerequencyToPeriod(float rate) {
        return ConvertRateToTime(rate);
    }

    static public float ConvertToDecibel(float volume) {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        return Mathf.Log10(volume) * 20f;
    }


    static public int Sign(float value) {
        return value >= 0f ? 1 : -1;
    }

    static public int Sign(int value) {
        return value >= 0 ? 1 : -1;
    }

    static public bool StringEquals(this string right, string left) {
        return string.Equals(right, left, System.StringComparison.InvariantCultureIgnoreCase);
    }

    static public int StringCompare(this string right, string left) {
        return string.Compare(right, left, System.StringComparison.InvariantCultureIgnoreCase);
    }

    static public bool StringContains(this string right, string value) {
        return right.Contains(value, System.StringComparison.InvariantCultureIgnoreCase);
    }

    static public bool StringContains(this string right, char value) {
        return right.Contains(value, System.StringComparison.InvariantCultureIgnoreCase);
    }

    static public bool HasElement<T>(this IEnumerable<T> enumerable) {
        return enumerable != null && enumerable.Count() > 0;
    }

    static public bool HasElement<T>(this T list) where T : ICollection {
        return list != null && list.Count > 0;
    }

    static public bool HasElement<T>(this T[] list) {
        return list != null && list.Length > 0;
    }

    static public void SafeAddRange<T>(this List<T> list, IEnumerable<T> newListToAdd) {
        if (newListToAdd == null || newListToAdd.Count() == 0) { return; }
        list.AddRange(newListToAdd);
    }

    static public void SafeForEach<T>(this IEnumerable<T> enumerable, System.Action<T> action) {
        if (enumerable.HasElement()) {
            enumerable.ForEach(action);
        }
    }

    static public void ErrIfNull(UnityEngine.Object unityObject, string nameof, GameObject gameObject, [CallerMemberName] string callerMemberName = null) {
        if (unityObject == null) { Debug.LogError($"called in ::{callerMemberName} - {nameof} in {gameObject.name} can not be null!"); }
    }
}