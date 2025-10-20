using UnityEngine;

[System.Serializable]
public struct SerializableRotation2D
{
    public float zDegrees;

    public SerializableRotation2D(float zDegrees) { this.zDegrees = zDegrees; }
    public SerializableRotation2D(Quaternion q) { zDegrees = q.eulerAngles.z; }
    public SerializableRotation2D(Transform t) { zDegrees = t.eulerAngles.z; }

    public Quaternion ToQuaternion() => Quaternion.Euler(0f, 0f, zDegrees);
    public float ToZDegrees() => zDegrees;

    public static SerializableRotation2D FromQuaternion(Quaternion q) => new SerializableRotation2D(q);
    public static SerializableRotation2D FromTransform(Transform t) => new SerializableRotation2D(t);
}
