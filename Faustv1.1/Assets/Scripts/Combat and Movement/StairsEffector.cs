using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsEffector : MonoBehaviour {

    void OnTriggerStay2D(Collider2D Player)
    {
        if (Player.CompareTag("User"))
        {
			PlayerController.moveSpeed = 2.5f;
        }
    }

    void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.CompareTag("User"))
        {
           PlayerController.moveSpeed = 4;
        }
    }
}
