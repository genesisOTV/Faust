using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Sprite[] ObjSprites;
    public float moveSpeed;
	public float carryingPos_Default;
	public bool isCarryingObj;
	public GameObject carriedObject;
	public bool isCarryingTorch;
	public torchBehaviour Torch;

	public Vector3 Orientation;
    //public double unitSize;

	public bool completedMove = false;

	private bool movementException = false;
	private float offsetDist;
	private float distRatio;
	private Vector2 Destination;
	private Vector2 initialPos;
	private Vector2 offsetVector;

    private Animator anim;
	private bool canMove;
    private bool playerMoving;
    private Vector2 lastMove;
    private Vector2 direction;
	private Sprite objSpriteHolder;

	private bool shouldAdjust = false;

    private Rigidbody2D rbody;
    Vector2 velocity; 

    // Use this for initialization
    void Start()
    {
		Messenger<bool>.AddListener ("canMove_Update", setCanMove);
		canMove = true;
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
    }
	public void setCanMove(bool canMove_setter){
		canMove = canMove_setter;
	}

	public void movePlayer(Vector2 finalPos){
		Debug.Log ("Ran");
		completedMove = false;
		movementException = true;
		initialPos = rbody.position;
		Destination = finalPos;
		offsetVector = new Vector2 ((Destination.x - rbody.position.x), (Destination.y - rbody.position.y));
		offsetDist = Vector2.Distance (rbody.position, Destination);
		distRatio = offsetDist / moveSpeed;
	}

    void Update()
    {
		if (canMove) {
			velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
		}
		if (movementException) {
			if (Vector2.Distance(initialPos, rbody.position) >= offsetDist) {
				completedMove = true;
				movementException = false;
			}
		}
    }

    void FixedUpdate()
    {
		#region Get Input
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		#endregion
		if (canMove) {
			if (inputX > 0) {
				Orientation = Vector3.right;
				objSpriteHolder = ObjSprites[0];
			} else if (inputX < 0) {
				Orientation = -Vector3.right;
				objSpriteHolder = ObjSprites[1];
			} else if (inputY > 0) {
				Orientation = Vector3.up;
				objSpriteHolder = ObjSprites[2];
			} else if (inputY < 0) {
				Orientation = -Vector3.up;
				objSpriteHolder = ObjSprites[3];
			}
			#region Animations
			bool playerMoving = (Mathf.Abs(inputX) + Mathf.Abs(inputY)) > 0;
			anim.SetBool("PlayerMoving", playerMoving);
			if (playerMoving)
			{
				anim.SetFloat("MoveX", inputX);
				anim.SetFloat("MoveY", inputY);
				if(!shouldAdjust){
					shouldAdjust = true;
				}
			}
			#endregion
			#region Setting the veloctiy based on input 
			if (movementException) {
				rbody.MovePosition(rbody.position + (offsetVector * (Time.fixedDeltaTime / distRatio)));
			} else{
				rbody.MovePosition(rbody.position + velocity * Time.fixedDeltaTime);
			}

			if(isCarryingObj && velocity != Vector2.zero){
				SpriteRenderer ObjRenderer = carriedObject.GetComponent<SpriteRenderer>();
				ObjRenderer.sprite = objSpriteHolder;
				
				Vector3 carryingPos_Applied = -(carryingPos_Default * Orientation);
				carriedObject.transform.position = transform.position + carryingPos_Applied;
			}
		}
		#endregion

		#region Unit grid simulation
		// [PROGRAMMER] Script Intercept (Beginning): 
		//Bit-World Simulation via Positioning Modulo 1/5 units
		if (inputX == 0 && inputY == 0 && shouldAdjust) {
			if (Mathf.Abs(transform.position.x) % 0.20f != 0 || Mathf.Abs(transform.position.y) % 0.20f != 0) {
				//Calling IEnumerator declared below
				StartCoroutine (adjustPos ());
				shouldAdjust = false;
			}
		}
		// [PROGRAMMER] Script Intercept (Ending).
		#endregion
    }

    #region Coroutine 
    //[PROGRAMMER] nQ's Script [PROGRAMMER] Start
    private IEnumerator adjustPos()
    {
        //Managing X position
		float Excess_x = transform.position.x % 0.20f;
		float Excess_y = transform.position.y % 0.20f;
		//Accounting for rounding errors
		if (Mathf.Abs (Excess_x) >= 0.195f && Mathf.Abs (Excess_x) <= 0.205f) {
			Excess_x = 0;
		}
		if (Mathf.Abs (Excess_y) >= 0.195f && Mathf.Abs (Excess_y) <= 0.205f) {
			Excess_y = 0;
		}
		float Delta_x = 0;
		float Delta_y = 0;
		if (Excess_x < -0.001f) {
			Delta_x = -Excess_x;
		} else if (Excess_x > 0.001f) {
			Delta_x = 0.20f - Excess_x;
		}
		if (Excess_y < -0.001f) {
			Delta_y = -Excess_y;
		} else if (Excess_y > 0.001f) {
			Delta_y = 0.20f - Excess_y;
		}

		Vector3 Delta_Applied = new Vector3 (Delta_x, Delta_y, 0);
		transform.position = transform.position + Delta_Applied;
		if (isCarryingObj) {
			carriedObject.transform.position += Delta_Applied;
		}
        yield return null;
    }
    //[PROGRAMMER] nQ's Script [PROGRAMMER] Finish 
    #endregion
}


