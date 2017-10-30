using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class questPackages : MonoBehaviour{
	//!!Keep class for name reference!!
}
//Interface of all quest-loads
public interface IqBase{
	//Declaration of inherit quest methods
	void qPack_AI(string callTag);
	void qPack_Mn(int Loader);
}

//QUESTS GO HERE; INCLUDE INTERFACE "IqBase" FOR SEAMLESS INTEGRATION:
public class questPackageI : MonoBehaviour, IqBase{
	//Fill manually for quest run path
	private DialogueManager dlgInterface;
	private DictionaryConverter dDictionaryArchive;
	private GameObject dlgManager;
	private GameObject Prompter;
	private GameObject Player;
	private Camera playerView;
	private PlayerController movementRef;
	private GameObject tDialogueCanvas;
	private GameObject ftTimer;
	//private GameObject BlackoutDisplay;

	private GameObject brawlersRef;
	private brawlTagger[] Brawlers;
	private PlayerCombat PvERef;
	private GameObject BrawlerBoss;

	private GameObject prmptCanvas;
	private Canvas pDCanvas;
	private Image prmptDisplay;
	private Text pDText;

	//REFORMAT
	private LightsScript cellarLight;
	private bool inputReceived;
	private bool canContinue;
	private bool DBgate;
	private bool FWgate;
	private bool success_stgI;
	private bool success_stgII;
	private int inputReturn;
	private List<int> stgRef;
	private int activeQNPC;
	private playerStatusLog qStatus;
	private Canvas dBCanvas;
	private Text tempTxt;
	private bool songSuccess;
	private string cont1Getter;
	private string cont2Getter;

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
		stgRef_Setup ();

		tDialogueCanvas = GameObject.FindGameObjectWithTag ("testerCanvas");
		dDictionaryArchive = tDialogueCanvas.GetComponent<DictionaryConverter> ();
		dlgManager = GameObject.FindGameObjectWithTag ("DialogueManager");
		dlgInterface = dlgManager.GetComponent<DialogueManager> ();

		Player = GameObject.FindGameObjectWithTag ("User");
		playerView = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		movementRef = Player.GetComponent<PlayerController> ();
		Prompter = GameObject.FindGameObjectWithTag ("tBox");

		assignPrmptCanvas ();

		cellarLight = Player.GetComponentInChildren<LightsScript> ();
		ftTimer = GameObject.FindGameObjectWithTag ("ftTimer");
		ftTimer.SetActive (false);

		PvERef = Player.GetComponent<PlayerCombat> ();

