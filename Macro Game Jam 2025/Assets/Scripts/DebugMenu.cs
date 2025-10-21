using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private int selectedSlot = 1;
    private bool showMenu = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            showMenu = !showMenu;
            PlayerControls.debugMenuActive = showMenu;

            CameraManager.Instance.ToggleCursorLockState(showMenu);
        }
    }

    private void OnGUI()
    {
        if (!showMenu) return;

        GUILayout.BeginArea(new Rect(10, 10, 250, 300), GUI.skin.box);
        GUILayout.Label("<b><size=15>Save System Debug</size></b>", GUI.skin.label);

        GUILayout.Space(10);
        GUILayout.Label("Selected Slot:");
        selectedSlot = GUILayout.SelectionGrid(selectedSlot - 1, new[] { "Slot 1", "Slot 2", "Slot 3" }, 3) + 1;

        GUILayout.Space(10);

        if (GUILayout.Button("?? Save Game"))
        {
            SaveGame(selectedSlot);
        }

        if (GUILayout.Button("?? Load Game"))
        {
            LoadGame(selectedSlot);
        }

        if (GUILayout.Button("??? Delete Save"))
        {
            SaveManager.DeleteSave(selectedSlot);
        }

        GUILayout.Space(10);

        for (int i = 1; i <= 3; i++)
        {
            string exists = SaveManager.SaveExists(i) ? "?" : "?";
            GUILayout.Label($"Slot {i}: {exists}");
        }

        GUILayout.EndArea();
    }

    private void SaveGame(int slot)
    {
        SaveData data = new SaveData();
        data.lastScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Player
        data.player = new PlayerData();
        Transform player = PlayerManager.Instance.playerTransform;

        data.player.playerPosition = new SerializableVector2(player.position);
        data.player.playerRotation = new SerializableRotation2D(player.rotation);

        // World
        data.world = new WorldData();

        SaveManager.Save(data, slot);
        Debug.Log($"[DebugMenu] Spielstand gespeichert in Slot {slot}");
    }

    private void LoadGame(int slot)
    {
        SaveData data = SaveManager.Load(slot);
        if (data == null)
        {
            Debug.LogWarning($"[DebugMenu] No save found in slot {slot}");
            return;
        }

        // Player
        var player = PlayerManager.Instance.playerTransform;

        // 2D-Physik kurz parken
        var rb2d = player.GetComponent<Rigidbody2D>();
        bool hadRb2d = rb2d != null;
        if (hadRb2d) rb2d.simulated = false;

        // Position: x,y aus Save, z beibehalten
        float currentZ = player.position.z;
        player.position = data.player.playerPosition.ToVector3(currentZ);

        // Rotation: nur Z
        player.rotation = data.player.playerRotation.ToQuaternion();

        if (hadRb2d)
        {
            rb2d.linearVelocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            rb2d.simulated = true;
        }

        Debug.Log($"[DebugMenu] Spieler geladen – Position: {player.position}, Rotation Z: {player.eulerAngles.z}°");

        // Welt

    }
}
