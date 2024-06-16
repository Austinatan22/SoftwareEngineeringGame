using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public static DialogueTrigger instance;
    public Dialogue dialogue;
    public GameObject Trigger;
    private CollectionController collectionController;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        collectionController = GetComponent<CollectionController>();
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, collectionController);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogueManager.Instance.Box.SetActive(true);
            TriggerDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogueManager.Instance.Box.SetActive(false);
        }
    }
}
