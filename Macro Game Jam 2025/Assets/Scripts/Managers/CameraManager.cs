using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    // Camera follow variables
    bool isFollowingTarget = false;
    float followDuration = 0.5f;
    Transform followTarget;

    float edgeThreshold = 5.0f;

    Camera mainCamera;

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
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isFollowingTarget && followTarget != null)
        {
            float currentDuration = followDuration;

            Vector3 targetPosition = followTarget.position;
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, targetPosition, currentDuration * Time.deltaTime);
            mainCamera.transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, mainCamera.transform.position.z);

            // if player moves closer to screen edge, follow faster
            
        }
    }

    public void ToggleCursorLockState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        // Implement camera shake logic here
        
        Debug.Log($"Shaking camera with intensity {intensity} for {duration} seconds.");
    }

    public void CameraFollow(Transform target, float duration, bool following)
    {
        followDuration = duration;
        followTarget = target;
        isFollowingTarget = following;

        Debug.Log($"Camera follow set to {following} for target {target.name} with duration {duration}.");
    }
}
