using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brlProjectile : MonoBehaviour {
	[SerializeField] private GameObject parentBarrel;
	[SerializeField] private GameObject shatterPrefab;
	public Vector3 vVectorRef;
	public float shatterAngle;
	public float F;
	private bool Collided = false;

	//[PROGRAMMER] nQ's Script [PROGRAMMER] Start
	void OnTriggerStay2D(Collider2D Target){
		//Detects collision with Boss gameObject
		if (Target.CompareTag ("Boss")) {
			GameObject Enemy = Target.gameObject;

			float distance = Vector2.Distance(Enemy.transform.position, transform.position);
			if (distance <= 0.2f && !Collided) {
				Debug.Log ("Boss hit");
				Collided = true;

				StartCoroutine (affectBoss (Enemy));
				//Disabling barrel sprite
				GetComponent<SpriteRenderer> ().enabled = false;
				//Add particle effect
				Vector3 shatterVector_I = Quaternion.Euler (0, 0, -70) * vVectorRef;
				Vector3 shatterVector_II = Quaternion.Euler (0, 0, 70) * vVectorRef;
				Vector3[] shatterVectors = new Vector3[]{ shatterVector_I.normalized, shatterVector_II.normalized };

				StartCoroutine (createHalves (shatterVectors));
			}
		}
	}

	//COROUTINES:
	private IEnumerator affectBoss(GameObject Boss){
		//Setting Boss variables
		Boss.GetComponent<bossStatusLog> ().hitByBarrel = true;
		Boss.GetComponent<Rigidbody2D> ().AddForce (vVectorRef * 1.2f, ForceMode2D.Impulse);

		yield return null;
	}
	private IEnumerator createHalves(Vector3[] Angles){
		Debug.Log ("Initiated");
		//Instantiating two gameObjects as barrel halves
		GameObject leftHalf = GameObject.Instantiate (shatterPrefab, this.transform);
		GameObject rightHalf = GameObject.Instantiate (shatterPrefab, this.transform);
		//Setting respective sprites of halves
		spriteHolder brlSpriteRef = GetComponent<spriteHolder> ();
		leftHalf.GetComponent<SpriteRenderer> ().sprite = brlSpriteRef.lSprite;
		rightHalf.GetComponent<SpriteRenderer> ().sprite = brlSpriteRef.rSprite;

		//Applying force to halves
		Rigidbody2D lRbody = leftHalf.GetComponent<Rigidbody2D> ();
		Rigidbody2D rRbody = rightHalf.GetComponent<Rigidbody2D> ();
		lRbody.position = transform.position;
		rRbody.position = transform.position;
		lRbody.AddForce (Angles [0] * F, ForceMode2D.Impulse);
		rRbody.AddForce (Angles [1] * F, ForceMode2D.Impulse);

		yield return new WaitForSeconds (1.15f);

		GameObject.Destroy (this.gameObject);
	}
	//[PROGRAMMER] nQ's Script [PROGRAMMER] Finish


	void Update(){
		//Respawns default barrel should the player miss (barrel 'hits' the ground)
		Rigidbody2D rbody = this.GetComponent<Rigidbody2D> ();
		if (rbody.velocity.magnitude <= 0.25f) {
			Debug.Log ("Hit the ground");
			GameObject barrel = GameObject.Instantiate (parentBarrel);
			barrel.transform.position = transform.position;

			GameObject.Destroy (gameObject);
		}
	}
}
