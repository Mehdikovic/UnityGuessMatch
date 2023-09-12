using UnityEngine;

public static class Vector3Extension {
    public static Vector3 ToVec3(this Vector2 vec2) {
        return new(vec2.x, vec2.y, 0f);
    }

    public static Vector3 ToVec3(this Vector2 vec2, float z) {
        return new(vec2.x, vec2.y, z);
    }

    public static Vector3 ToVec3EvalZ(this Vector2 vec2) {
        return new(vec2.x, 0f, vec2.y);
    }

    public static Vector3 ToVec3EvalZ(this Vector2 vec2, float y) {
        return new(vec2.x, y, vec2.y);
    }

    public static Vector2 ToVec2(this Vector3 vec3) {
        return new(vec3.x, vec3.y);
    }

    public static Vector2 ToVec2EvalZ(this Vector3 vec3) {
        return new(vec3.x, vec3.z);
    }

    public static Vector3 OnlyX(this Vector3 vec3) {
        return new(vec3.x, 0f, 0f);
    }

    public static Vector3 OnlyY(this Vector3 vec3) {
        return new(0f, vec3.y, 0f);
    }

    public static Vector3 OnlyZ(this Vector3 vec3) {
        return new(0f, 0f, vec3.z);
    }

    public static Vector3 WithX(this Vector3 vec3, float x) {
        return new(x, vec3.y, vec3.z);
    }

    public static Vector3 WithY(this Vector3 vec3, float y) {
        return new(vec3.x, y, vec3.z);
    }

    public static Vector3 WithZ(this Vector3 vec3, float z) {
        return new(vec3.x, vec3.y, z);
    }

    public static Vector3 FlatX(this Vector3 vec3) {
        return new(0f, vec3.y, vec3.z);
    }

    public static Vector3 FlatY(this Vector3 vec3) {
        return new(vec3.x, 0f, vec3.z);
    }

    public static Vector3 FlatZ(this Vector3 vec3) {
        return new(vec3.x, vec3.y, 0f);
    }

    public static Vector3 ShiftX(this Vector3 vec3, float shiftedX) {
        return new Vector3(vec3.x + shiftedX, vec3.y, vec3.z);
    }

    public static Vector3 ShiftY(this Vector3 vec3, float shiftedY) {
        return new Vector3(vec3.x, vec3.y + shiftedY, vec3.z);
    }

    public static Vector3 ShiftZ(this Vector3 vec3, float shiftedZ) {
        return new Vector3(vec3.x, vec3.y, vec3.z + shiftedZ);
    }

    public static bool IsZero(this Vector3 vec3) {
        return Mathf.Approximately(vec3.sqrMagnitude, 0f);
    }

    public static bool IsZero(this Vector2 vec2) {
        return Mathf.Approximately(vec2.sqrMagnitude, 0f);
    }

    public static bool IsNotZero(this Vector3 vec3) {
        return !IsZero(vec3);
    }

    public static bool IsNotZero(this Vector2 vec2) {
        return !IsZero(vec2);
    }

    public static Vector3 Clamp(this Vector3 vec3, float min, float max) {
        float x = Mathf.Clamp(vec3.x, min, max);
        float y = Mathf.Clamp(vec3.y, min, max);
        float z = Mathf.Clamp(vec3.z, min, max);
        return new(x, y, z);
    }

    public static Vector3 Clamp(this Vector3 vec3) {
        float x = Mathf.Clamp01(vec3.x);
        float y = Mathf.Clamp01(vec3.y);
        float z = Mathf.Clamp01(vec3.z);
        return new(x, y, z);
    }

    public static Vector3 ClampX(this Vector3 vec3, float min, float max) {
        float x = Mathf.Clamp(vec3.x, min, max);
        return new(x, vec3.y, vec3.z);
    }

    public static Vector3 ClampX(this Vector3 vec3) {
        float x = Mathf.Clamp01(vec3.x);
        return new(x, vec3.y, vec3.z);
    }

    public static Vector3 ClampY(this Vector3 vec3, float min, float max) {
        float y = Mathf.Clamp(vec3.y, min, max);
        return new(vec3.x, y, vec3.z);
    }

    public static Vector3 ClampY(this Vector3 vec3) {
        float y = Mathf.Clamp01(vec3.y);
        return new(vec3.x, y, vec3.z);
    }

    public static Vector3 ClampZ(this Vector3 vec3, float min, float max) {
        float z = Mathf.Clamp(vec3.z, min, max);
        return new(vec3.x, vec3.y, z);
    }

    public static Vector3 ClampZ(this Vector3 vec3) {
        float z = Mathf.Clamp01(vec3.z);
        return new(vec3.x, vec3.y, z);
    }

    public static Vector3 ClampXY(this Vector3 vec3, float min, float max) {
        float x = Mathf.Clamp(vec3.x, min, max);
        float y = Mathf.Clamp(vec3.y, min, max);
        return new(x, y, vec3.z);
    }

    public static Vector3 ClampXY(this Vector3 vec3) {
        float x = Mathf.Clamp01(vec3.x);
        float y = Mathf.Clamp01(vec3.y);
        return new(x, y, vec3.z);
    }

