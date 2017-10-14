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

	public Vector3 Orientation;
    //public double unitSize;

    private Animator anim;
	private bool canMove;
    private bool playerMoving;
    private Vector2 lastMove;
    private Vector2 direction;
	private Sprite objSpriteHolder;

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

    void Update()
    {
		if (canMove) {
			velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
		}
    }

    void FixedUpdate()
    {
		if (canMove) {
			#region Get Input
			float inputX = Input.GetAxisRaw("Horizontal");
			float inputY = Input.GetAxisRaw("Vertical");
			#endregion
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
			}
			#endregion
			#region Setting the veloctiy based on input 
			rbody.MovePosition(rbody.position + velocity * Time.fixedDeltaTime);

			if(isCarryingObj && velocity != Vector2.zero){
				SpriteRenderer ObjRenderer = carriedObject.GetComponent<SpriteRenderer>();
				ObjRenderer.sprite = objSpriteHolder;
				
				Vector3 carryingPos_Applied = -(carryingPos_Default * Orientation);
				carriedObject.transform.position = transform.position + carryingPos_Applied;
			}
		}
			#endregion

			#region Commented Out
			// [PROGRAMMER] Script Intercept (Beginning): 
			/*else {
			//Bit-World Simulation via Positioning Modulo 1/4 units
			if (transform.position.x % 0.25 != 0 || transform.position.y % 0.25 != 0) {
				//Calling IEnumerator declared below
				StartCoroutine (adjustPos ());
			}
		}*/
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


