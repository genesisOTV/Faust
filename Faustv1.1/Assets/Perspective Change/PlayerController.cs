using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public Sprite[] ObjSprites;
    //public float carryingPos_Default;
    //public bool isCarryingObj;
    //public GameObject carriedObject;
    //private Sprite objSpriteHolder;

    public Vector3 Orientation;
    //public double unitSize;

    //private Animator anim;
    private bool canMove;
    private bool playerMoving;

    private Rigidbody rbody;
    Vector3 velocity;
    public static float moveSpeed = 4;

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
		//Messenger<bool>.AddListener ("canMove_Update", setCanMove);
		canMove = true;
        //anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
    }

    public void setCanMove(bool canMove_setter){
		canMove = canMove_setter;
	}

    void FixedUpdate()
    {
		if (canMove) {
			//Get Input
			float inputX = Input.GetAxisRaw("Horizontal");
			float inputY = Input.GetAxisRaw("Vertical");

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
            velocity = Vector3.zero;
            if (timeDownX > timeDownY)
            {
                velocity = Vector3.right * inputX;
            }
            if (timeDownX < timeDownY)
            {
                velocity = Vector3.forward * inputY;
            }

            //velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;

            //Orientation
            if (inputX > 0) {
				Orientation = Vector3.right;
				//objSpriteHolder = ObjSprites[0];
			} else if (inputX < 0) {
				Orientation = -Vector3.right;
                //objSpriteHolder = ObjSprites[1];
			} else if (inputY > 0) {
				Orientation = Vector3.up;
                //objSpriteHolder = ObjSprites[2];
			} else if (inputY < 0) {
				Orientation = -Vector3.up;
                //objSpriteHolder = ObjSprites[3];
			}

            // Animations
            playerMoving = (Mathf.Abs(inputX) + Mathf.Abs(inputY)) > 0;
            //anim.SetBool("PlayerMoving", playerMoving);
            if (Mathf.Abs(inputX) > 0 && Mathf.Abs(inputY) > 0)
            {
                velocity = Vector3.zero;
                //anim.SetBool("PlayerMoving", false);
                playerMoving = false;
            }

            if (playerMoving)
            {
                //anim.SetFloat("MoveX", inputX);
                //anim.SetFloat("MoveY", inputY);
            }

            // Setting the veloctiy based on input 
            rbody.MovePosition(rbody.position + velocity * Time.fixedDeltaTime * moveSpeed);
            //transform.Translate(velocity * Time.deltaTime * moveSpeed);

            //if (isCarryingObj && velocity != Vector2.zero){
			//	SpriteRenderer ObjRenderer = carriedObject.GetComponent<SpriteRenderer>();
			//	ObjRenderer.sprite = objSpriteHolder;
				
			//	Vector3 carryingPos_Applied = -(carryingPos_Default * Orientation);
			//	carriedObject.transform.position = transform.position + carryingPos_Applied;
			//}
		}

        #region Commented Out
        //Bit-World Simulation via Positioning Modulo 1/4 units
        //if (playerMoving == false)
        //{
        //    if (transform.position.x % 0.25 != 0 || transform.position.y % 0.25 != 0)
        //    {
        //        //Calling IEnumerator declared below
        //        StartCoroutine(adjustPos());
        //    }
        //}
        //  [PROGRAMMER] Script Intercept (Ending).
        #endregion
    }


    #region Coroutine 
    //[PROGRAMMER] nQ's Script [PROGRAMMER] Start
    private IEnumerator adjustPos()
    {
        //Managing X position
        float Excess_x = transform.position.x % (float)0.25;
        if (Excess_x < 0.10)
        {
            Vector3 Epsilon = new Vector3(Excess_x, 0, 0);
            transform.position = transform.position - Epsilon;
        }
        else
        {
            Vector3 Epsilon = new Vector3((float)0.25 - Excess_x, 0, 0);
            transform.position = transform.position + Epsilon;
        }
        //Managing Y position
        float Excess_y = transform.position.y % (float)0.25;
        if (Excess_y < 0.10)
        {
            Vector3 Epsilon = new Vector3(0, Excess_y, 0);
            transform.position = transform.position - Epsilon;
        }
        else
        {
            Vector3 Epsilon = new Vector3(0, (float)0.25 - Excess_y, 0);
            transform.position = transform.position + Epsilon;
        }
        yield return null;
    }
    //[PROGRAMMER] nQ's Script [PROGRAMMER] Finish 
    #endregion
}