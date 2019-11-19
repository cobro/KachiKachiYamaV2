
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	public Text dialogueTextRabbit;
    public Text dialogueTextRacoon;

    public bool rabbit = true;


	private Queue<string> sentences;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
	}

	public void StartDialogue (Dialogue dialogue)
	{
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopCoroutine("TypeSentence");
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
        if(rabbit){
            dialogueTextRabbit.transform.parent.gameObject.SetActive(true);
            dialogueTextRacoon.transform.parent.gameObject.SetActive(false);
            dialogueTextRabbit.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueTextRabbit.text += letter;
                yield return null;
            }
            rabbit = false;
        }
        else{
            dialogueTextRabbit.transform.parent.gameObject.SetActive(false);
            dialogueTextRacoon.transform.parent.gameObject.SetActive(true);
            dialogueTextRacoon.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueTextRacoon.text += letter;
                yield return null;
            }
            rabbit = true;
        }
	}

	void EndDialogue()
	{
		dialogueTextRabbit.transform.parent.gameObject.SetActive(false);
        dialogueTextRacoon.transform.parent.gameObject.SetActive(false);
	}

}