using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make camera follow player  

public class CameraController : MonoBehaviour {
	//[PROGRAMMER] Gene [PROGRAMMER] start
	[SerializeField] private GameObject followTarget;
	private Vector3 targetPos;
	public float moveSpeed;

    //Singleton pattern
    static CameraController Instance;

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

    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		targetPos = new Vector3 (followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
		//Linearlly interpolates two vectors //delete this later
		transform.position = Vector3.Lerp(transform.position/*where the camera is*/, targetPos/*where we want it to be*/, moveSpeed * Time.deltaTime/*the movement amount*/);
	}
	//[PROGRAMMER] Gene [PROGRAMMER] end
}
