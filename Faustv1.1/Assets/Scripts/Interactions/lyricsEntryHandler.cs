using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lyricsEntryHandler : MonoBehaviour {
	[SerializeField] private chantRefs Lyrics;
	[SerializeField] private Slider Timer;
	[SerializeField] private Text lyricDisplay;
	[SerializeField] private Text dialogueText;
	[SerializeField] private InputField LEInput;
	public string[] LEKeys;
	public float timeLimit;

	private Dictionary<int, string[]> LELoads;
	private string[] LELoad;
	private string currentLyric;
	private bool beginTimer;
	private int LEIndex;
	private float timeRemaining;
	void Awake(){
		LELoads = new Dictionary<int, string[]> ();
		LELoads.Add (0, Lyrics.GetComponent<chantRefs>().chant0);
		LELoads.Add (1, Lyrics.GetComponent<chantRefs>().chant1);
		LELoads.Add (2, Lyrics.GetComponent<chantRefs>().chant2);
	}


	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	public void initiateLE(int LELoadKey){
		Debug.Log (LELoadKey);
		Messenger<bool>.Broadcast ("canMove_Update", false);

		LELoad = LELoads [LELoadKey];
		LEIndex = 0;
		currentLyric = LELoad [LEIndex];
		lyricDisplay.text = "Lyric : " + currentLyric;
		dialogueText.text = "";
		LEInput.text = "";
		LEInput.ActivateInputField ();

		timeRemaining = timeLimit;
		beginTimer = true;
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	//Method triggered by L.E. InputField; checks entered word:
	public void checkLE(Text textEntry){
		string lyricEntered = textEntry.text;
		if (lyricEntered == currentLyric /*&& timeRemaining > 0*/) {
			LEIndex++;
			if (LEIndex < LELoad.Length) {
				currentLyric = LELoad [LEIndex];
				lyricDisplay.text = currentLyric;
				dialogueText.text = dialogueText.text + " " + lyricEntered;

				LEInput.text = "";
				timeRemaining = timeLimit;
			}
		} else {
			Debug.Log ("Incorrect");
			textEntry.text = "";
		}
		LEInput.ActivateInputField ();

		//Run upon L.E. task completion
		if (LEIndex == LELoad.Length) {
			Debug.Log ("Task completed");
			beginTimer = false;
			Messenger<bool>.Broadcast ("canMove_Update", true);
			Messenger<bool>.Broadcast ("LERtrn", true);
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish


	void Update () {
		//Timer function
		if (beginTimer) {
			timeRemaining -= Time.deltaTime;
			Timer.value = timeRemaining / timeLimit;

			if (timeRemaining <= 0) {
				Debug.Log ("Time elapsed");
				Messenger<bool>.Broadcast ("canMove_Update", true);
				Messenger<bool>.Broadcast ("LERtrn", false);
				beginTimer = false;

			}
		}
	}
}
