using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcCombat : MonoBehaviour {
	//[SerializeField] private Sprite flameSprite;
	//Set to 1
	private float hitRange = 1.2f;
	//Set to 20
	private float Health = 20;
	//Set to 4
	private float attackDamage = 4;
	//Set to 4
	public float chaseSpeed = 4;
	//Set to 0.75
	private float CooldownLimit = 1;

	public int brawlTier;
	public bossStatusLog statusRef;

	private int movementState;
	private float Cooldown;
	private bool withinHitRange;
	private bool local_onFire = false;
	private bool burnTimerOn = false;
	private float burnTime = 0;
	private float burnLimit = 7.5f;
	private int punchCombo;
	private int attackTally;
	private float comboSpan;
	private float playerDamage;

	public Sprite flameSprite;
	public GameObject Target;
	public bool isTracking;
	private bool canAttack;

	public event AttackHandler DmgReceiver_NPCSide;

	private Rigidbody2D rbody;
	//Assignment of important variables
	void Awake(){
		Messenger<bool>.AddListener ("PlayerKO", haltAttacks);
		FindObjectOfType<PlayerCombat> ().DmgReceiver_UsrSide += HandlePlayerAttack;
		FindObjectOfType<PlayerCombat> ().BlkReceiver += HandleAttackDeflection;
		rbody = GetComponent<Rigidbody2D> ();

		punchCombo = 0;
		attackTally = 0;
		canAttack = true;
	}
	public void haltAttacks(bool halt){
		canAttack = !halt;
	}
	//Actuates Boss upon first player engagement
	void OnTriggerEnter2D(Collider2D Interactor){
		if (brawlTier == 2) {
			if (!isTracking) {
				if (Interactor.CompareTag ("User")) {
					Messenger.Broadcast ("BossEngaged");
				}
			}
		}
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	public void targetTracker(GameObject opponent){
		if (opponent != null) {
			Target = opponent;
			isTracking = true;
			Debug.Log ("Now tracking: " + Target.gameObject.tag);
		} else {
			isTracking = false;
			Target = new GameObject ();
		}

	}
	void Update(){
		if (canAttack) {
			if (isTracking) {
				Vector3 Direction = (transform.position - Target.transform.position).normalized;
				Vector3 targetPos = new Vector3 ();
				if (Direction.x != 0) {
					targetPos = Target.transform.position + new Vector3 (Mathf.Sign (Direction.x), 0, 0);
				} else if (Direction.y != 0) {
					targetPos = Target.transform.position + new Vector3 (0, Mathf.Sign (Direction.y), 0);
				}
				//Vector3 appliedBuffer = Direction * hitRange;
				Vector3 Distance = targetPos - transform.position;

				Vector2 Velocity = new Vector3 ();
				if (movementState == 1) {
					Velocity = Distance.normalized * chaseSpeed;
				} else if (movementState == 0) {
					Velocity = Distance.normalized * (chaseSpeed * 3/5);
				}

				if (Mathf.Abs (Distance.x) > 0.1f || Mathf.Abs (Distance.y) > 0.1f) {
					rbody.MovePosition (rbody.position + (Velocity * Time.deltaTime));
				}
			}

			if (withinHitRange) {
				if (Cooldown <= 0) {
					StartCoroutine (throwPunch (attackDamage, 0.4f));
				} else {
					Cooldown -= Time.deltaTime;
				}
			}
		}

		if (brawlTier == 1) {
			if (punchCombo == 1) {
				comboSpan += Time.deltaTime;
			} else if (punchCombo == 2) {
				if (comboSpan <= 0.9f) {
					Health -= playerDamage * 2.5f;
					Debug.Log ("Combo");
				} else {
					Health -= playerDamage * 2;
				}
				punchCombo = 0;
				Debug.Log (Health);

				if (Health <= 0) {
					StartCoroutine (deathRoutine ());
				}
			}
		} else if (brawlTier == 2) {
			if (punchCombo == 2) {
				if (attackTally < 2) {
					attackTally++;
					if (attackTally == 1) {
						Debug.Log ("First attack received");
						Messenger.Broadcast ("firstAttack");
					}
				}
			}
		}

		if (statusRef != null && brawlTier == 2) {
			if (statusRef.onFire && !local_onFire) {
				local_onFire = true;
				burnTimerOn = true;
				burnTime = 0;
				StartCoroutine (igniteBoss ());
			}
			if (burnTimerOn) {
				burnTime += Time.deltaTime;
				if (burnTime >= burnLimit) {
					Debug.Log ("Enemy killed; burn limit reached");
					burnTimerOn = false;
					Messenger<int>.Broadcast ("finalEventType", 1);
					Destroy (gameObject);
				}
			}
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnTriggerStay2D(Collider2D Interactor){
		if (Interactor.gameObject.tag == "User") {
			movementState = 0;

			float Distance = Vector2.Distance (Target.transform.position, rbody.position);
			if (Distance <= hitRange) {
				if (!withinHitRange) {
					withinHitRange = true;
				}
			} else {
				if (withinHitRange) {
					withinHitRange = false;
				}
			}
		}
	}
	void OnTriggerExit2D(Collider2D Interactor){
		if (Interactor.gameObject.CompareTag(Target.gameObject.tag)) {
			movementState = 1;

			withinHitRange = false;
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish

	//COMBAT HANDLERS:
	void HandlePlayerAttack(object sender, CombatEventArgs e){
		if (e.TorchMod) {
			if (brawlTier == 1) {
				playerDamage += e.TorchDmg;
			} else if (brawlTier == 2) {
				playerDamage = statusRef.torchAtkHandler (e.TorchDmg);
				Health -= playerDamage;
			}
		} else {
			punchCombo++;
			if (brawlTier == 1) {
				playerDamage = e.Dmg;
			} else if (brawlTier == 2) {
				StartCoroutine (throwPunch (2, 0.2f));
			}
		}
	}
	void HandleAttackDeflection(object sender, CombatEventArgs e){
		Cooldown += e.Dmg;
	}

	//COROUTINES:
	private IEnumerator throwPunch(float Dmg, float forceMltplr){
		Vector2 Force = Target.transform.position - transform.position;
		Vector3 appliedForce = forceMltplr * Force.normalized;
		//Sends punch info
		DmgReceiver_NPCSide (this.gameObject, new CombatEventArgs (Dmg, appliedForce, brawlTier, local_onFire, 0));
		//**ADD ANIMATION**
		Cooldown = CooldownLimit;

		yield return null;
	}

	private IEnumerator deathRoutine(){
		Debug.Log ("NPC killed");
		gameObject.SetActive (false);
		//Add body
		Messenger.Broadcast ("brawlerDefeated");

		yield return null;
	}

	private IEnumerator igniteBoss(){
		GameObject Flame = new GameObject ();
		Flame.transform.SetParent (transform);
		Flame.transform.localPosition = Vector3.zero;
		Flame.transform.localScale = new Vector3 (1, 1, 0);

		SpriteRenderer spriteHolder = Flame.AddComponent<SpriteRenderer> ();
		flameSprite = Resources.Load ("Sprites/faustTorch", typeof(Sprite)) as Sprite;
		spriteHolder.sprite = flameSprite;
		Debug.Log ("Boss ignited");
		yield break;
	}
}
