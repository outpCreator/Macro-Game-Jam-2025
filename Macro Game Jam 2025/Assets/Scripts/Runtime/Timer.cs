using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Visuals")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject timerUI;

    [Header("Timer Settings")]
    [SerializeField] float duration;
    public UnityEvent onTimerEnd;

    bool timerRunning = false;
    float timeRemaining;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                OnTimerEnd();
            }
        }

        DisplayTimer();
    }

    public void ToggleTimer(bool state)
    {
        timerRunning = state;
    }

    void DisplayTimer()
    {
        // Format time as MM:SS
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        timeRemaining = duration;
    }

    void OnTimerEnd()
    {
        Debug.Log("Timer ended!");
        onTimerEnd?.Invoke();
    }
}
