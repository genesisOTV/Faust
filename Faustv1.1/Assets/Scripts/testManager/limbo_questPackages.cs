using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class limbo_questPackages : MonoBehaviour {
	//INACTIVE/OBSOLETE CODE FORMERLY UTILIZED BY QUESTPACKAGES.CS

	//DIALOGUE TESTER STAGE **TEMPORARY**
	/*private IEnumerator testerStage(){
		//RELOCATE AS SCRIPT-WIDE VARIABLE
		Interactable dialogueProxy;
		Dictionary<string, string> dialogueRef = new Dictionary<string, string> ();
		string npcName = "Tester";
		string[] contOptions = new string[2];
		//Holder
		int qBranchI = -1;

		dialogueProxy = tDialogueCanvas.GetComponent<Interactable> ();
		dialogueRef = dialogueProxy.sentenceArchive_Tester;
		string OrationI = dialogueRef ["tDialogue"];
		contOptions = new string[]{ "Left (1)", "Right (2)" };
		dlgInterface.outputDialogue (npcName, OrationI, contOptions);

		inputReceived = false;
		inputReturn = -1;
		yield return new WaitUntil (() => inputReceived);
		qBranchI = inputReturn;

		string OrationII = "Holder";
		if (qBranchI == 0) {
			OrationII = dialogueRef ["tDialogue0"];
		} else if (qBranchI == 1) {
			OrationII = dialogueRef ["tDialogue1"];
		} else {
			Debug.Log ("Failure");
		}
		dlgInterface.outputDialogue (npcName, OrationII, contOptions);
		//Prompter.SetActive (true);
		/*
		GameObject DBOne = new GameObject ();
		StartCoroutine (createDB (1, DBGet => {
			if (DBGet != null) {
				DBOne = DBGet;
			}
		}));
		
		string OrationI = dialogueRef ["tDialogue"];
		Text DBOneTxt = DBOne.GetComponent<Text> ();
		DBOneTxt.text = "This is a test: " + OrationI;


		yield return null;
	}*/


	//MISC. BUTTON ENTRY HANDLERS:
	/*public void limbo_compileDecision(string Cont1, string Cont2, int Choice){
		cont1Getter = Cont1;
		if (Cont2 != "Holder") {
			cont2Getter = Cont2;
		} else {
			cont2Getter = "Empty";
		}
		inputReceived = true;
		inputReturn = Choice;
	}
	public void compileResponseII_i(bool completionType){
		songSuccess = completionType;
		inputReceived = true;
	}
	public void updateLEResponse(string lEInput){
		Text lEDBTxt = lEAccessDB.GetComponent<Text> ();
		lEDBTxt.text = "Faust: " + lEInput;
	}*/


	//INACTIVE TEXTCRAWL METHOD:
	/*private IEnumerator textCrawl(Dictionary<string, float> lyrics, Text DBTxt){
		string[] words = new string[19];
		float[] pauses = new float[19];
		lyrics.Keys.CopyTo (words, 0);
		lyrics.Values.CopyTo (pauses, 0);

		string Song_Reassembled = "";
		for (int i = 0; i < lyrics.Count; i++) {
			Song_Reassembled = Song_Reassembled + words [i];
			DBTxt.text = "Frosch: " + Song_Reassembled;

			yield return new WaitForSeconds (pauses [i]);
		}
		TCgate = true;
		yield return null;
	}*/


	//OBSOLETE QUEST COMPLETION COROUTINE:
	/*private IEnumerator checkCompletion(bool completionRef, Action<bool> rtrnGate){
		rtrnGate (completionRef);
		if (completionRef) {
			yield return new WaitForSeconds (2f);
			//Set stage's qPhase to complete
			qStatus.qPhase [activeQNPC] = 2;
			//Removal of stage's events (Cleanup)
			GameObject[] activeDBs = GameObject.FindGameObjectsWithTag ("DBSlot");
			if (activeDBs.Length > 0) {
				foreach (GameObject DB in activeDBs) {
					Destroy (DB);
				}
			}
			Prompter.SetActive (false);
		}
		yield return null;
	}*/


	//ELEMENTS OF QUEST 2

	//SECTION 1:
	/*GameObject DBOne = new GameObject ();
		StartCoroutine (createDB (1, DBGet => {
			if(DBGet != null){
				DBOne = DBGet;
			}
		}));
		Text DBOneTxt = DBOne.GetComponent<Text> ();
		DBOneTxt.text = Orater + OrationI;

		canContinue = false;
		inputReceived = false;
		StartCoroutine (getResponseII (btnLoad_Frosch, 0, 0, 2, choiceGet => {
			if(choiceGet != null){
				choiceI = choiceGet;
				canContinue = true;
			}
		}));

		yield return new WaitUntil(() => canContinue);*/
	
	//SECTION 2:
	/*
	 * if (choiceI != 0) {
			int resumeTo = 0;
			//string OrationII = "Holder";
			//string OrationIII = "Holder";

			GameObject DBFour = new GameObject();
			StartCoroutine (createDB (4, DBGet => {
				if(DBGet != null){
					DBFour = DBGet;
				}
			}));
			Text DBFourTxt = DBFour.GetComponent<Text> ();
			if (choiceI == 1) {
				//OrationII = "Ah-hah! Uh drink y'say. You were put up to this by that... that dolt Siebel — that's my guess.";
				resumeTo = 1;
				qBranchI = 0;
			} else if (choiceI == 2) {
				//OrationII = "Eve'nin'—evenin' you say... Bloody'ell it's already dark out!?";
				resumeTo = 2;
				qBranchI = 1;
			}
			DBFourTxt.text = Orater + OrationII;
			yield return new WaitForSeconds (3.5f);

			GameObject DBFive = new GameObject ();
			StartCoroutine (createDB (5, DBGet => {
				if(DBGet != null){
					DBFive = DBGet;
				}
			}));
			Text DBFiveTxt = DBFive.GetComponent<Text> ();
			if (resumeTo == 1) {
				//OrationIII = "Y'here to, uh, join in the fes... festivities? Hey! Why don't we start uh sing-'long!";
			} else if (resumeTo == 2) {
				//OrationIII = "Oh nevermind, I plann'd on bein' here 'til 1:00 anyway... Uh, what can I do for you?";
			}
			DBFiveTxt.text = Orater + OrationIII;
		}

		canContinue = false;
		inputReceived = false;
		StartCoroutine (getResponseII (btnLoad_Frosch, 1, qBranchI, 6, choiceGet => {
			if(choiceGet != null){
				choiceII = choiceGet;
				canContinue = true;
			}
		}));

		yield return new WaitUntil (() => canContinue);
		yield return new WaitForSeconds (2);

		if (choiceII != 0) {
			int resumeTo = 0;
			int DBNumber = 7;
			if (qBranchI == 1 && choiceII == 2) {
				//Either 7 or 8 depending on path selected
				DBNumber++;
			}
			string OrationIV = "Holder";
			string OrationV = "Holder";
			GameObject DBSeven_I = new GameObject ();
			StartCoroutine (createDB (DBNumber, DBGet => {
				if(DBGet != null){
					DBSeven_I = DBGet;
				}
			}));
			Text DBSeven_ITxt = DBSeven_I.GetComponent<Text> ();

			if (qBranchI == 0) {
				if (choiceII == 1) {
					OrationIV = "That's the spirit! This one is from, uh, Spain — pitch'n when I throw you the lead!";
					resumeTo = 1;
				} else if (choiceII == 2) {
					OrationIV = "Oh c'mon, uh frien' doesn't let his part-n'r sing alone!";
					resumeTo = 1;
				}
				qBranchII = 0;
				//Skip 9th slot DB; clear and return to top
				DBBuffer =+ 2;
			} else if (qBranchI == 1) {
				if (choiceII == 1) {
					OrationIV = "Huh... what? Oh, yeah. It's the only decent tavern in the go'damn sector.";
					resumeTo = 2;
					qBranchII = 1;
				} else if (choiceII == 2) {
					OrationIV = "You've come t'the right man—I've been comin' to this bar longer than I can remember.";
					resumeTo = 3;
					qBranchII = 2;
				}
				DBBuffer = 0;
			}
			DBSeven_ITxt.text = Orater + OrationIV;
			yield return new WaitForSeconds (3.5f);

			GameObject DBEight_II = new GameObject ();
			tempDBIndex = DBNumber + DBBuffer + 1;
			DBgate = false;
			StartCoroutine (createDB (tempDBIndex, DBGet => {
				if(DBGet != null){
					DBEight_II = DBGet;
					DBgate = true;
				}
			}));
			yield return new WaitUntil (() => DBgate);
			Text DBEight_IITxt = DBEight_II.GetComponent<Text> ();

			if (resumeTo == 1) {
				Dictionary<string, float> Chant = textLog.chant1Lyrics1;
				//ADD GATE TO PREVENT PRE-MATURE CONTINUATION
				TCgate = false;
				StartCoroutine (textCrawl (Chant, DBEight_IITxt));
				yield return new WaitUntil (() => TCgate);
			} else if (resumeTo == 2) {
				OrationV = "Trust me, I sure've lived here long-'nough.";
			} else if (resumeTo == 3) {
				OrationV = "Well, what are yuh lookin' for?";
			}
			DBEight_IITxt.text = Orater + OrationV;
		}

		canContinue = false;
		inputReceived = false;
		if (qBranchII == 0) {
			StartCoroutine (getResponseII_i (btnLoad_Frosch, 3, tempDBIndex, getSuccess => {
				if(getSuccess != null){
					Debug.Log(getSuccess);
					songIAchieved = getSuccess;
					canContinue = true;
					Messenger<bool>.Broadcast("canMove_update", true);
				}
			}));
		} else if (qBranchII == 1 || qBranchII == 2) {
			StartCoroutine (getResponseII (btnLoad_Frosch, 3, qBranchII, tempDBIndex, choiceGet => {
				if(choiceGet != null){
					choiceIII = choiceGet;
					canContinue = true;
				}
			}));
		}

		yield return new WaitUntil (() => canContinue);
		Debug.Log ("Continuation reached");
		yield return new WaitForSeconds (3);

		if (qBranchII > 0) {
			//Path of qBranchI & qBranchII
			int resumeTo = 0;
			GameObject DBNine = new GameObject ();
			StartCoroutine (createDB (9, DBGet => {
				if (DBGet != null) {
					DBNine = DBGet;
				}
			}));
			Text DBNineTxt = DBNine.GetComponent<Text> ();
			string OrationVI = "Holder";
			string OrationVII = "Holder";
			if (qBranchII == 2 || qBranchII == 1 && choiceIII == 2) {
				OrationVI = "Oh, y're shit out-uh've luck, yuh jus' misst the bartender, Altmayer.";
				resumeTo = 1;
			} else if (qBranchII == 1 && choiceIII == 1) {
				OrationVI = "Psht, wuh's with the questionin', it's like you were performing uhn audit.";
				resumeTo = 2;
			}
			DBNineTxt.text = Orater + OrationVI;
			yield return new WaitForSeconds (3.5f);

			GameObject DBTen = new GameObject ();
			DBgate = false;
			StartCoroutine (createDB (10, DBGet => {
				if (DBGet != null) {
					DBTen = DBGet;
					DBgate = true;
				}
			}));
			yield return new WaitUntil (() => DBgate);
			Text DBTenTxt = DBTen.GetComponent<Text> ();
			if (resumeTo == 1) {
				OrationVII = "I believe he went down to the basement, you can probably find him there.";
			} else if (resumeTo == 2) {
				OrationVII = "";
			}
			DBTenTxt.text = Orater + OrationVII;
		} else if (qBranchII == 0) {
			//Path following Chant1:
			GameObject DBTwelve = new GameObject();
			StartCoroutine (createDB (12, DBGet => {
				if(DBGet != null){
					DBTwelve = DBGet;
				}
			}));
			Text DBTwelveTxt = DBTwelve.GetComponent<Text> ();
			Dictionary<string, float> Chant = textLog.chant1Lyrics2;
			TCgate = false;
			StartCoroutine (textCrawl (Chant, DBTwelveTxt));
			yield return new WaitUntil (() => TCgate);
	*/

	//*OBSOLETE* GETRESPONSE METHODS:

	//COROUTINE 1:
	/*
	private IEnumerator getResponseII(GameObject BLCollection, int BLIndex, int branchPath, int DBIndex, Action<int> choice){
		Canvas[] BLs = BLCollection.GetComponentsInChildren<Canvas> ();
		BLIndex = BLIndex + branchPath;
		Canvas BLCalled = BLs.GetValue (BLIndex) as Canvas;
		Canvas currentUI = Instantiate (BLCalled, BLPrompter.transform);

		yield return new WaitUntil (() => inputReceived);
		//Dismantling canvas dependencies
		DestroyImmediate (currentUI.gameObject.GetComponent<GraphicRaycaster>());
		DestroyImmediate (currentUI.gameObject.GetComponent<CanvasScaler> ());
		//Removing button loadout
		Destroy (currentUI, 0.2f);

			GameObject responseDB1 = new GameObject ();
			StartCoroutine (createDB (DBIndex, DBGet => {
				if(DBGet != null){
					responseDB1 = DBGet;
				}
			}));
			Text response1Txt = responseDB1.GetComponent<Text> ();
			response1Txt.text = "Faust: " + cont1Getter;
			if (cont2Getter != "Empty") {
				yield return new WaitForSeconds (3);

				GameObject responseDB2 = new GameObject ();
				DBIndex++;
				StartCoroutine (createDB (DBIndex, DBGet => {
					if(DBGet != null){
						responseDB2 = DBGet;
					}
				}));
				Text response2Txt = responseDB2.GetComponent<Text> ();
				response2Txt.text = "Faust: " + cont2Getter;
			}
		choice (inputReturn);
		yield return null;
	}
	*/

	//COROUTINE 2:
	/*
	private IEnumerator getResponseII_i(GameObject BLCollection, int BLIndex, int DBIndex, Action<bool> Success){
		Messenger<bool>.Broadcast ("canMove_update", false);
		Canvas[] BLs = BLCollection.GetComponentsInChildren<Canvas> ();
		Canvas BLCalled = BLs.GetValue (BLIndex) as Canvas;
		GameObject responseDB = new GameObject ();
		DBgate = false;
		DBIndex++;
		StartCoroutine (createDB (DBIndex, DBGet => {
			if(DBGet != null){
				responseDB = DBGet;
				DBgate = true;
			}
		}));
		yield return new WaitUntil (() => DBgate);
		lEAccessDB = responseDB;
		Canvas currentUI = Instantiate (BLCalled, BLPrompter.transform);
		InputField lEInputField = currentUI.GetComponentInChildren<InputField> ();
		lEInputField.Select ();
		lyricsEntryHandler rspnsManager = BLCalled.GetComponent<lyricsEntryHandler> ();
		rspnsManager.timerControl (6.0f, true);

		yield return new WaitUntil (() => inputReceived);
		//Dismantling canvas dependencies
		DestroyImmediate (currentUI.gameObject.GetComponent<GraphicRaycaster>());
		DestroyImmediate (currentUI.gameObject.GetComponent<CanvasScaler> ());
		//Removing button loadout
		Destroy (currentUI, 0.2f);

		Success (songSuccess);
		yield return null;
	}
	*/


	//ELEMENTS OF QUEST 1:

	//SECTION 1: 
	/*
	GameObject DBOne = new GameObject ();
		StartCoroutine (createDB (1, DBGet => {
			if(DBGet != null){
				DBOne = DBGet;
			}
		}));
		Text DBOneTxt = DBOne.GetComponent<Text> ();
		//string OrationI = "What's this? A traveler, eh? ";
		//DBOneTxt.text = Orater + OrationI;

		canContinue = false;
		inputReceived = false;
		StartCoroutine (getResponseI_I (btnLoad_Siebel, 0, choiceGet => {
			if(choiceGet != null){
				//choiceI = choiceGet;
				canContinue = true;
			}
		}));

		yield return new WaitUntil (() => canContinue);
		yield return new WaitForSeconds (1.5f);

		if (choiceI != 0) {
			GameObject DBThree = new GameObject ();
			StartCoroutine (createDB (3, DBGet => {
				if (DBGet != null) {
					DBThree = DBGet;
				}
			}));
			Text DBThreeTxt = DBThree.GetComponent<Text> ();
			int resumeTo = 0;
			if (choiceI == 1) {
				//Correct response: Add one Social Point (SP)
				string OrationI_i = "Good day indeed! I suppose Leipzig's an attraction, a little Paris spreading light and culture!";
				//DBThreeTxt.text = Orater + OrationI_i;
				resumeTo = 1;
			} else if (choiceI == 2) {
				string OrationI_i = "A snide one have we here. A smartass by the seems of it! Some good-for-nothing foreign knit-wit?";
				//DBThreeTxt.text = Orater + OrationI_i;
				resumeTo = 2;
			}
			yield return new WaitForSeconds (3f);

			GameObject DBFour = new GameObject ();
			StartCoroutine (createDB (4, DBGet => {
				if (DBGet != null) {
					DBFour = DBGet;
				}
			}));
			Text DBFourTxt = DBFour.GetComponent<Text> ();
			if (resumeTo == 1) {
				string OrationI_ii = "What, fine sir, might be your occupation?";
				//DBFourTxt.text = Orater + OrationI_ii;
			} else if (resumeTo == 2) {
				string OrationI_ii = "What is your vocation?";
				//DBFourTxt.text = Orater + OrationI_ii;
			}
		}
		canContinue = false;
		inputReceived = false;
		StartCoroutine (getResponseI_II (btnLoad_Siebel, 1, choiceGet => {
			if(choiceGet != null){
				choiceII = choiceGet;
				canContinue = true;
			}
		}));

		yield return new WaitUntil (() => canContinue);
		yield return new WaitForSeconds (1.5f);

		if (choiceII != 0) {
			GameObject DBSix = new GameObject ();
			StartCoroutine (createDB (6, DBGet => {
				if(DBGet != null){
					DBSix = DBGet;
				}
			}));
			Text DBSixTxt = DBSix.GetComponent<Text> ();
			int resumeTo = 0;
			if (choiceII == 1) {
				string OrationI_iii = "Relax my friend, you'll have your way - I don't mean to incriminate you!";
				//DBSixTxt.text = Orater + OrationI_iii;
				resumeTo = 1;
			} else if(choiceII == 2) {
				//Correct response: Add one Social Point (SP)
				string OrationI_iii = "Aha! That I respect, a man whose greatest concern is his brew!";
				//DBSixTxt.text = Orater + OrationI_iii;
				resumeTo = 2;
			}
			yield return new WaitForSeconds (3f);

			GameObject DBSeven = new GameObject ();
			StartCoroutine (createDB (7, DBGet => {
				if(DBGet != null) {
					DBSeven = DBGet;
				}
			}));
			Text DBSevenTxt = DBSeven.GetComponent<Text> ();

			if (resumeTo == 1) {
				qBranchI = 0;
				string OrationI_iv = "But do tell me, how have you so far found your vacation?";
				//DBSevenTxt.text = Orater + OrationI_iv;
			} else if (resumeTo == 2) {
				qBranchI = 1;
				string OrationI_iv = "Let me guess, for a man of your stature, wine is what you desire!";
				//DBSevenTxt.text = Orater + OrationI_iv;
			}
		}
		canContinue = false;
		inputReceived = false;
		StartCoroutine (getResponseI_III (btnLoad_Siebel, 2, qBranchI, choiceGet => {
			if(choiceGet != null) {
				choiceIII = choiceGet;
				canContinue = true;
			}
		}));

		yield return new WaitUntil (() => canContinue);
		yield return new WaitForSeconds (1.5f);

		if (choiceIII != 0) {
			GameObject DBTen = new GameObject ();
			DBgate = false;
			StartCoroutine (createDB (10, DBGet => {
				if(DBGet != null){
					DBTen = DBGet;
					DBgate = true;
					Debug.Log("DBTen set");
				}
			}));
			yield return new WaitUntil (() => DBgate);
			Text DBTenTxt = DBTen.GetComponent<Text> ();
			int resumeTo = 0;
			if (qBranchI == 0) {
				if (inputReturn == 1) {
					string OrationI_v = "Oh please, there's no need for flattery. With that kind of talk, you can't pander to me.";
					//DBTenTxt.text = Orater + OrationI_v;
					resumeTo = 1;
				} else if (inputReturn == 2) {
					string OrationI_v = "Haha, well you've found yourself in the right tavern.";
					//DBTenTxt.text = Orater + OrationI_v;
					resumeTo = 2;
				}
			}else if (qBranchI == 1) {
				if (inputReturn == 1) {
					string OrationI_v = "If it's wine you desire, I suggest champagne - you can't always avoid what's foreign.";
					//DBTenTxt.text = Orater + OrationI_v;
					resumeTo = 3;
				} else if (inputReturn == 2) {
					string OrationI_v = "You're in luck, I believe that's served here!";
					//DBTenTxt.text = Orater + OrationI_v;
					resumeTo = 4;
				}
			}
			//Change value to decision-sensitive variable
			yield return new WaitForSeconds (3);

			GameObject DBEleven = new GameObject ();
			StartCoroutine (createDB (11, DBGet => {
				if(DBGet != null){
					DBEleven = DBGet;
				}
			}));
			Text DBElevenTxt = DBEleven.GetComponent<Text> ();
			if (resumeTo == 1) {
				string OrationI_vi = "Leipzig I share no sympathy! It's a city whose very spirit irks me.";
				//DBElevenTxt.text = Orater + OrationI_vi;
				qBranchII = 0;
			} else if (resumeTo == 2) {
				string OrationI_vi = "So then! What beverage would you prefer?";
				//DBElevenTxt.text = Orater + OrationI_vi;
				qBranchII = 1;
			} else if (resumeTo == 3) {
				string OrationI_vi = "I'm not sure we still have any however, talk to Frosch over by the counter.";
				//DBElevenTxt.text = Orater + OrationI_vi;
				qComplete = true;
				qSuccess = true;
			} else if (resumeTo == 4) {
				string OrationI_vi = "Go ask Frosch, up at the bar, he opened a barrel recently... That is, if I remember correctly.";
				//DBElevenTxt.text = Orater + OrationI_vi;
				qComplete = true;
				qSuccess = true;
			}
		}
		yield return new WaitForSeconds (4);
		//Add final response before parting
		//Response to bid NPC farewell
		if(qComplete){
			inputReceived = false;
			FWgate = false;
			StartCoroutine (BidFarewell (btnLoad_Siebel, DBTally, Proceed => {
				if(Proceed != null){
					FWgate = true;
				}
			}));
			yield return new WaitUntil (() => FWgate);
		}
		StartCoroutine (checkCompletion (qComplete, isComplete => {
			if(isComplete != null && isComplete){
				canReturn = isComplete;
			}
		}));
		//Continuation for uncompleted routes
		if (!canReturn) {
			canContinue = false;
			inputReceived = false;
			StartCoroutine (getResponseI_IV (btnLoad_Siebel, 4, qBranchII, choiceGet => {
				if(choiceGet != null){
					choiceIV = choiceGet;
					canContinue = true;
				}
			}));
			yield return new WaitUntil (() => canContinue);
			yield return new WaitForSeconds (1.5f);

			if (choiceIV != 0) {
				GameObject DBThirteen = new GameObject ();
				StartCoroutine (createDB (13, DBGet => {
					if(DBGet != null){
						DBThirteen = DBGet;
					}
				}));
				Text DBThirteenTxt = DBThirteen.GetComponent<Text> ();
				string OrationI_vii = "Holder";
				int resumeTo = 0;
				if (qBranchII == 0) {
					if (choiceIV == 1) {
						OrationI_vii = "I need not your pity! To hell with you and your insults.";
						qComplete = true;
					} else if (choiceIV == 2) {
						OrationI_vii = "Okay then, surprise me!";
						resumeTo = 2;
					}
				} else if (qBranchII == 1) {
					if (choiceIV == 1) {
						OrationI_vii = "If it's wine that you desire, I suggest champagne - you can't always avoid what's foreign.";
						resumeTo = 3;
					} else if (choiceIV == 2) {
						OrationI_vii = "You're in luck, I believe wine of that type is served here!";
						resumeTo = 4;
					}
				}
				//DBThirteenTxt.text = Orater + OrationI_vii;

				if (resumeTo != 0) {
					yield return new WaitForSeconds (3);

					GameObject DBFourteen = new GameObject ();
					StartCoroutine (createDB (14, DBGet => {
						if(DBGet != null){
							DBFourteen = DBGet;
						}
					}));
					Text DBFourteenTxt = DBFourteen.GetComponent<Text> ();
					string OrationI_viii = "Go talk to Frosch up at the bar.";
					//DBFourteenTxt.text = Orater + OrationI_viii;

					qComplete = true;
					qSuccess = true;
					yield return new WaitForSeconds (2f);

					inputReceived = false;
					FWgate = false;
					StartCoroutine (BidFarewell (btnLoad_Siebel, DBTally, Proceed => {
						if(Proceed != null){
							FWgate = true;
						}
					}));
					yield return new WaitUntil (() => FWgate);
				}
			}
			StartCoroutine (checkCompletion (qComplete, isComplete => {
				if(isComplete != null && isComplete){
					Debug.Log("Ran");
					canReturn = isComplete;
				}
			}));
		}
		yield return new WaitUntil (() => canReturn);
	*/

	//*OBSOLETE* GETRESPONSE METHODS:

	//COROUTINE 1:
	/*
	private IEnumerator getResponseI_I(GameObject BLCollection, int BLIndex, Action<int> choice){
		Canvas[] BLs = BLCollection.GetComponentsInChildren<Canvas> ();
		Canvas BLCalled = BLs.GetValue (BLIndex) as Canvas;
		Canvas currentUI = Instantiate (BLCalled, BLPrompter.transform);

		yield return new WaitUntil (() => inputReceived);
		//Dismantling canvas dependencies
		DestroyImmediate (currentUI.gameObject.GetComponent<GraphicRaycaster>());
		DestroyImmediate (currentUI.gameObject.GetComponent<CanvasScaler> ());
		//Removing button load
		Destroy (currentUI, 0.2f);

			string ResponseI = cont1Getter;
			GameObject DBTwo = new GameObject ();
			StartCoroutine (createDB (2, DBGet => {
				if(DBGet != null) {
					DBTwo = DBGet;
				}
			}));
			Text DBTwoTxt = DBTwo.GetComponent<Text> ();
			DBTwoTxt.text = "Faust: " + ResponseI;

			choice (inputReturn);
			//FALSIFY inputReceived
		yield return null;
	}
	*/

	//COROUTINE 2:
	/*
	 * private IEnumerator getResponseI_II(GameObject BLCollection, int BLIndex, Action<int> choice){
		Canvas[] BLs = BLCollection.GetComponentsInChildren<Canvas> ();
		Canvas BLCalled = BLs.GetValue (BLIndex) as Canvas;
		Canvas currentUI = Instantiate (BLCalled, BLPrompter.transform);

		yield return new WaitUntil (() => inputReceived);
		//Dismantling canvas dependencies
		DestroyImmediate (currentUI.gameObject.GetComponent<GraphicRaycaster>());
		DestroyImmediate (currentUI.gameObject.GetComponent<CanvasScaler> ());
		//Removing button load
		Destroy (currentUI, 0.2f);

			string ResponseII = cont1Getter;
			GameObject DBFive = new GameObject ();
			StartCoroutine (createDB (5, DBGet => {
				if(DBGet != null) {
					DBFive = DBGet;
				}
			}));
			Text DBFiveTxt = DBFive.GetComponent<Text> ();
			DBFiveTxt.text = "Faust: " + ResponseII;

			choice (inputReturn);
		yield return null;
	}*/

	//COROUTINE 3:
	/*
	private IEnumerator getResponseI_III(GameObject BLCollection, int BLIndex, int branchPath, Action<int> choice){
		Canvas[] BLs = BLCollection.GetComponentsInChildren<Canvas> ();
		//Assigning placeholder for accessible declaration
		Canvas BLCalled = new Canvas ();
		BLIndex = BLIndex + branchPath;
		BLCalled = BLs.GetValue (BLIndex) as Canvas;
		Canvas currentUI = Instantiate (BLCalled, BLPrompter.transform);

		yield return new WaitUntil (() => inputReceived);
		//Dismantling canvas dependencies
		DestroyImmediate (currentUI.gameObject.GetComponent<GraphicRaycaster>());
		DestroyImmediate (currentUI.gameObject.GetComponent<CanvasScaler> ());
		//Removing button load
		Destroy (currentUI, 0.2f);

			string ResponseIII_i = cont1Getter;
			GameObject DBEight = new GameObject();
			StartCoroutine (createDB(8, DBGet => {
				if(DBGet != null){
					DBEight = DBGet;
				}
			}));
			Text DBEightTxt = DBEight.GetComponent<Text> ();
			DBEightTxt.text = "Faust: " + ResponseIII_i;
			//Account for 2nd part of response [NON-FUNCTIONAL CODE: START]
			if (branchPath == 1 && inputReturn == 3) {
				string ResponseIII_ii = cont2Getter;
				GameObject DBNine = new GameObject ();
				StartCoroutine (createDB (9, DBGet => {
					if(DBGet != null){
						DBNine = DBGet;
					}
				}));
				Text DBNineTxt = DBNine.GetComponent<Text> ();

				yield return new WaitForSeconds (3.0f);
				DBNineTxt.text = "Faust: " + ResponseIII_ii;
			}
			//[NON-FUNCTIONAL CODE: END]
			choice(inputReturn);
		yield return null;
	}
	*/

	//COROUTINE 4:
	/*
	private IEnumerator getResponseI_IV(GameObject BLCollection, int BLIndex, int branchPath, Action<int> choice){
		Canvas[] BLs = BLCollection.GetComponentsInChildren<Canvas> ();
		Canvas BLCalled = new Canvas ();
		BLIndex = BLIndex + branchPath;
		BLCalled = BLs.GetValue (BLIndex) as Canvas;
		Canvas currentUI = Instantiate (BLCalled, BLPrompter.transform);

		yield return new WaitUntil (() => inputReceived);
		//Dismantling canvas dependencies
		DestroyImmediate (currentUI.gameObject.GetComponent<GraphicRaycaster>());
		DestroyImmediate (currentUI.gameObject.GetComponent<CanvasScaler> ());
		//Removing button load
		Destroy (currentUI, 0.2f);

		GameObject DBTwelve = new GameObject ();
		StartCoroutine (createDB (12, DBGet => {
			if(DBGet != null){
				DBTwelve = DBGet;
			}
		}));
		string ResponseIV = cont1Getter;
		Text DBTwelveTxt = DBTwelve.GetComponent<Text> ();
		DBTwelveTxt.text = "Faust: " + ResponseIV;

		choice (inputReturn);
		yield return null;
	}
	*/


	//"BID FAREWELL" METHOD:
	/*
	private IEnumerator BidFarewell(GameObject BLCollection, int createdDBs, Action<bool> hasRan){
		Canvas[] Bls = BLCollection.GetComponentsInChildren<Canvas> ();
		Canvas BLCalled = new Canvas ();
		int BLIndex = Bls.Length - 1;
		Debug.Log (BLIndex);
		BLCalled = Bls.GetValue (BLIndex) as Canvas;

		Canvas currentUI = Instantiate (BLCalled, BLPrompter.transform);

		yield return new WaitUntil (() => inputReceived);
		//Dismantling canvas dependencies
		DestroyImmediate (currentUI.gameObject.GetComponent<GraphicRaycaster>());
		DestroyImmediate (currentUI.gameObject.GetComponent<CanvasScaler> ());
		//Removing button load
		Destroy (currentUI, 0.2f);

		GameObject DBFinal = new GameObject ();
		//Convert from 12 to tally of created DBs
		createdDBs = createdDBs + 2;
		StartCoroutine (createDB (createdDBs, DBGet => {
			if(DBGet != null){
				DBFinal = DBGet;
			}
		}));
		Text DBFinalTxt = DBFinal.GetComponent<Text> ();
		string FarewellText = cont1Getter;
		DBFinalTxt.text = "Faust: " + FarewellText;

		hasRan (true);
		yield return null;
	}
	*/


	//*OBSOLETE* CREATEDB METHOD: 
	/*
	private IEnumerator createDB(int i, Action<GameObject> DB){
		int dbCongruence = (i - 1) % 9;
		if (dbCongruence == 0) {
			GameObject[] activeDBs = GameObject.FindGameObjectsWithTag ("DBSlot");
			if (activeDBs.Length > 0) {
				yield return new WaitForSeconds (1.5f);
				foreach(GameObject activeDB in activeDBs){
					Destroy (activeDB);
				}
			}
		}
		//Creating new dialogue box (Generic DB)
		GameObject tempDB = new GameObject ();
		Text tempDBTxt = tempDB.AddComponent<Text> ();
		tempDB.gameObject.tag = "DBSlot";
		tempDB.gameObject.name = "DB" + i.ToString ();
		tempDB.transform.SetParent (dBCanvas.transform);
		//Setting respective position
		float yCoord = modelDB.transform.localPosition.y;
		yCoord -= 50 * dbCongruence;
		Vector3 tempPos = new Vector3 (-122, yCoord, 0);
		tempDB.transform.localPosition = tempPos;
		//Establishing DB inheritance of modelDB
		tempDBTxt.rectTransform.anchorMax = new Vector2 (1, .5f);
		tempDBTxt.rectTransform.anchorMin = new Vector2 (1, .5f);
		tempDBTxt.rectTransform.sizeDelta = new Vector2 (220, 50);
		tempDBTxt.font = modelDB.font;
		tempDBTxt.fontSize = modelDB.fontSize;
		tempDBTxt.color = modelDB.color;
		//Returning respective DB via lambda callback
		DB (tempDB);
		DBTally++;
		yield return null;
	}
	*/
}
