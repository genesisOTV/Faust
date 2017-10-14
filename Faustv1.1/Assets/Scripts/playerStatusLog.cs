using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatusLog : MonoBehaviour {
	public int onQuest;
	public List<int> qPhase;

	public void qPhaseSet(List<int> sceneStages){
		//0 For not started; 1 for started 1st phase; 2 for started 2nd stage; etc.
		qPhase = new List<int> ();
		qPhase = sceneStages;
	}

	void Awake(){
		onQuest = 0;
		
	}
}
