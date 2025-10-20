using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScriptingConventionsExample : MonoBehaviour
{
    [Header("Example Header")]
    // Im Inspektor sichtbare Variablen
    [field: SerializeField] public GameObject exampleGameobject { get; private set; }
    [SerializeField] string exampleString;
    public UnityEvent exampleEvent;

    // Nicht im Inspektor sichtbare Variablen
    [HideInInspector] public int exampleInt;
    bool exampleBoolisRunning;
    bool exampleBoolisActive;
    float exampleFloat;

    // Caches / State
    bool hasInvokedEvent;
    Coroutine runRoutine;
    bool lastSetActiveState;

    private void Awake()
    {
        if (exampleGameobject == null) exampleGameobject = gameObject;

        lastSetActiveState = exampleGameobject != null && exampleGameobject.activeSelf;
    }

    private void Start()
    {
        exampleInt = 0;
        runRoutine = StartCoroutine(RunRoutine());
    }

    private IEnumerator RunRoutine()
    {
        while (true)
        {
            if (!exampleBoolisActive)
            {
                yield return null;
                continue;
            }

            if (exampleBoolisRunning)
            {
                exampleFloat = 0f;
                exampleBoolisRunning = false;

                SetActiveIfChanged(exampleBoolisRunning);
            }
            else
            {
                while (exampleFloat < 2f && exampleBoolisActive && !exampleBoolisRunning)
                {
                    exampleFloat += Time.deltaTime;
                    yield return null;
                }

                if (!exampleBoolisActive) continue;

                if (exampleFloat >= 2f)
                {
                    ExampleMethod();
                    exampleBoolisRunning = true;

                    SetActiveIfChanged(exampleBoolisRunning);
                }
            }

            yield return null;
        }
    }

    void SetActiveIfChanged(bool active)
    {
        if (exampleGameobject == null) return;

        if (exampleGameobject.activeSelf != active)
        {
            exampleGameobject.SetActive(active);
            lastSetActiveState = active;
        }
    }

    void ExampleMethod()
    {
        exampleInt++;

        if (!hasInvokedEvent && exampleInt >= 6)
        {
            exampleEvent?.Invoke();
            hasInvokedEvent = true;
        }
    }

    public void ToggleActive(bool active)
    {
        exampleBoolisActive = active;
    }
}
