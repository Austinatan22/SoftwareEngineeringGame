using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance;

	// public Image characterIcon;
	// public TextMeshProUGUI characterName;
	public TextMeshProUGUI dialogueArea;

	private Queue<DialogueLine> lines;
	public GameObject Box;

	public bool isDialogueActive = false;

	public float typingSpeed = 0.001f;

	// Reference to the item being considered for purchase
	private CollectionController currentShopItem;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;

		lines = new Queue<DialogueLine>();
	}

	private void Update()
	{
		if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
		{
			DisplayNextDialogueLine();
		}
	}

	public void StartDialogue(Dialogue dialogue, CollectionController shopItem = null)
	{
		isDialogueActive = true;
		currentShopItem = shopItem;

		lines.Clear();

		foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
		{
			lines.Enqueue(dialogueLine);
		}

		DisplayNextDialogueLine();
	}

	public void DisplayNextDialogueLine()
	{
		if (lines.Count == 0)
		{
			EndDialogue();
			return;
		}

		DialogueLine currentLine = lines.Dequeue();

		StopAllCoroutines();

		StartCoroutine(TypeSentence(currentLine));
	}

	IEnumerator TypeSentence(DialogueLine dialogueLine)
	{
		dialogueArea.text = "";
		foreach (char letter in dialogueLine.line.ToCharArray())
		{
			dialogueArea.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
	}

	void EndDialogue()
	{
		isDialogueActive = false;
		Box.SetActive(false);
	}

	public void BuyItem()
	{
		if (currentShopItem != null)
		{
			currentShopItem.buyShopItem();
			EndDialogue();
		}
	}

	public void Cancel()
	{
		EndDialogue();
	}
}
