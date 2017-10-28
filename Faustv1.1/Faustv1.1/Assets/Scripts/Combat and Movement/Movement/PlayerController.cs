using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static float moveSpeed = 4;
    public Sprite[] ObjSprites;
	public spriteHolder objOrientation;
    public float carryingPos_Default;
    public bool isCarryingObj;
    public GameObject carriedObject;
    private Sprite objSpriteHolder;
	public bool isCarryingTorch;
	public torchBehaviour Torch;

    public Vector3 Orientation;
	private Vector2 offsetVector;
	private Vector2 initialPos;
	private Vector2 Destination;
	public bool completedMove = false;
	private bool movementException = false;
	private float offsetDist;
	private float distRatio;

    private Animator anim;
    private bool canMove;
    private bool playerMoving;
	private bool shouldAdjust;

    private Rigidbody2D rbody;
    Vector2 velocity;

    private float timeDownX = 0.0f; // time which horizontal was pressed
    private float timeDownY = 0.0f; // time which vertical was pressed

    //Singleton pattern
    static PlayerController Instance; 

    void Awake()
    {
        //Checks if the instaniated object is a copy
        if (Instance != null)
        {
            //Commit suicide if is it a copy
            Destroy(this.gameObject);
            return; 
        }
        //Instance is set to be the singelton if not a copy
        Instance = this; 
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
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


	void Update(){
		if (movementException) {
			if (Vector2.Distance(initialPos, rbody.position) >= offsetDist) {
				completedMove = true;
				movementException = false;
			}
		}
	}
    void FixedUpdate()
    {
		//Get Input
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		if (canMove) {
            //if key is pressed on Horizontal save time it was pressed
            if (inputX != 0)
            {
                if (timeDownX == 0.0f) //if there is no stored time for it
                    timeDownX = Time.time;
            }
            else
            {
                timeDownX = 0.0f; // reset time if no button is being pressed
            }

            //if key is pressed on vertical save time it was pressed
            if (inputY != 0)
            {
                if (timeDownY == 0.0f) //if there is no stored time for it
                    timeDownY = Time.time;
            }
            else
            {
                timeDownY = 0.0f; // reset time if no button is being pressed
            }
           
            //check which button was hit last to determin direction
            velocity = Vector2.zero;
            if (timeDownX > timeDownY)
            {
                velocity = Vector2.right * inputX;
            }
            if (timeDownX < timeDownY)
            {
                velocity = Vector2.up * inputY;
            }

            //velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;

            //Orientation
			int Quadrant = -1;
            if (inputX > 0) {
				Orientation = Vector3.right;
				objSpriteHolder = ObjSprites[0];
				Quadrant = 2;
			} else if (inputX < 0) {
				Orientation = -Vector3.right;
                objSpriteHolder = ObjSprites[1];
				Quadrant = 4;
			} else if (inputY > 0) {
				Orientation = Vector3.up;
                objSpriteHolder = ObjSprites[2];
				Quadrant = 1;
			} else if (inputY < 0) {
				Orientation = -Vector3.up;
                objSpriteHolder = ObjSprites[3];
				Quadrant = 3;
			}

            // Animations
            playerMoving = (Mathf.Abs(inputX) + Mathf.Abs(inputY)) > 0;
            anim.SetBool("PlayerMoving", playerMoving);
            if (Mathf.Abs(inputX) > 0 && Mathf.Abs(inputY) > 0)
            {
                velocity = Vector2.zero;
                anim.SetBool("PlayerMoving", false);
                playerMoving = false;
            }

            if (playerMoving)
            {
                anim.SetFloat("MoveX", inputX);
                anim.SetFloat("MoveY", inputY);
				if(!shouldAdjust){
					shouldAdjust = true;
				}
				if (objOrientation != null) {
					objOrientation.setPoles (Quadrant);
				}
            }

            // Setting the veloctiy based on input 
			if (movementException) {
				rbody.MovePosition(rbody.position + (offsetVector * (Time.fixedDeltaTime / distRatio)));
			} else{
				rbody.MovePosition(rbody.position + (velocity * moveSpeed) * Time.fixedDeltaTime);
			}

			//Appropriating local offset values of carried objects
			if (velocity != Vector2.zero) {
				//Setting carriedObject offset
				if (isCarryingObj) {
					SpriteRenderer ObjRenderer = carriedObject.GetComponent<SpriteRenderer>();
					ObjRenderer.sprite = objSpriteHolder;

					carriableBehaviour brlCarriable = carriedObject.GetComponent<carriableBehaviour> ();
					Vector3 brlOffset = Orientation * brlCarriable.carriedOffset;
					carriedObject.transform.localPosition = brlOffset;
				}
				//Setting torch offset
				if (isCarryingTorch) {
					carriableBehaviour torchCarriable = Torch.GetComponent<carriableBehaviour> ();
					Vector3 torchOffset = Orientation * torchCarriable.carriedOffset;
					Torch.transform.localPosition = torchOffset;
				}
			}
		}
		
		// [PROGRAMMER] nQ's Script [PROGRAMMER] Start
		//Bit-World Simulation via Positioning Modulo 1/5 units
		if (inputX == 0 && inputY == 0 && shouldAdjust) {
			if (Mathf.Abs(transform.position.x) % 0.20f != 0 || Mathf.Abs(transform.position.y) % 0.20f != 0) {
				//Calling IEnumerator declared below
				StartCoroutine (adjustPos ());
				shouldAdjust = false;
			}
		}
		// [PROGRAMMER] nQ's Script [PROGRAMMER] Finish
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