using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AttackHandler(object sender, CombatEventArgs e);
public delegate void BlockHandler(object sender, CombatEventArgs e);

public class PlayerCombat : MonoBehaviour
{
	//Weapon dependent variables (modifiers)
	public float damageValue;
	public float BlkRecoil;
	public float blockLimit;
	public float knockbackValue;
	public float dmgPlseLength;

	private bool holdingTorch;
	public float flameDamage;

	private Texture2D fadeTexture;
	public float setHealth;
	public float regenAmt;
	public float regenRate;
    public float varHealth;
	public bool suspendRegen;
	//Set to 4
    RaycastHit2D hitInfo;
	private bool isBlocking = false;
	private bool canBlock = true;
	private float blockDuration;
	private float blockCooldown;
	private float regenTime;
	private Rigidbody2D rbody;
	private PlayerController directionRef;
	private Camera mainCamera;

	private bool dealKnockback = false;
	private Vector2 kbForce;
	private Vector2 P1;
	private Vector2 P2;
	private float deltaP;

    public event AttackHandler PlayerAttacked;
	public event AttackHandler DmgReceiver_UsrSide;
	public event BlockHandler BlkReceiver;

    // Use this for initialization
    void Awake()
    {
		fadeTexture = new Texture2D (2, 2, TextureFormat.ARGB32, false);
		fadeTexture.SetPixel (0, 0, Color.clear);
		fadeTexture.SetPixel (1, 0, Color.clear);
		fadeTexture.SetPixel (0, 1, Color.clear);
		fadeTexture.SetPixel (1, 1, Color.clear);
		fadeTexture.Apply ();

		varHealth = setHealth;

		rbody = GetComponent<Rigidbody2D> ();
		directionRef = GetComponent<PlayerController> ();
		mainCamera = Camera.main;
		Physics2D.queriesStartInColliders = false; 
    }

    // Check if the player hit an enemy and give damage accordingly every frame 
    void Update()
    {
		if (DmgReceiver_UsrSide != null) {
			if (Input.GetMouseButtonDown (0) && !isBlocking && !directionRef.isCarryingObj) {
				Vector3 direction = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
				hitInfo = Physics2D.Raycast (transform.position, direction, 1);
				if (hitInfo.collider != null) {
					GameObject objectHit = hitInfo.collider.gameObject;
					if (objectHit.GetComponent<npcCombat> () != null) {
						Debug.Log ("Attacked NPC");
						holdingTorch = directionRef.isCarryingTorch;
						DmgReceiver_UsrSide (this, new CombatEventArgs (damageValue, Vector3.zero, 0, holdingTorch, flameDamage));
					}
				}
			}
		}

		if (canBlock) {
			if (Input.GetKey (KeyCode.Q)) {
				//Ran once
				if (!isBlocking) {
					isBlocking = true;
					blockDuration = 0;
					Debug.Log("Now blocking");
					Messenger<bool>.Broadcast ("canMove_Update", false);
				}
				blockDuration += Time.deltaTime;
			} else if (!Input.GetKey (KeyCode.Q)) {
				//Ran once
				if (isBlocking) {
					isBlocking = false;
					Debug.Log ("Released block");
					Messenger<bool>.Broadcast ("canMove_Update", true);
				}
				//Voluntary block release
				if (blockDuration > 0) {
					blockDuration -= Time.deltaTime * 2;
				}
			}
			//Forces block cooldown if blockDuration exceeds certain time
			if (blockDuration >= blockLimit) {
				Messenger<bool>.Broadcast ("canMove_Update", true);
				canBlock = false;
				isBlocking = false;
				blockCooldown = blockLimit / 2;
				Debug.Log ("Forced block cooldown; reached block limit");
			}
		} else {
			blockCooldown -= Time.deltaTime;
			if (blockCooldown <= 0) {
				canBlock = true;
				Debug.Log ("Forced block cooldown expired");
			}
		}

		if (varHealth < setHealth && !suspendRegen) {
			regenTime += Time.deltaTime;
			if (regenTime >= regenRate) {
				float detractedHealth = setHealth - varHealth;
				if (detractedHealth >= regenAmt) {
					varHealth += regenAmt;
				} else {
					varHealth += detractedHealth;
				}
				regenTime = 0;
			}
		}

		if (varHealth <= 0) {
			suspendRegen = true;
			if (!dealKnockback) {
				//DO SOMETHING
				StartCoroutine(KORoutine());
			}
		}
    }

    public void HandleEnemyAttack(object sender, CombatEventArgs e)
    {
		int enemyType = e.CombatLvl;
		if (enemyType == 1) {
			if (isBlocking) {
				BlkReceiver (this, new CombatEventArgs (BlkRecoil, Vector3.zero, 0, false, 0));
				Debug.Log ("Attack blocked");
			}
		} else if (enemyType == 2) {
			if (isBlocking) {
				Debug.Log ("Block negated");
				varHealth -= e.Dmg;
				Debug.Log (varHealth);

				StartCoroutine (damageEffect ());

				kbForce = 1.5f * e.Force;
				dealKnockback = true;
				P1 = transform.position;
				P2 = P1 + kbForce;

				deltaP = Vector2.Distance (P2, P1);
				Debug.Log (kbForce);
			}
		}
		if (!isBlocking) {
			varHealth -= e.Dmg;
			Debug.Log (varHealth);

			StartCoroutine (damageEffect ());

			kbForce = e.Force;
			dealKnockback = true;
			P1 = transform.position;
			P2 = P1 + kbForce;

			deltaP = Vector2.Distance (P2, P1);
			Debug.Log (kbForce);
		}
    }

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnGUI(){
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}
	void FixedUpdate(){
		if (dealKnockback) {
			//Debug.Log ("Applying force");
			rbody.MovePosition (rbody.position + 10 * (kbForce * Time.deltaTime));

			float deltaTraveled = Vector3.Distance (rbody.position, P1);
			if (deltaTraveled >= deltaP) {
				dealKnockback = false;
			}
		}
	}

	//COROUTINES:
	private IEnumerator damageEffect(){
		//Briefly turns the screen red
		Color rTint = new Color (1, 0, 0, 0.25f);
		for (int i = 0; i <= 1; i++) {
			for (int j = 0; j <= 1; j++) {
				fadeTexture.SetPixel (i, j, rTint);
			}
		}
		fadeTexture.Apply ();
		yield return new WaitForSeconds (dmgPlseLength);

		if (varHealth > 0) {
			for (int i = 0; i <= 1; i++) {
				for (int j = 0; j <= 1; j++) {
					fadeTexture.SetPixel (i, j, Color.clear);
				}
			}
			fadeTexture.Apply ();
			yield return null;
		} else {
			yield return null;
		}
	}
	private IEnumerator KORoutine(){
		//Notify npcCombat of KO; suspending further attacks
		Messenger<bool>.Broadcast ("PlayerKO", true);

		//Screen fade:
		for (int i = 0; i <= 1; i++) {
			for (int j = 0; j <= 1; j++) {
				fadeTexture.SetPixel (i, j, Color.black);
			}
		}
		fadeTexture.Apply ();
		yield return new WaitForSeconds (2.0f);
		for (int i = 0; i <= 1; i++) {
			for (int j = 0; j <= 1; j++) {
				fadeTexture.SetPixel (i, j, Color.clear);
			}
		}
		fadeTexture.Apply ();

		//Reviving player
		varHealth = setHealth;
		suspendRegen = false;
		//Resuming npcCombat attacks
		Messenger<bool>.Broadcast ("PlayerKO", false);

		yield break;
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
