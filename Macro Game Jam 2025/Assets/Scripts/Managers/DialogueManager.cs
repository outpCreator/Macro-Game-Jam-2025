using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public DialogueAsset[] dialogueAssets;
    [SerializeField] Transform dialogueAnchor;

    DialogueStarter currentDialogueStarter;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(string dialogueName, DialogueStarter dialogueStarter)
    {
        currentDialogueStarter = dialogueStarter;

        DialogueAsset dialogueAsset = GetDialogue(dialogueName);
        if (dialogueAsset != null)
        {
            GameObject dialogueInstance = Instantiate(dialogueAsset.dialoguePrefab, dialogueAnchor);
            Dialogue dialogue = dialogueAsset.dialogue;
            dialogue.StartDialogue();
        }
        else
        {
            Debug.LogWarning("Dialogue not found: " + dialogueName);
        }
    }

    public void EndDialogue()
    {
        if (currentDialogueStarter != null)
        {
            currentDialogueStarter.ResetDialogue();
            currentDialogueStarter = null;
        }
    }

    DialogueAsset GetDialogue(string dialogueName)
    {
        foreach (DialogueAsset dialogueAsset in dialogueAssets)
        {
            if (dialogueAsset.dialogueName == dialogueName)
            {
                return dialogueAsset;
            }
        }
        return null;
    }
}
