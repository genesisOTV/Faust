using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class limbo_lyricsEntryHandler : MonoBehaviour {
	[SerializeField] private InputField entryField;
	[SerializeField] private Text Prompter;
	[SerializeField] private Slider timeDisplay;

	private string[] lyrics = new string[]{"Calling", "his", "tailor", "he said,", "fetch", "the needles,", "and scissors,", "measure", "the", "Baron", "for", "shirts!"};
	private int lyricsCount = 0;
	private string lyricsEntered = "";

	public float timeFrame = 0;
	public float timeRemaining = 0;
	public bool beginTime;
	private float defaultTime = 6.0f;

	public void textEntry_q2(){
		string submittedWord = entryField.text;
		if (timeRemaining > 0) {
			if (submittedWord == lyrics [lyricsCount]) {
				Debug.Log ("Word Correct");
				timerControl (defaultTime, false);
				lyricsEntered = lyricsEntered + submittedWord + " ";
				Messenger<string>.Broadcast ("LyricEntered", lyricsEntered);
				lyricsCount++;
				if (lyricsCount < lyrics.Length) {
					Prompter.text = lyrics [lyricsCount];
				}
				entryField.text = "";

				timerControl (defaultTime, true);
			} else {
				Debug.Log ("Word Incorrect");
				entryField.text = "";
			}
		}
		entryField.ActivateInputField ();
		StartCoroutine (checkCompletion ());

		/*
		GameObject lEInputField = GameObject.FindGameObjectWithTag ("TestInputField");
		InputField Field = lEInputField.GetComponentInChildren<InputField> ();
		if (Field != null) {
			Debug.Log ("Found");
			Field.ActivateInputField ();
		}
		*/
	}
	private IEnumerator checkCompletion(){
		if (lyricsCount == lyrics.Length) {
			Debug.Log ("Completed");
			Messenger<bool>.Broadcast ("Chant1Finished", true);
		}
		yield return null;
	}

	public void timerControl(float timeLimit, bool Start){
		//Set timer specs
		timeFrame = timeLimit;
		timeRemaining = timeFrame;
		beginTime = Start;
		Debug.Log ("Timer (re)set");
	}
	void Update(){
		//Debug.Log (beginTime);
		if (beginTime) {
			//Debug.Log ("Update Ran");
			timeRemaining -= Time.deltaTime;
			//Debug.Log (timeRemaining);
			timeDisplay.value = (timeRemaining / timeFrame);
			if (timeRemaining < 0) {
				Debug.Log ("Time Elapsed");
				beginTime = false;
			}
		}
	}
}
