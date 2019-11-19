using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginningScript : MonoBehaviour
{

    public Image Beginning1;
    public Image Beginning2;
    public GameObject Character;
    public GameObject Racoon;

    public DialogueTrigger dialoguetriggering;
    public DialogueManager nextSentence;
    
    void Start()
    {

        Character.SetActive(false);
        Racoon.SetActive(false);
        StartCoroutine("waitForIntroduction");
        Beginning1.gameObject.SetActive(true);
        Beginning2.gameObject.SetActive(true);
    }

    IEnumerator waitForIntroduction(){
        yield return new WaitForSeconds(1.5f);
        Beginning1.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Beginning2.gameObject.SetActive(false);
        Character.SetActive(true);
        Racoon.SetActive(true);
        dialoguetriggering.TriggerDialogue(dialoguetriggering.dialogueMeeting);
    }

    void Update(){
        if(Input.GetKeyUp(KeyCode.F) || Input.GetButtonUp("buttonA")){
            nextSentence.DisplayNextSentence();
        }
    }
}