    public static Vector3 ClampXZ(this Vector3 vec3, float min, float max) {
        float x = Mathf.Clamp(vec3.x, min, max);
        float z = Mathf.Clamp(vec3.z, min, max);
        return new(x, vec3.y, z);
    }

    public static Vector3 ClampXZ(this Vector3 vec3) {
        float x = Mathf.Clamp01(vec3.x);
        float z = Mathf.Clamp01(vec3.z);
        return new(x, vec3.y, z);
    }

    public static Vector3 ClampYZ(this Vector3 vec3, float min, float max) {
        float y = Mathf.Clamp(vec3.y, min, max);
        float z = Mathf.Clamp(vec3.z, min, max);
        return new(vec3.x, y, z);
    }

    public static Vector3 ClampYZ(this Vector3 vec3) {
        float y = Mathf.Clamp01(vec3.y);
        float z = Mathf.Clamp01(vec3.z);
        return new(vec3.x, y, z);
    }

    public static Vector2 Clamp(this Vector2 vec2, float min, float max) {
        float x = Mathf.Clamp(vec2.x, min, max);
        float y = Mathf.Clamp(vec2.y, min, max);
        return new(x, y);
    }

    public static Vector2 Clamp(this Vector2 vec2) {
        float x = Mathf.Clamp01(vec2.x);
        float y = Mathf.Clamp01(vec2.y);
        return new(x, y);
    }

    public static Vector2 ClampX(this Vector2 vec2, float min, float max) {
        float x = Mathf.Clamp(vec2.x, min, max);
        return new(x, vec2.y);
    }

    public static Vector2 ClampX(this Vector2 vec2) {
        float x = Mathf.Clamp01(vec2.x);
        return new(x, vec2.y);
    }

    public static Vector2 ClampY(this Vector2 vec2, float min, float max) {
        float y = Mathf.Clamp(vec2.y, min, max);
        return new(vec2.x, y);
    }

    public static Vector2 ClampY(this Vector2 vec2) {
        float y = Mathf.Clamp01(vec2.y);
        return new(vec2.x, y);
    }

    public static Vector3 CalibrateLessThan(this Vector3 vec, float minCalibrationValue, float value) {
        float calibratedX = (vec.x >= minCalibrationValue || vec.x <= -minCalibrationValue) ? vec.x : value;
        float calibratedY = (vec.y >= minCalibrationValue || vec.y <= -minCalibrationValue) ? vec.y : value;
        float calibratedZ = (vec.z >= minCalibrationValue || vec.z <= -minCalibrationValue) ? vec.z : value;
        return new Vector3(calibratedX, calibratedY, calibratedZ);
    }

    public static Vector3 CalibrateMoreThan(this Vector3 vec, float maxCalibrationValue, float value) {
        float calibratedX = (vec.x <= maxCalibrationValue || vec.x >= -maxCalibrationValue) ? vec.x : value;
        float calibratedY = (vec.y <= maxCalibrationValue || vec.y >= -maxCalibrationValue) ? vec.y : value;
        float calibratedZ = (vec.z <= maxCalibrationValue || vec.z >= -maxCalibrationValue) ? vec.z : value;
        return new Vector3(calibratedX, calibratedY, calibratedZ);
    }

    public static Vector2 CalibrateLessThan(this Vector2 vec, float minCalibrationValue, float value) {
        float calibratedX = (vec.x >= minCalibrationValue || vec.x <= -minCalibrationValue) ? vec.x : value;
        float calibratedY = (vec.y >= minCalibrationValue || vec.y <= -minCalibrationValue) ? vec.y : value;
        return new Vector2(calibratedX, calibratedY);
    }

    public static Vector2 CalibrateMoreThan(this Vector2 vec, float maxCalibrationValue, float value) {
        float calibratedX = (vec.x <= maxCalibrationValue || vec.x >= -maxCalibrationValue) ? vec.x : value;
        float calibratedY = (vec.y <= maxCalibrationValue || vec.y >= -maxCalibrationValue) ? vec.y : value;
        return new Vector2(calibratedX, calibratedY);
    }

    public static Vector3 DirectionTo(this Vector3 from, Vector3 to) {
        return (to - from).normalized;
    }

    public static Vector3 DirectionTo(this Vector3 from, Vector2 to) {
        return (to - from.ToVec2()).normalized;
    }

    public static float SqrDistanceTo(this Vector3 fromPosition, Vector3 toPosition) {
        return (toPosition - fromPosition).sqrMagnitude;
    }

    public static float DistanceTo(this Vector3 fromPosition, Vector3 toPosition) {
        return (toPosition - fromPosition).magnitude;
    }

    public static Vector3 DirectionTo(this Vector2 from, Vector2 to) {
        return (to - from).normalized;
    }

    public static float SqrDistanceTo(this Vector2 fromPosition, Vector2 toPosition) {
        return (toPosition - fromPosition).sqrMagnitude;
    }

    public static float DistanceTo(this Vector2 fromPosition, Vector2 toPosition) {
        return (toPosition - fromPosition).magnitude;
    }
}
