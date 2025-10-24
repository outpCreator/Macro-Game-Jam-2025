using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading Screen Elements")]
    [SerializeField] Slider loadingBarSlider;

    float currentPercent = 0f;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetLoadingBarPercent(float percent)
    {
        currentPercent = Mathf.Clamp01(percent);

        loadingBarSlider.value = currentPercent;
        Debug.Log($"Loading bar set to {percent * 100}%");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
