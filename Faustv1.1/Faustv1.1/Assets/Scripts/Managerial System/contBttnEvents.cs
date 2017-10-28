using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contBttnEvents : MonoBehaviour {

	public void contButtonHandler(int choice){
		//Debug.Log ("Button Pressed");
		Messenger<int>.Broadcast ("contBtnRtrn", choice);
	}
}
