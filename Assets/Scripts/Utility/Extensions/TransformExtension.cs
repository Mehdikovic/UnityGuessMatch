using UnityEngine;

public static class TransformExtension {
    public static Vector2 ToVec2(this Transform transform) {
        return transform.position.ToVec2();
    }

    public static Vector2 ToVec2EvalZ(this Transform transform) {
        return transform.position.ToVec2EvalZ();
    }

    public static Vector3 OnlyX(this Transform transform) {
        return transform.position.OnlyX();
    }

    public static Vector3 OnlyY(this Transform transform) {
        return transform.position.OnlyY();
    }

    public static Vector3 OnlyZ(this Transform transform) {
        return transform.position.OnlyZ();
    }

    public static Vector3 WithX(this Transform transform, float x) {
        return transform.position.WithX(x);
    }

    public static Vector3 WithY(this Transform transform, float y) {
        return transform.position.WithY(y);
    }

    public static Vector3 WithZ(this Transform transform, float z) {
        return transform.position.WithZ(z);
    }

    public static Vector3 FlatX(this Transform transform) {
        return transform.position.FlatX();
    }

    public static Vector3 FlatY(this Transform transform) {
        return transform.position.FlatY();
    }

    public static Vector3 FlatZ(this Transform transform) {
        return transform.position.FlatZ();
    }

    public static Vector3 ShitfX(this Transform transform, float x) {
        return transform.position.ShiftX(x);
    }

    public static Vector3 ShiftY(this Transform transform, float y) {
        return transform.position.ShiftY(y);
    }

    public static Vector3 ShiftZ(this Transform transform, float z) {
        return transform.position.ShiftZ(z);
    }

    public static Vector3 Clamp(this Transform transform, float min, float max) {
        float x = Mathf.Clamp(transform.position.x, min, max);
        float y = Mathf.Clamp(transform.position.y, min, max);
        float z = Mathf.Clamp(transform.position.z, min, max);
        return new(x, y, z);
    }

    public static Vector3 Clamp(this Transform transform) {
        float x = Mathf.Clamp01(transform.position.x);
        float y = Mathf.Clamp01(transform.position.y);
        float z = Mathf.Clamp01(transform.position.z);
        return new(x, y, z);
    }

    public static Vector3 ClampX(this Transform transform, float min, float max) {
        return transform.position.ClampX(min, max);
    }

    public static Vector3 ClampX(this Transform transform) {
        return transform.position.ClampX();
    }

    public static Vector3 ClampY(this Transform transform, float min, float max) {
        return transform.position.ClampY(min, max);
    }

    public static Vector3 ClampY(this Transform transform) {
        return transform.position.ClampY();
    }

    public static Vector3 ClampZ(this Transform transform, float min, float max) {
        return transform.position.ClampZ(min, max);
    }

    public static Vector3 ClampZ(this Transform transform) {
        return transform.position.ClampZ();
    }

    public static Vector3 ClampXY(this Transform transform, float min, float max) {
        return transform.position.ClampXY(min, max);
    }

    public static Vector3 ClampXY(this Transform transform) {
        return transform.position.ClampXY();
    }

    public static Vector3 ClampXZ(this Transform transform, float min, float max) {
        return transform.position.ClampXZ(min, max);
    }

    public static Vector3 ClampXZ(this Transform transform) {
        return transform.position.ClampXZ();
    }

    public static Vector3 ClampYZ(this Transform transform, float min, float max) {
        return transform.position.ClampYZ(min, max);
    }

    public static Vector3 ClampYZ(this Transform transform) {
        return transform.position.ClampYZ();
    }

    static public Transform ResetLocalTransform(this Transform transform) {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        return transform;
    }

    public static Vector3 DirectionTo(this Transform from, Transform to) {
        return from.position.DirectionTo(to.position);
    }

    public static Vector3 DirectionTo(this Transform from, Vector3 to) {
        return from.position.DirectionTo(to);
    }

    public static Vector3 DirectionTo(this Transform from, Vector2 to) {
        return from.position.DirectionTo(to);
    }

    public static float SqrDistanceTo(this Transform from, Transform to) {
        return from.position.SqrDistanceTo(to.position);
    }

    public static float SqrDistanceTo(this Transform from, Vector3 to) {
        return from.position.SqrDistanceTo(to);
    }

    public static float DistanceTo(this Transform from, Transform to) {
        return from.position.DistanceTo(to.position);
    }

    public static float DistanceTo(this Transform from, Vector3 to) {
        return from.position.DistanceTo(to);
    }
}
