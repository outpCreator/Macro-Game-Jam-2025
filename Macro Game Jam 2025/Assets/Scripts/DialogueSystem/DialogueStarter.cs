using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public string dialogueName;
    [SerializeField] bool isOneTimeEvent;

    bool isInDialogue = false;
    bool canBeStarted = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!canBeStarted || isInDialogue) return;

        if (collision.CompareTag("Player") && InputManager.Instance.interact)
        {
            DialogueManager.Instance.StartDialogue(dialogueName, this);

            if (isOneTimeEvent)
            {
                canBeStarted = false;
            }
        }
    }

    public void ResetDialogue()
    {
        isInDialogue = false;
    }
}
