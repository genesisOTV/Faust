using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadExemption : MonoBehaviour {

	//Add to gameObjects needed in consecutive scenes to prevent re-instantiation
	void Awake(){
		DontDestroyOnLoad (this);
	}
}
