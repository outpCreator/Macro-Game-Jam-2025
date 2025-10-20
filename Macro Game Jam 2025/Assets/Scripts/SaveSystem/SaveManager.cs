using System.IO;
using UnityEngine;

public static class SaveManager
{
    private const int MaxSlots = 3;

    public static string GetSavePath(int slot)
    {
        slot = Mathf.Clamp(slot, 1, MaxSlots);
        return Path.Combine(Application.persistentDataPath, $"saveSlot{slot}.json");
    }

    public static void Save(SaveData data, int slot)
    {
        string path = GetSavePath(slot);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log($"[SaveManager] Game saved to slot {slot} ({path})");
    }

    public static SaveData Load(int slot)
    {
        string path = GetSavePath(slot);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[SaveManager] No save file found in slot {slot}");
            return null;
        }

        string json = File.ReadAllText(path);
        var data = JsonUtility.FromJson<SaveData>(json);
        return data;
    }

    public static void DeleteSave(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[SaveManager] Save file in slot {slot} deleted.");
        }
    }

    public static bool SaveExists(int slot)
    {
        return File.Exists(GetSavePath(slot));
    }
}
