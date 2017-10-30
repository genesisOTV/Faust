using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour {
	// *Script-Wide Variable Declaration*
	public float turnSens;
	private Animator anim;
	private bool negateRot;
	private Vector3 defaultDirection;
	private Vector3 fwdRep;

	void Awake(){
		anim = GetComponent<Animator> ();
		negateRot = false;
		defaultDirection = new Vector3 (0, -1, 0);
		fwdRep = defaultDirection;
	}

	void Update () {
		//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
		if (Input.GetKey (KeyCode.LeftShift)) {
			//Primes animator for player spinning
			anim.SetBool ("CanSpin", true);
			//Counter-Clockwise & Clockwise Rotation
			if (Input.GetKey (KeyCode.RightArrow)) {
				//Angle of virtual vector from origin
				var currentRot = Quaternion.FromToRotation (defaultDirection, fwdRep);
				//Rotation in degrees
				float X_Degrees = currentRot.eulerAngles.x;
				float Y_Degrees = currentRot.eulerAngles.y;
				//Rotating and Interpreting Virtual Vector Position (Counter-Clockwise)
				rotCoutrClkwse(X_Degrees, Y_Degrees);
			} 
			else if (Input.GetKey (KeyCode.LeftArrow)) {
				//Angle of virtual vector from origin
				var currentRot = Quaternion.FromToRotation (defaultDirection, fwdRep);
				//Rotation in degrees
				float X_Degrees = currentRot.eulerAngles.x;
				float Y_Degrees = currentRot.eulerAngles.y;
				//Rotating and Interpreting of Virtual Vector Position (Clockwise)
				rotClkwse (X_Degrees, Y_Degrees);
			}
			//Getting angle from origin for animator
			int virtRot = (int)Vector3.Angle (fwdRep, defaultDirection);
			//Compensating for potentially negated return value
			if (!negateRot) {
				anim.SetInteger ("ModifiedRotation", virtRot);
			} else if (negateRot) {
				int negatedVirtRot = 360 - virtRot;
				anim.SetInteger ("ModifiedRotation", negatedVirtRot);
			}

		} 
		//Returning virtual vector to origin upon LShift release
		else if (Input.GetKeyUp (KeyCode.LeftShift)) {
			fwdRep = defaultDirection;
			negateRot = false;
			anim.SetInteger ("ModifiedRotation", 0);
			anim.SetBool ("CanSpin", false);
		}
	}

	public void rotCoutrClkwse(float X_Degrees, float Y_Degrees){
		var updateRot = Quaternion.Euler (10, 0, 0);
		//Bounding values for accurate interpretation
		if (X_Degrees < 180 && Y_Degrees == 0) {
			fwdRep = updateRot * fwdRep;
			negateRot = false;
		} else if (X_Degrees > 0 && X_Degrees < 90 && Y_Degrees == 180) {
			fwdRep = updateRot * fwdRep;
			negateRot = false;
		} else if (X_Degrees == 0 && Y_Degrees == 180 || X_Degrees > 270 && Y_Degrees == 180) {
			Vector3 modedFwd = new Vector3 (-fwdRep.x, fwdRep.y, fwdRep.z);
			fwdRep = updateRot * modedFwd;
			negateRot = true;
		} else if (X_Degrees >= 270 && Y_Degrees == 0) {
			Vector3 modedFwd = new Vector3 (-fwdRep.x, fwdRep.y, fwdRep.z);
			fwdRep = updateRot * modedFwd;
			negateRot = true;
		}
	}

	public void rotClkwse(float X_Degrees, float Y_Degrees){
		var updateRot = Quaternion.Euler (-10, 0, 0);
		if (X_Degrees >= 270 && Y_Degrees == 0) {
			Vector3 modedFwd = new Vector3 (-fwdRep.x, fwdRep.y, fwdRep.z);
			fwdRep = updateRot * modedFwd;
			negateRot = true;
		} else if (X_Degrees > 270 && X_Degrees < 360 && Y_Degrees == 180) {
			Vector3 modedFwd = new Vector3 (-fwdRep.x, fwdRep.y, fwdRep.z);
			fwdRep = updateRot * modedFwd;
			negateRot = true;
		} else if (X_Degrees == 360 && Y_Degrees == 180 || X_Degrees < 90 && Y_Degrees == 180) {
			fwdRep = updateRot * fwdRep;
			negateRot = false;
		} else if (X_Degrees <= 90 && Y_Degrees == 0) {
			fwdRep = updateRot * fwdRep;
			negateRot = false;
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