		qStatus = Player.GetComponent<playerStatusLog> ();
		qStatus.qPhaseSet (stgRef);
		canContinue = false;
	}
	public void stgRef_Setup(){
		stgRef = new List<int> ();
		//Add foreach with # of NPCs
		stgRef.Add (0);
		stgRef.Add (0);
		stgRef.Add (0);
	}

	//Misc. Button Input Handlers:
	public void compileDecision(int btnSelection){
		inputReceived = true;
		inputReturn = btnSelection;
		Debug.Log (inputReturn);
	}
	public void LEGate(bool LESuccess){
		inputReceived = true;
		if (LESuccess) {
			inputReturn = 1;
		} else if (!LESuccess) {
			inputReturn = 0;
		}
	}


	//NPC-SIDE QUEST-MANAGER INTERFACE PACKAGE:
	public void qPack_AI(string caller){
		if (qStatus.onQuest == 0) {
			//SET TO ZERO AFTER COMPLETION
			qStatus.onQuest = 1;
		}
		if (caller == "qNPC_I") {
			Debug.Log ("T");
			if (qStatus.onQuest == 1) {
				if (qStatus.qPhase [0] == 0) {
					//Issue quest 1:
					qStatus.onQuest = 2;
					Messenger<int>.AddListener ("contBtnRtrn", compileDecision);

					StartCoroutine (firstStage (wasSuccessful => {
						if (wasSuccessful != null) {
							Debug.Log ("Returned to start");
							success_stgI = wasSuccessful;
							if (success_stgI) {
								Debug.Log ("Quest1 Successfully Completed");
								qStatus.onQuest = 0;
								Messenger<int>.RemoveListener ("contBtnRtrn", compileDecision);
							}
						}
					}));
				}
			}
		} else if (caller == "qNPC_II") {
			if (qStatus.onQuest == 1) {
				if (/*qStatus.qPhase [0] == 2 &&*/ qStatus.qPhase [1] == 0) {
					//Issue quest 2:
					//ADD MOVEMENT SCRIPT PARAMETER TO HALT MOVEMENT UPON ENGAGEMENT OF QUEST
					qStatus.onQuest = 2;
					Messenger<int>.AddListener ("contBtnRtrn", compileDecision);
					Messenger<bool>.AddListener ("LERtrn", LEGate);

					StartCoroutine (secondStage (wasSuccessful => {
						if (wasSuccessful != null) {
							Debug.Log ("Returned to start");
							success_stgII = wasSuccessful;
							if (success_stgII) {
								Debug.Log ("Quest2 Successfully Completed");
								qStatus.onQuest = 0;
								Messenger<int>.RemoveListener ("contBtnRtrn", compileDecision);
								Messenger<bool>.RemoveListener ("LERtrn", LEGate);
							}
						}
					}));
				}
			}
		} else if (caller == "qNPC_III") {
			if (qStatus.onQuest == 1) {
				if (/*qStatus.qPhase[1] == 2 &&*/ qStatus.qPhase [2] == 0) {
					qStatus.onQuest = 2;
					Messenger<int>.AddListener ("contBtnRtrn", compileDecision);

					StartCoroutine (thirdStage (wasSuccessful => {
						if (wasSuccessful != null) {
							Debug.Log ("Returned to start");

							Messenger<bool, int>.Broadcast ("MnPackAssignment", true, 0);
							Messenger<bool, int>.Broadcast ("(un)lockCellar", true, 1);
						}
					}));
				}
			}
		} else if (caller == "qNPC_IV") {
			if (qStatus.onQuest == 1) {
				if (/*qStatus.qPhase[2] == 2 &&*/ qStatus.qPhase [3] == 0) {
					qStatus.onQuest = 2;

					StartCoroutine (fourthStage (wasSuccessful => {
						if (wasSuccessful != null) {
							Debug.Log ("Returned to start");
						}
					}));
				}
			}
		}
		else if (caller == "qNPC_Tester") {
			Debug.Log ("Engaged Test NPC, NPC Inactive");
			qStatus.onQuest = 2;
			Messenger<int>.AddListener ("contBtnRtrn", compileDecision);
			//StartCoroutine (testerStage());
		}
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start:
	private IEnumerator displayResponse(Dictionary<string, string> responseArchive, string path, int rdType){
		string[] options_Filler = new string[0];

		string[] Response = new string[]{ responseArchive [path] };
		dlgInterface.outputDialogue ("Faust", rdType, Response, options_Filler, -1);

		yield return new WaitForSeconds (5.5f);

		canContinue = true;
		yield return null;
	}

	private IEnumerator fourthStage(Action<bool> Accomplished){
		Debug.Log ("Engaged Brawl Lead");
		string NPCname = "Brawler";
		string[] options_Filler = new string[0];
		qStatus.qPhase [3] = 1;

		string usrOrationI_a = "What's going on?";
		string[] usrOration = new string[]{ usrOrationI_a };

		dlgInterface.outputDialogue ("Faust", 1, usrOration, options_Filler, -1);

		yield return new WaitForSeconds (4);

		string OrationI_a = "What's it to you, you nosey tourist.";
		string OrationI_b = "I'll teach you to keep your mouth shut.";
		string[] OrationI = new string[]{ OrationI_a, OrationI_b };

		dlgInterface.outputDialogue (NPCname, 1, OrationI, options_Filler, -1);

		yield return new WaitForSeconds (4);

		dlgInterface.EndDialogue ();
		//Call method to begin fight
		yield break;
	}

	private IEnumerator thirdStage(Action<bool> Accomplished){
		Debug.Log ("Engaged Altmayer");
		string NPCname = "Altmayer";
		string[] contOptions = new string[2];
		int qBranchI = -1;
		int qBranchII = -1;
		int chantKeyHolder = -1;
		//Set qPhase to 1 to denote quest initiation
		qStatus.qPhase [2] = 1;

		string OrationI_a = "Hey there youngin, I'm afraid you got me at a bad time, I was about to head down into the cellar.";
		string OrationI_b = "Say, would you mind going to fetch the wine for me?";
		string[] OrationI = new string[]{ OrationI_a, OrationI_b };
		contOptions = new string[]{ "Accept" };
		//Display appropriate dialogue
		dlgInterface.outputDialogue (NPCname, 1, OrationI, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);
		qBranchI = inputReturn;

		string[] OrationII = new string[2];
		if (qBranchI == 2) {
			string OrationII_a = "Thanks, I don't mean to impose, but hauling up barrels is taxing on my old body.";
			string OrationII_b = "Just find and bring the whole keg up, and make sure you use the torch.";
			OrationII = new string[]{ OrationII_a, OrationII_b };
			contOptions = new string[]{ "Leave" };
		} else {
			Debug.Log ("Error");
		}
		//Display appropriate dialogue
		dlgInterface.outputDialogue (NPCname, 1, OrationII, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);
		qBranchII = inputReturn;

		if (qBranchII == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			//Set qPhase to 2 to denote quest completion
			qStatus.qPhase [2] = 2;

			Accomplished (true);
			yield break;
		}
	}

	private IEnumerator secondStage(Action<bool> Accomplished){
		Debug.Log ("Engaged Frosch");
		Dictionary<string, string> dialogueRef_NPC = dDictionaryArchive.sentences_Frosch;
		Dictionary<string, string> dialogueRef_User = dDictionaryArchive.responses_Frosch;
		string NPCname = "Frosch";
		string branchPath = "fshDialogue";
		string[] contOptions = new string[2];
		int qBranchI = -1;
		int qBranchII = -1;
		int qBranchIII = -1;
		int qBranchIV = -1;
		int qBranchV = -1;
		int qBranchVI = -1;
		int chantKeyHolder = -1;
		int chantKey = 0;
		//Set qPhase to 1 to denote quest initiation
		qStatus.qPhase[1] = 1;

		string OrationI_a = dialogueRef_NPC [branchPath];
		string[] OrationI = new string[]{ OrationI_a };
		contOptions = new string[]{ "Mention Siebel", "Greet" };
		//Display appropriate dialogue
		dlgInterface.outputDialogue (NPCname, 1, OrationI, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchI = inputReturn;
		branchPath = branchPath + qBranchI.ToString ();
		canContinue = false;
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

		yield return new WaitUntil (() => canContinue);

		string OrationII_a = dialogueRef_NPC [branchPath + "a"];
		string OrationII_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationII = new string[]{ OrationII_a, OrationII_b };
		if (qBranchI == 0) {
			//PATH ROUTE: 0
			contOptions = new string[]{ "Accept", "Decline" };
		} else if (qBranchI == 1) {
			//PATH ROUTE: 1
			contOptions = new string[]{ "Make Small Talk", "Ask About Wine" };
		}
		//Display appropriate dialogue
		dlgInterface.outputDialogue (NPCname, 1, OrationII, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchII = inputReturn;
		branchPath = branchPath + qBranchII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIII_a = dialogueRef_NPC [branchPath + "a"];
		string OrationIII_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationIII = new string[]{ OrationIII_a, OrationIII_b };
		//Differentiates between LE response and contButton response
		int rspnsTypeIII = 0;
		int rdTypeI = 0;
		if (qBranchI == 0) {
			//PATH ROUTE: 0*
			rspnsTypeIII = 2;
			rdTypeI = -1;
			chantKey = 0;
		} else if(qBranchI == 1){
			rspnsTypeIII = 1;
			if (qBranchII == 0) {
				//PATH ROUTE: 10
				rdTypeI = 1;
				contOptions = new string[]{ "Further Inquire", "Ask About Wine" };
			} else if (qBranchII == 1) {
				//PATH ROUTE: 11
				rdTypeI = 1;
				contOptions = new string[]{ "Mention Siebel", "Discuss Wine" };
			}
		}
		dlgInterface.outputDialogue (NPCname, rspnsTypeIII, OrationIII, contOptions, chantKey);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIII = inputReturn; 
		if (qBranchI == 0 && qBranchIII == 0) {
			//Quest failed
			Debug.Log ("Failed LE; Quest failed");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [1] = 2;

			//Return to start:
			Accomplished (true);
			yield break;
		}
		branchPath = branchPath + qBranchIII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, rdTypeI));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIV_a = dialogueRef_NPC [branchPath + "a"];
		string OrationIV_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationIV = new string[]{ OrationIV_a, OrationIV_b };
		//Differentiates between LE response and contButton response
		int rspnsTypeIV = 0;
		int rdTypeII = 0;
		if (qBranchI == 0) {
			if (qBranchIII == 1) {
				//PATH ROUTE: 0*1
				rdTypeII = -1;
				rspnsTypeIV = 2;
				chantKey = 1;
			}
		} else if (qBranchI == 1) {
			rspnsTypeIV = 1;
			rdTypeII = 1;
			if (qBranchII == 0) {
				if (qBranchIII == 0) {
					//PATH ROUTE: 100
					contOptions = new string[]{ "Decline", "Accept" };
				} else if (qBranchIII == 1) {
					//PATH ROUTE: 101
					//Quest completed
					contOptions = new string[]{ "Leave" };
				}
			} else if (qBranchII == 1) {
				//PATH ROUTE: 11*
				//Quest completed
				contOptions = new string[]{ "Leave" };
			}
		}
		//Display appropriate dialogue
		dlgInterface.outputDialogue (NPCname, rspnsTypeIV, OrationIV, contOptions, chantKey);
		
		
		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIV = inputReturn;
		if (qBranchIV == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [1] = 2;

			Accomplished (true);
			yield break;
		} else if (qBranchI == 0 && qBranchIV == 0) {
			//Quest failed
			Debug.Log ("Failed LE; Quest failed");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [1] = 2;

			//Return to start:
			Accomplished (true);
			yield break;
		}

		branchPath = branchPath + qBranchIV.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, rdTypeII));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationV_a = dialogueRef_NPC [branchPath + "a"];
		string OrationV_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationV = new string[]{ OrationV_a, OrationV_b };
		//Differentiates between LE response and contButton response
		int rspnsTypeV = 0;
		int rdTypeIII = 0;
		if (qBranchI == 0) {
			if (qBranchIV == 1) {
				//PATH ROUTE: 0*11
				rdTypeIII = -1;
				rspnsTypeV = 2;
				chantKey = 2;
			}
		} else if (qBranchI == 1) {
			rspnsTypeV = 1;
			rdTypeIII = 1;
			if (qBranchIV == 0) {
				//PATH ROUTE: 1000
				//Quest failed
				contOptions = new string[]{ "Leave" };
			} else if (qBranchIV == 1) {
				//PATH ROUTE: 1001
				//Quest completed
				contOptions = new string[]{ "Leave" };
			}
		}
		//Display appropriate dialogue
		dlgInterface.outputDialogue (NPCname, rspnsTypeV, OrationV, contOptions, chantKey);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchV = inputReturn;
		Debug.Log (qBranchV);
		if (qBranchV == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [1] = 2;

			Accomplished (true);
			yield break;
		} else if (qBranchV == 0) {
			//Quest failed
			Debug.Log ("Failed LE; Quest failed");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [1] = 2;

			//Return to start
			Accomplished (true);
			yield break;
		}

		branchPath = branchPath + qBranchV.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, rdTypeIII));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationVI_a = dialogueRef_NPC [branchPath + "a"];
		string OrationVI_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationVI = new string[]{ OrationVI_a, OrationVI_b };
		//Differentiates between LE response and contButton response
		int rspnsTypeVI = 1;
		if (qBranchV == 1) {
			//PATH ROUTE: 0*111
			contOptions = new string[]{ "Leave" };
		} else {
			Debug.Log ("Error");
		}
		//Display appropriate dialogue
		dlgInterface.outputDialogue (NPCname, rspnsTypeVI, OrationVI, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchVI = inputReturn;
		if (qBranchVI == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [1] = 2;

			//Return to start
			Accomplished (true);
			yield break;
		} else {
			Debug.Log ("Error");
		}

		//*TEMPORARY*
		canContinue = false;
		yield return new WaitUntil (() => canContinue);

	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	private IEnumerator firstStage(Action<bool> Accomplished){
		Debug.Log ("Engaged Siebel");
		Dictionary<string, string> dialogueRef_NPC = dDictionaryArchive.sentences_Siebel;
		Dictionary<string, string> dialogueRef_User = dDictionaryArchive.responses_Siebel;
		string NPCname = "Siebel";
		string branchPath = "sblDialogue";
		string[] contOptions = new string[2];
		int qBranchI = -1;
		int qBranchII = -1;
		int qBranchIII = -1;
		int qBranchIV = -1;
		int chantKeyHolder = -1;
		bool qComplete = false;
		//Set qPhase to 1 to denote quest initiation
		qStatus.qPhase[0] = 1;

		string[] OrationI = new string[]{ dialogueRef_NPC [branchPath] };
		contOptions = new string[]{ "Greet", "Dismiss" };
		dlgInterface.outputDialogue (NPCname, 1, OrationI, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchI = inputReturn;
		branchPath = branchPath + qBranchI.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationII_a = dialogueRef_NPC [branchPath + "a"];
		string OrationII_b = dialogueRef_NPC [branchPath + "b"];
		string[] OrationII = new string[]{ OrationII_a, OrationII_b };
		if (qBranchI == 0) {
			contOptions = new string[]{ "Answer", "Change Topic" };
		} else if (qBranchI == 1) {
			contOptions = new string[]{ "Answer", "Digress" };
		}
		dlgInterface.outputDialogue (NPCname, 1, OrationII, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchII = inputReturn;
		branchPath = branchPath + qBranchII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIII_a = "Holder";
		string OrationIII_b = "Holder";
		string[] OrationIII = new string[]{ };
		if (qBranchI == 0) {
			OrationIII_a = dialogueRef_NPC [branchPath + "a"];
			OrationIII_b = dialogueRef_NPC [branchPath + "b"];
			OrationIII = new string[]{ OrationIII_a, OrationIII_b };
			if (qBranchII == 0) {
				contOptions = new string[]{ "Accept Offer", "Decline Offer" };
			} else if (qBranchII == 1) {
				contOptions = new string[]{ "Ask Preferences", "Agree" };
			}
		} else if(qBranchI == 1){
			if (qBranchII == 0) {
				OrationIII = new string[]{ dialogueRef_NPC [branchPath] };
				contOptions = new string[]{ "Change Topic", "Attempt to Pander" };
			} else if (qBranchII == 1) {
				OrationIII_a = dialogueRef_NPC [branchPath + "a"];
				OrationIII_b = dialogueRef_NPC [branchPath + "b"];
				OrationIII = new string[]{ OrationIII_a, OrationIII_b };
				contOptions = new string[]{ "Ask Preferences", "Ask for Wine" };
			}
		}
		dlgInterface.outputDialogue (NPCname, 1, OrationIII, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIII = inputReturn;
		branchPath = branchPath + qBranchIII.ToString ();
		StartCoroutine (displayResponse (dialogueRef_User, branchPath, 1));

		canContinue = false;
		yield return new WaitUntil (() => canContinue);

		string OrationIV_a = "Holder";
		string OrationIV_b = "Holder";
		string[] OrationIV = new string[]{ };
		if (qBranchI == 0) {
			if (qBranchII == 0) {
				OrationIV = new string[]{ dialogueRef_NPC [branchPath] };

			} else if (qBranchII == 1) {
				if (qBranchIII == 0) {
					OrationIV_a = dialogueRef_NPC [branchPath + "a"];
					OrationIV_b = dialogueRef_NPC [branchPath + "b"];
					OrationIV = new string[]{ OrationIV_a, OrationIV_b };
				} else if (qBranchIII == 1) {
					OrationIV = new string[]{ dialogueRef_NPC [branchPath] };
				}
			}
			contOptions = new string[]{ "Leave" };
			qComplete = true;
		} else if (qBranchI == 1) {
			if (qBranchII == 0) {
				OrationIV_a = dialogueRef_NPC [branchPath + "a"];
				OrationIV_b = dialogueRef_NPC [branchPath + "b"];
				OrationIV = new string[]{ OrationIV_a, OrationIV_b };
				contOptions = new string[]{ "Leave" };
				qComplete = true;
			} else if (qBranchII == 1) {
				if (qBranchIII == 0) {
					//CONTINUATION
					OrationIV_a = dialogueRef_NPC [branchPath + "a"];
					//CORRECT CORRESPONDING STRING IN JSON ARRAY
					OrationIV_b = dialogueRef_NPC [branchPath + "b"];
					OrationIV = new string[]{ OrationIV_a, OrationIV_b };
					contOptions = new string[]{ "Leave" };
					qComplete = true;
				} else if (qBranchIII == 1) {
					//RECONFIGURE AND ADD CORRESPONDING STRINGS
					OrationIV_a = dialogueRef_NPC [branchPath + "a"];
					OrationIV_b = dialogueRef_NPC [branchPath + "b"];
					OrationIV = new string[]{ OrationIV_a, OrationIV_b };
					contOptions = new string[]{ "Leave" };
					//ADD RESTART HERE
					Debug.Log ("Quest failed; prompting restart option.");
				}
			}
		}
		//Tester
		if (qComplete) {
			Debug.Log ("Quest successful");
		}
		dlgInterface.outputDialogue (NPCname, 1, OrationIV, contOptions, chantKeyHolder);

		inputReceived = false;
		yield return new WaitUntil (() => inputReceived);

		qBranchIV = inputReturn;
		if (qBranchIV == 2) {
			Debug.Log ("Quest Complete");
			dlgInterface.EndDialogue ();
			qStatus.qPhase [0] = 2;

			Accomplished (true);
			yield break;
		} else {
			Debug.Log ("Continuing");
		}

		//*TEMPORARY*
		canContinue = false;
		yield return new WaitUntil (() => canContinue);
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish

	

	private bool reachedObjectiveI = false;
	private bool inCellar = false;
	private bool firstIteration = true;
	private bool confrontedBoss = false;
	private bool bossEliminated = false;
	private bool playerEliminated = false;
	private int finalEventType;
	private int brawlerIndex;
	private GameObject activeBrawler;
	private npcCombat brawlComponent;
	private bossStatusLog bossLog;

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	public void updateInCellar(bool updateBool){
		inCellar = updateBool;
	}
	public void updateObjReachedI(){
		reachedObjectiveI = true;
		Debug.Log ("Bool set");
	}
	//Called to assign prompt canvas components
	public void assignPrmptCanvas(){
		prmptCanvas = GameObject.FindGameObjectWithTag ("promptCanvas");
		pDCanvas = prmptCanvas.GetComponentInChildren<Canvas> ();
		prmptDisplay = pDCanvas.GetComponentInChildren<Image> ();
		pDText = prmptDisplay.GetComponentsInChildren<Text> () [0];
		pDCanvas.gameObject.SetActive (false);
	}
	//Called to assign brawler references (post-cellar)
	public void assignBrawlers(){
		brawlersRef = GameObject.FindGameObjectWithTag ("Brawlers");
		Brawlers = brawlersRef.GetComponentsInChildren<brawlTagger> ();
		BrawlerBoss = GameObject.FindGameObjectWithTag ("Boss");
	}
	public void newBrawler(){
		//Spawning new Brawlers
		brawlerIndex++;
		if (brawlerIndex < Brawlers.Length) {
			float zoomRatio = ((float)(brawlerIndex + 1) / (float)Brawlers.Length);
			Debug.Log (zoomRatio);
			playerView.orthographicSize = 4 - zoomRatio;
			activeBrawler = Brawlers [brawlerIndex].gameObject;

			brawlComponent = activeBrawler.AddComponent<npcCombat> ();
			brawlComponent.DmgReceiver_NPCSide += PvERef.HandleEnemyAttack;
			brawlComponent.isTracking = true;
			brawlComponent.Target = Player;
			brawlComponent.brawlTier = 1;
			Debug.Log ("New Brawler Activated");
		} else {
			Debug.Log ("All Brawlers have been defeated");
		}
	}
	public void readyBoss(){
		//Readying Boss
		brawlComponent = BrawlerBoss.AddComponent<npcCombat> ();
		brawlComponent.DmgReceiver_NPCSide += PvERef.HandleEnemyAttack;
		brawlComponent.brawlTier = 2;
		brawlComponent.chaseSpeed = 3.2f;

		bossLog = BrawlerBoss.AddComponent<bossStatusLog> ();
		brawlComponent.statusRef = bossLog;
		Debug.Log ("Boss Activated");
	}
	public void bossBtlInitiator(){
		confrontedBoss = true;
	}
	public void finalEventHandler(int eventType){
		if (eventType == 1) {
			bossEliminated = true;
		} else if (eventType == 2) {
			playerEliminated = true;
		}
		finalEventType = eventType;
	}
	public void promptHint(){
		//**MIGRATE**
		pDCanvas.gameObject.SetActive (true);
		pDText.text = "This enemy is stronger and more aggressive than you, he cannot be defeated in melee combat. Be resourceful when battling this opponent.";
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish

	//MANAGER QUEST-INTERFACE PACKAGE:
	public void qPack_Mn(int MnLoadout){
		if (MnLoadout == 0) {
			Messenger<bool>.AddListener ("inCellarUpdate", updateInCellar);
			Messenger.AddListener ("objectiveIReached", updateObjReachedI);
			StartCoroutine (firstMnQ (wasSuccessful => {
				if (wasSuccessful != null) {
					Debug.Log ("Returned to Start");
					if (wasSuccessful) {
						qStatus.onQuest = 0;
						//Broadcast to activate MnLoadout1
						Messenger<bool, int>.Broadcast ("MnPackAssignment", true, 1);
					}
				}
			}));
		} else if (MnLoadout == 1) {
			//Creating necessary Messengers
			Messenger.AddListener ("brawlerDefeated", newBrawler);
			Messenger.AddListener ("BossEngaged", bossBtlInitiator);
			Messenger.AddListener ("firstAttack", promptHint);
			Messenger<int>.AddListener ("finalEventType", finalEventHandler);
			StartCoroutine (secondMnQ (wasSuccessful => {
				if (wasSuccessful != null) {
					Debug.Log ("Quest completed; returned to start");
				}
			}));
		}
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	private IEnumerator secondMnQ(Action<bool> Accomplished){
		Debug.Log ("Bar fight initiated");
		if (firstIteration) {
			Debug.Log ("First Brawler deployed");
			assignBrawlers ();
			brawlerIndex = -1;
			newBrawler ();

			firstIteration = false;
		}

		yield return new WaitUntil (() => (brawlerIndex == Brawlers.Length));

		Debug.Log ("All brawlers eliminated: Proceeding");
		readyBoss ();
		Debug.Log ("Defend Altmayer");

		yield return new WaitUntil (() => confrontedBoss);

		Debug.Log ("Initiated boss battle");
		brawlComponent.isTracking = true;
		brawlComponent.Target = Player;

		yield return new WaitUntil (() => bossLog.hitByBarrel);

		Debug.Log ("Boss drenched in alcohol");
		bossLog.isIgnitable = true;

		yield return new WaitUntil (() => bossLog.onFire);

		Debug.Log ("Boss Ignited - qP_Mn");

		yield return new WaitUntil (() => bossEliminated || playerEliminated);

		switch (finalEventType) {
		case 1:
			Debug.Log ("Boss defeated");
			break;
		case 2:
			Debug.Log ("Quest failed");
			break;
		}

		//**TEMPORARY**
		bool Continue = false;
		yield return new WaitUntil (() => Continue);
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	private IEnumerator firstMnQ(Action<bool> Accomplished){
		Debug.Log ("MnQuest #1 Initiated");

		//Detects when player enters cellar via doorBehaviour.cs message broadcast
		yield return new WaitUntil (() => inCellar);

		Debug.Log ("Entered cellar -ManagerAI");
		//Activates blackout screen
		ftTimer.SetActive(true);
		cellarLight.areLightsOn = true;
		cellarLight.BackgroundLightIntensity = 0;

		//Detects when player acquires wine keg
		Debug.Log("Gate reached");
		reachedObjectiveI = false;
		yield return new WaitUntil (() => reachedObjectiveI);

		//Unlocks cellar exit
		Debug.Log("Objective Reached");
		Messenger<bool, int>.Broadcast ("(un)lockCellar", true, 2);
		//Detects when player leaves cellar via doorBehaviour.cs message broadcast
		yield return new WaitUntil (() => !inCellar);

		//Deactivates blackout screen
		//ftTimer.SetActive(false);
		cellarLight.areLightsOn = false;
		//Dropping held items:
		//Dropping carried barrel
		GameObject qObjective = movementRef.carriedObject;
		Sprite[] objOrientation = movementRef.ObjSprites;
		qObjective.GetComponent<SpriteRenderer> ().sprite = objOrientation [2];
		qObjective.transform.position = new Vector3 (5, 1, 0);
		qObjective.transform.parent = null;
		movementRef.isCarryingObj = false;
		//Dropping held torch
		GameObject torchRef = movementRef.Torch.gameObject;
		torchRef.transform.position = new Vector3 (5, 2, 0);
		torchRef.transform.parent = null;
		movementRef.isCarryingTorch = false;

		barrelBehaviour brlComponent = qObjective.AddComponent<barrelBehaviour> ();
		brlComponent.isInteractable = true;

		Debug.Log ("Quest Completed; Left the cellar");
		qStatus.qPhase [2] = 2;

		yield return new WaitForSeconds (2.0f);
	
		Accomplished (true);
		yield break;
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}