using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrelBehaviour : MonoBehaviour {
	public GameObject barrel_Projectile;
	private bool isCarried = false;
	public bool isInteractable;
	public Collider2D Mass;
	private carriableBehaviour carriableProp;
	private PlayerController movementRef;
	private Camera mainCamera;
	private Rigidbody2D rb;
	//Assignment important variables
	void Awake(){
		mainCamera = Camera.main;
		rb = GetComponent<Rigidbody2D> ();
		rb.bodyType = RigidbodyType2D.Static;
		carriableProp = GetComponent<carriableBehaviour> ();
	}

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnTriggerStay2D(Collider2D Interactor){
		if (Interactor.CompareTag ("User") && isInteractable) {
			GameObject Player = Interactor.gameObject;
			//Detects when player hoists barrel
			if (Input.GetKeyDown (KeyCode.E) && !isCarried) {
				Debug.Log ("Picked up barrel");
				isCarried = true;
				//Disabling Rigidbody for smooth movement
				Mass.enabled = false;
				Destroy (rb);
				//Setting parent for synced movement
				transform.SetParent (Player.transform);
				movementRef = Player.GetComponent<PlayerController> ();
				transform.localPosition = movementRef.Orientation * carriableProp.carriedOffset;

				//Assigning necessary player properties
				movementRef.carryingPos_Default = -0.5f;
				movementRef.isCarryingObj = true;
				movementRef.carriedObject = this.gameObject;
				movementRef.objOrientation = GetComponent<spriteHolder> ();
			}
		}
	}

	void FixedUpdate(){
		if (isCarried) {
			if (Input.GetMouseButtonDown (0)) {
				//Raycast for thrown barrel
				Vector2 direction = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
				RaycastHit2D hitInfo = Physics2D.Raycast (transform.position, direction, 4.5f);

				if (hitInfo.collider != null) {
					GameObject target = hitInfo.collider.gameObject;
					if (target.CompareTag ("Boss")) {
						Debug.Log ("Barrel thrown");
						//Instantiating projectile barrel
						GameObject ProjectileInst = Instantiate (barrel_Projectile);
						//Setting properties of projectile barrel
						ProjectileInst.transform.position = transform.position;
						ProjectileInst.GetComponent<SpriteRenderer> ().sprite = GetComponent<SpriteRenderer> ().sprite;
						//Setting path direction reference
						brlProjectile prjctlBehaviour = ProjectileInst.GetComponent<brlProjectile> ();
						prjctlBehaviour.vVectorRef = direction.normalized;
						//Deleting this.gameObject / disassociating it from PlayerController instance
						movementRef.isCarryingObj = false;
						GameObject.Destroy (this.gameObject);
						//Applying force to projectile barrel prefab
						Rigidbody2D ProjRB = ProjectileInst.GetComponent<Rigidbody2D> ();
						ProjRB.AddForce (direction.normalized * 6, ForceMode2D.Impulse);
					} else {
						Debug.Log ("Enemy too agile: barrel rendered ineffective.");
					}
				} else {
					Debug.Log ("Do not waste good wine.");
				}
			}
		}
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish
}
