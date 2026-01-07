using System;
using UnityEngine;

/// <summary>
/// Class that specifically is used to save
/// the vector3 position of a gameobject.
/// </summary>
[Serializable]
public class SerializableVector3 {
    public float x, y, z;

    public SerializableVector3(Vector3 v) {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }
}
