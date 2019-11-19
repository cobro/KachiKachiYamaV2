using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogueIntro;
    public Dialogue dialogueMeeting;
    public Dialogue burntRacoon1;
    public Dialogue burntRacoon2;
    public Dialogue burntRacoon3;
    public Dialogue burntRacoon4;
    public Dialogue burntRacoon5;
    public Dialogue dialogueLost;

    public Dialogue dialogueWon;

	public void TriggerDialogue (Dialogue tempDialogue)
	{
		FindObjectOfType<DialogueManager>().StartDialogue(tempDialogue);
	}

}