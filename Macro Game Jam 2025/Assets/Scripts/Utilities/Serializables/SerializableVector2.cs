using UnityEngine;

[System.Serializable]
public struct SerializableVector2
{
    public float x, y;

    public SerializableVector2(float x, float y) { this.x = x; this.y = y; }
    public SerializableVector2(Vector2 v) { x = v.x; y = v.y; }
    public SerializableVector2(Vector3 v3) { x = v3.x; y = v3.y; }

    public Vector2 ToVector2() => new Vector2(x, y);
    public Vector3 ToVector3(float z) => new Vector3(x, y, z);

    public static SerializableVector2 FromVector2(Vector2 v) => new SerializableVector2(v);
    public static SerializableVector2 FromVector3(Vector3 v3) => new SerializableVector2(v3);
}
