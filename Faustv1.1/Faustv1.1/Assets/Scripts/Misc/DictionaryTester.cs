using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//INACTIVE SCRIPT
public class DictionaryTester : MonoBehaviour {

	public Dictionary<string, string> sentences_Tester = new Dictionary<string, string> ();

	public string[] testTags;
	public string[] testSentences;

	void Start(){
		for(int i = 0; i < testTags.Length; i++){
			string indexedKey = testTags [i];
			string indexedElem = testSentences [i];
			sentences_Tester.Add (indexedKey, indexedElem);
		}
		//Debug.Log ("Script Ran");
		//Debug.Log (sentences_Tester ["#1"]);
		//Debug.Log (sentences_Tester ["#2"]);
	}

}
