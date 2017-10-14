using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AttackHandler(object sender, CombatEventArgs e);
public delegate void BlockHandler(object sender, CombatEventArgs e);

public class PlayerCombat : MonoBehaviour
{
    public float health;
	//Set to 4
    public float damageValue;
	public float BlkRecoil;
	public float blockLimit;
    RaycastHit2D hitInfo;
	private bool isBlocking = false;
	private bool canBlock = true;
	private float blockDuration;
	private float blockCooldown;
	private PlayerController directionRef;

    public event AttackHandler PlayerAttacked;
	public event AttackHandler DmgReceiver_UsrSide;
	public event BlockHandler BlkReceiver;

    // Use this for initialization
    void Awake()
    {
        //FindObjectOfType<EnemyCombat>().EnemyAttacked += HandleEnemyAttack;
		FindObjectOfType<npcCombat>().DmgReceiver_NPCSide += HandleEnemyAttack;

		directionRef = GetComponent<PlayerController> ();
        Physics2D.queriesStartInColliders = false; 
    }

    // Check if the player hit an enemy and give damage accordingly every frame 
    void Update()
    {
		/*
        if (PlayerAttacked != null && Input.GetButtonDown("Fire1"))
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(x, y).normalized;

            if (Math.Abs(x) > 0 || Math.Abs(y) > 0) 
            {
                hitInfo = Physics2D.Raycast(transform.position, direction, 0.5f);
            }


            if (hitInfo.collider != null)
            {
                PlayerAttacked(this, new CombatEventArgs(damageValue));
                Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + direction * 0.5f, Color.green);
            }
        }*/
		if (DmgReceiver_UsrSide != null) {
			if (Input.GetMouseButtonDown (0) && !isBlocking) {
				Vector3 direction = directionRef.Orientation;
				hitInfo = Physics2D.Raycast (transform.position, direction, 0.85f);
				if (hitInfo.collider != null) {
					GameObject objectHit = hitInfo.collider.gameObject;
					if (objectHit.GetComponent<npcCombat> () != null) {
						Debug.Log ("Attacked NPC");
						DmgReceiver_UsrSide (this, new CombatEventArgs (damageValue));
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
				//Voluntary block cooldown
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
    }

    void HandleEnemyAttack(object sender, CombatEventArgs e)
    {
		if (!isBlocking) {
			health -= e.Dmg;
			Debug.Log (health);
		} else {
			BlkReceiver (this, new CombatEventArgs (BlkRecoil));
			Debug.Log ("Attack blocked");
		}
    }
}
