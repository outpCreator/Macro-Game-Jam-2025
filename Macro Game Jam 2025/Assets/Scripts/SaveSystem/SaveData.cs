using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string lastScene;

    public PlayerData player;
    public WorldData world;
}

[Serializable]
public class PlayerData
{
    [Header("Player")]
    public SerializableVector2 playerPosition;
    public SerializableRotation2D playerRotation;
}

[Serializable]
public class WorldData
{
    // Add world-related data here
}
