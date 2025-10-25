using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPoint;

    [HideInInspector] public GameObject playerInstance;
    [HideInInspector] public PlayerControls playerControls;
    [HideInInspector] public Transform playerTransform;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitPlayer()
    {
        if (playerInstance != null) return;

        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        playerControls = playerInstance.GetComponent<PlayerControls>();
        playerTransform = playerInstance.transform;

        SceneLoader.Instance.onSceneChanged.AddListener(OnSceneChange);

        CameraManager.Instance.CameraFollow(playerTransform, 5f, true);
    }

    public void OnSceneChange(string entryID)
    {

    }
}
