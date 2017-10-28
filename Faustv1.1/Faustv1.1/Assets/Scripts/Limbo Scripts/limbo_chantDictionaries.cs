using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class limbo_chantDictionaries: MonoBehaviour{
	
	public Dictionary<string, float> chant1Lyrics1;
	public Dictionary<string, float> chant1Lyrics2;
	void Awake(){
		chant1Lyrics1 = new Dictionary<string, float>();
		//INCREASE PAUSE TIME
		chant1Lyrics1.Add ("Once ", 0.45f);
		chant1Lyrics1.Add ("the ", 0.30f);
		chant1Lyrics1.Add ("King ", 0.25f);
		chant1Lyrics1.Add ("had ", 0.25f);
		chant1Lyrics1.Add ("a ", 0.20f);
		chant1Lyrics1.Add ("flea, ", 0.70f);
		chant1Lyrics1.Add ("a flea ", 0.40f);
		chant1Lyrics1.Add ("as ", 0.30f);
		chant1Lyrics1.Add ("big as ", 0.20f);
		chant1Lyrics1.Add ("could be. ", 0.70f);
		chant1Lyrics1.Add ("He ", 0.20f);
		chant1Lyrics1.Add ("doted ", 0.35f);
		chant1Lyrics1.Add ("fondly ", 0.35f);
		chant1Lyrics1.Add ("on ", 0.25f);
		chant1Lyrics1.Add ("that ", 0.15f);
		chant1Lyrics1.Add ("thing, ", 0.70f);
		chant1Lyrics1.Add ("with ", 0.20f);
		chant1Lyrics1.Add ("fatherly ", 0.18f);
		chant1Lyrics1.Add ("affection. ", 0.15f);

		chant1Lyrics2 = new Dictionary<string, float> ();

		chant1Lyrics2.Add ("Glowing satins,", 0.55f);
		chant1Lyrics2.Add ("gleaming silks", 0.60f);
		chant1Lyrics2.Add ("now were,", 0.40f);
		chant1Lyrics2.Add ("the flea's,", 0.40f);
		chant1Lyrics2.Add ("attire.", 0.70f);
		chant1Lyrics2.Add ("In", 0.25f);
		chant1Lyrics2.Add ("sign", 0.35f);
		chant1Lyrics2.Add ("of his", 0.30f);
		chant1Lyrics2.Add ("exalted post,", 0.45f);
		chant1Lyrics2.Add ("as", 0.25f);
		chant1Lyrics2.Add ("the", 0.25f);
		chant1Lyrics2.Add ("King's", 0.25f);
		chant1Lyrics2.Add ("First", 0.25f);
		chant1Lyrics2.Add ("Minister", 0.25f);
	}

}
