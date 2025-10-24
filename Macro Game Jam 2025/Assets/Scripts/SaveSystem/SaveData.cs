using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string lastScene;

    public PlayerData player;
    public InventoryData inventory;
    public WorldData world;
}

[Serializable]
public class PlayerData
{
    public SerializableVector2 playerPosition;
    public SerializableRotation2D playerRotation;
}

[Serializable]
public class InventoryData
{
    public int currentInventorySlots;
    public int maxInventorySize;

    public List<ItemTearOne> tearOneItems;
}

[Serializable]
public class WorldData
{
    // Add world-related data here
}
