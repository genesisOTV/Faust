using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chantRefs : MonoBehaviour {
	public string[] chant0;
	public string[] chant1;
	public string[] chant2;

	void Awake(){
		chant0 = new string[]{ 
			"He",
			"doted",
			"fondly",
			"on",
			"that thing",
			"with",
			"fatherly",
			"affection"
		};
		chant1 = new string[] {
			"The",
			"Queen",
			"and",
			"her",
			"lady's-maid",
			"though",
			"bitten",
			"till",
			"delirious",
			"forbore",
			"to",
			"squash",
			"the fleas"
		};
		chant2 = new string[] {
			"We",
			"free souls",
			"squash",
			"all",
			"fleas",
			"the instant",
			"they",
			"light",
			"on us"
		};
	}
}
