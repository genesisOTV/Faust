using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
	public Button contBtn1;
	public Button contBtn2;
	public Button contBtnFinal;
	public Text contBtn1Txt;
	public Text contBtn2Txt;
	public Text contBtnFinalTxt;

    public Animator anim;
	private bool multipleSentences;

    //public Queue<string> sentences;
	/*
	void Start () {
        sentences = new Queue<string>();
	}
	*/

	public void outputDialogue(/*Dialogue dialogue*/string NPCName, string[] sentences, string[] btnTitles)
    {
		Debug.Log ("OutputDialogue Reached");
        anim.SetBool("isOpen", true);
		//nameText.text = dialogue.name;
		nameText.text = NPCName;
		if (btnTitles.Length == 0) {
			//Empty title arrays denote response displays
			contBtn1.gameObject.SetActive (false);
			contBtn2.gameObject.SetActive (false);
			contBtnFinal.gameObject.SetActive (false);
		} else if (btnTitles.Length == 1) {
			//Title arrays of length 1 denote closing sentence displays
			contBtn1.gameObject.SetActive (false);
			contBtn2.gameObject.SetActive (false);
			contBtnFinal.gameObject.SetActive (true);

			contBtnFinalTxt.text = btnTitles [0];
		} 
		else {
			contBtn1.gameObject.SetActive (true);
			contBtn2.gameObject.SetActive (true);
			contBtnFinal.gameObject.SetActive (false);

			contBtn1Txt.text = btnTitles [0];
			contBtn2Txt.text = btnTitles [1];
		}

		/*sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }*/
		if (sentences.Length == 2) {
			contBtn1.gameObject.SetActive (false);
			contBtn2.gameObject.SetActive (false);

			StartCoroutine (TypeSentences (sentences));
		} else if (sentences.Length == 1) {
			StartCoroutine (TypeSentence (sentences));
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
    IEnumerator TypeSentence(string[] sentence)
    {
		string sentence_I = sentence [0];
        dialogueText.text = "";
        foreach (char letter in sentence_I.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
	IEnumerator TypeSentences(string[] sentences)
	{
		string sentence_I = sentences [0];
		string sentence_II = sentences [1];
		dialogueText.text = "";
		foreach (char letter in sentence_I.ToCharArray())
		{
			dialogueText.text += letter;
            yield return null;
        }
		yield return new WaitForSeconds (6.0f);
		contBtn1.gameObject.SetActive (true);
		contBtn2.gameObject.SetActive (true);
		dialogueText.text = "";

		foreach (char letter in sentence_II.ToCharArray())
		{
			dialogueText.text += letter;
            yield return null;
        }
	}
}
