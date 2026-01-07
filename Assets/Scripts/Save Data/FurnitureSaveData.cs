using System;

/// <summary>
/// Class that specifically is used to save
/// the player's furniture in the store.
/// </summary>
[Serializable]
public class FurnitureSaveData {
    public string instanceId;
    public string furnitureId;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

}
