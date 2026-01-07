using System;
using UnityEngine;

/// <summary>
/// Class that specifically is used to save
/// the quaternion of a gameobject.
/// </summary>
[Serializable]
public class SerializableQuaternion {
    public float x, y, z, w;

    public SerializableQuaternion(Quaternion q) {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }

    public Quaternion ToQuaternion() {
        return new Quaternion(x, y, z, w);
    }
}
