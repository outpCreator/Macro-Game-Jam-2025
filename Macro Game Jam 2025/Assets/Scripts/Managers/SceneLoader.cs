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

    public IEnumerator SwitchSceneRoutine(string sceneName, string entryID = "")
    {
        Debug.Log($"[SceneLoader] Switching to scene '{sceneName}' (entryID: {entryID})");

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[SceneLoader] Scene '{sceneName}' cannot be loaded. Check your Build Settings.");
            yield break;
        }

        // Optional Fade-Out
        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeToBlack();

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
            yield return null;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid())
            SceneManager.SetActiveScene(loadedScene);

        onSceneChanged.Invoke(entryID);
        yield return new WaitForEndOfFrame();

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeFromBlack();

        onSceneLoadedFully.Invoke();

        Debug.Log($"[SceneLoader] Scene '{sceneName}' loaded and active.");
    }

    public async Task SwitchSceneAsync(string sceneName, string entryID = "")
    {
        Debug.Log($"[SceneLoader] Async switch to scene '{sceneName}'");

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[SceneLoader] Scene '{sceneName}' cannot be loaded.");
            return;
        }

        if (ScreenFader.Instance != null)
            await ScreenFader.Instance.FadeToBlackAsync();

        onBeforeSceneUnload.Invoke();

        scenesToUnload.Clear();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s.name != gameScene)
                scenesToUnload.Add(s);
        }

        foreach (Scene s in scenesToUnload)
        {
            await SceneManager.UnloadSceneAsync(s);
        }

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            await Task.Yield();

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid())
            SceneManager.SetActiveScene(loadedScene);

        onSceneChanged.Invoke(entryID);

        if (ScreenFader.Instance != null)
            await ScreenFader.Instance.FadeFromBlackAsync();

        onSceneLoadedFully.Invoke();
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
