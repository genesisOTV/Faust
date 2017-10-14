using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcMovement : MonoBehaviour {

    //[PROGRAMMER] Gene [PROGRAMMER] start

    //All the variables
    public float moveSpeed;

	private Rigidbody2D rbody;

	public bool isWalking; 

	public float walkTime;
	private float walkCounter;

	public float waitTime;
	private float waitCounter;

	private int walkDirection;

    public Collider2D walkZone;
    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkZone; 

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D>();

		waitCounter = waitTime;
		walkCounter = walkTime;

        //Only assigns constrains movement if bounds are set
        if (walkZone != null)
        {
            minWalkPoint = walkZone.bounds.min;
            maxWalkPoint = walkZone.bounds.max;
            hasWalkZone = true; 
        }

		ChooseDirection(); 
	}
	
	// Update is called once per frame
	void Update () {
        // this if else switches between the walking and waiting states
        if (isWalking)
		{
			walkCounter -= Time.deltaTime; 
            //Switch statement for which direction to move depending on random number. Also does a check on the bounds
			switch (walkDirection) 
			{
			case 0:
				rbody.velocity = new Vector2 (0, moveSpeed);
                //Bounds check
                if (hasWalkZone && transform.position.y > maxWalkPoint.y)
                {
                    isWalking = false;
                    waitCounter = waitTime; 
                }
				break;
			case 1:
				rbody.velocity = new Vector2 (moveSpeed, 0);
                //Bounds check
                if (hasWalkZone && transform.position.x > maxWalkPoint.x)
                {
                    isWalking = false;
                    waitCounter = waitTime;
                }
                break;
			case 2:
				rbody.velocity = new Vector2 (0, -moveSpeed);
                //Bounds check
                if (hasWalkZone && transform.position.y < minWalkPoint.y)
                {
                    isWalking = false;
                    waitCounter = waitTime; 
                }
				break;
			case 3:
				rbody.velocity = new Vector2 (-moveSpeed, 0);
                //Bounds check
                if (hasWalkZone && transform.position.x < minWalkPoint.x)
                {
                    isWalking = false;
                    waitCounter = waitTime; 
                }
				break;
			}

            //Switch to waiting state
			if(walkCounter < 0)
			{
				isWalking = false;
				waitCounter = waitTime; 
			}
		}
		else
		{
			waitCounter -= Time.deltaTime;

			rbody.velocity = Vector2.zero;
            
            //Intializes ChooseDirection() which in turn initializes the walking state
			if (waitCounter < 0) 
			{
				ChooseDirection();
			}
		}
	}

	public void ChooseDirection()
	{
		walkDirection = Random.Range (0, 4);
        //Initialize walking state
        isWalking = true;
		walkCounter = walkTime; 
	}
    //[PROGRAMMER] Gene [PROGRAMMER] end
}
