using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	[SerializeField] private GameObject DialogueCanvas;
	[SerializeField] private GameObject rTypeLoad1;
	[SerializeField] private GameObject rTypeLoad2;
	[SerializeField] private Button contBtnFinal;
    public Text nameText;
    public Text dialogueText;
	public Button contBtn1;
	public Button contBtn2;
	public Text contBtn1Txt;
	public Text contBtn2Txt;
	public Text contBtnFinalTxt;

    public Animator anim;
	private lyricsEntryHandler LEHandler;
	private int LELoadKey;
	private bool multipleSentences;

	private int currentResponseType = -1;
	
	void Awake(){
		LEHandler = DialogueCanvas.GetComponent<lyricsEntryHandler> ();

	}

	public void outputDialogue(string NPCName, int rType, string[] sentences, string[] btnTitles, int chantKey)
    {
		Debug.Log ("OutputDialogue Reached");
        anim.SetBool("isOpen", true);
		contBtnFinal.gameObject.SetActive (false);
		rTypeLoad1.SetActive(false);
		rTypeLoad2.SetActive (false);
		//nameText.text = dialogue.name;
		nameText.text = NPCName;

		currentResponseType = -1;
		if (rType == 1) {
			if (btnTitles.Length == 0) {
				//Empty title arrays denote response displays
				currentResponseType = 1;
			} else if (btnTitles.Length == 1) {
				//Title arrays of length 1 denote closing sentence displays
				currentResponseType = 2;
			} else {
				//Regular sentence display load:
				currentResponseType = 0;
			}
		} else if (rType == 2) {
			//Lyrics Entry response display:
			currentResponseType = 3;

			LELoadKey = chantKey;
		}

		if (currentResponseType == 0) {
			contBtn1Txt.text = btnTitles [0];
			contBtn2Txt.text = btnTitles [1];
		} else if (currentResponseType == 2) {
			contBtnFinalTxt.text = btnTitles [0];
		} else if (currentResponseType == 3) {
			LELoadKey = chantKey;
		}

		if (sentences.Length == 2) {
			rTypeLoad1.SetActive (false);
			
			StartCoroutine (TypeSentences (sentences, rType));
		} else if (sentences.Length == 1) {
			StartCoroutine (TypeSentence (sentences, rType));
		}
    }

	public void EndDialogue()
	{
		Debug.Log("End of conversation");
		anim.SetBool("isOpen", false);
	}
	/*
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
	*/
	IEnumerator TypeSentence(string[] sentence, int rType)
    {
		string sentence_I = sentence [0];
        dialogueText.text = "";
        foreach (char letter in sentence_I.ToCharArray())
        {
            dialogueText.text += letter;
			if (rType != -1) {
				yield return null;
			}
        }
		if (currentResponseType == 0) {
			rTypeLoad1.SetActive (true);
		} else if (currentResponseType == 2) {
			contBtnFinal.gameObject.SetActive (true);
		} else if (currentResponseType == 3) {
			yield return new WaitForSeconds (3.0f);
			rTypeLoad2.SetActive (true);

			LEHandler.initiateLE (LELoadKey);
		}
    }
	IEnumerator TypeSentences(string[] sentences, int rType)
	{
		string sentence_I = sentences [0];
		string sentence_II = sentences [1];
		dialogueText.text = "";
		foreach (char letter in sentence_I.ToCharArray())
		{
			dialogueText.text += letter;
			if (rType != -1) {
				yield return null;
			}
		}
		yield return new WaitForSeconds (6.0f);
		dialogueText.text = "";

		foreach (char letter in sentence_II.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}

		if (currentResponseType == 0) {
			rTypeLoad1.SetActive (true);
		} else if (currentResponseType == 2) {
			contBtnFinal.gameObject.SetActive (true);
		} else if (currentResponseType == 3) {
			yield return new WaitForSeconds (3.0f);
			rTypeLoad2.SetActive (true);

			LEHandler.initiateLE (LELoadKey);
		}
	}
}
