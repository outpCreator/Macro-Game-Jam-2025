using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] GameObject playerPrefab;

    [HideInInspector] public GameObject playerInstance;
    [HideInInspector] public PlayerControls playerControls;
    [HideInInspector] public Transform playerTransform;

    [HideInInspector] public Transform spawnPoint;

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

    private void Start()
    {
        if (playerInstance != null) return;

        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        playerControls = playerInstance.GetComponent<PlayerControls>();
        playerTransform = playerInstance.transform;
    }
}
