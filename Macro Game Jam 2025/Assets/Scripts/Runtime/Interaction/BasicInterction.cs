using UnityEngine;
using UnityEngine.Events;

public class BasicInterction : MonoBehaviour
{
    public UnityEvent OnInteracted;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered interaction zone of " + gameObject.name);

            if (InputManager.Instance.interact)
            {
                //OnInteracted?.Invoke();

                Debug.Log("Interacted with " + gameObject.name);

                gameObject.SetActive(false);
            }
        }
    }

    public void RespawnObject()
    {
        gameObject.SetActive(true);
    }
}
