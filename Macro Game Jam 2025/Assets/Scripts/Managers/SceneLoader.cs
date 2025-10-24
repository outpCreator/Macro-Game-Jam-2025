using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    const string gameScene = "Game";

    [HideInInspector] public UnityEvent<string> onSceneChanged = new UnityEvent<string>();
    [HideInInspector] public UnityEvent onBeforeSceneUnload = new UnityEvent();
    [HideInInspector] public UnityEvent onSceneLoadedFully = new UnityEvent();

    List<Scene> scenesToUnload = new List<Scene>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchScene(string sceneName, string entryID = "")
    {
        StartCoroutine(SwitchSceneRoutine(sceneName, entryID));
    }

    IEnumerator SwitchSceneRoutine(string sceneName, string entryID = "")
    {
        Debug.Log($"[SceneLoader] Switching to scene '{sceneName}' (entryID: {entryID})");

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[SceneLoader] Scene '{sceneName}' cannot be loaded. Check your Build Settings.");
            yield break;
        }

        // Optional Fade-Out
        if (ScreenFader.Instance != null) yield return ScreenFader.Instance.FadeToBlack();

        onBeforeSceneUnload.Invoke();

        // Collect and unload non-persistent scenes
        scenesToUnload.Clear();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s.name != gameScene)
                scenesToUnload.Add(s);
        }

        foreach (Scene s in scenesToUnload)
        {
            yield return SceneManager.UnloadSceneAsync(s);
        }

        // Load target scene
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
        {
            // Add optional loading progress handling here
            yield return null;
        }


        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid()) SceneManager.SetActiveScene(loadedScene);

        onSceneChanged.Invoke(entryID);
        yield return new WaitForEndOfFrame();

        if (ScreenFader.Instance != null) yield return ScreenFader.Instance.FadeFromBlack();

        onSceneLoadedFully.Invoke();

        Debug.Log($"[SceneLoader] Scene '{sceneName}' loaded and active.");
    }

    [RuntimeInitializeOnLoadMethod]
    static void LoadGameScene()
    {
        if (!SceneManager.GetSceneByName(gameScene).IsValid())
        {
            Debug.Log("[SceneLoader] Loading persistent Game scene...");
            SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
        }
    }
}
