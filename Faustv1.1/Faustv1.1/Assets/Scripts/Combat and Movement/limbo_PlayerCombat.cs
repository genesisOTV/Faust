using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AttackedHandler(object sender, CombatEventArgs e);

public class limbo_PlayerCombat : MonoBehaviour
{
    public float health;
    public float damageValue;
    RaycastHit2D hitInfo;
    Vector3 Orientation;

    public event AttackedHandler PlayerAttacked;

    void Start()
    {
        FindObjectOfType<EnemyCombat>().EnemyAttacked += HandleEnemyAttacked; 
        Physics2D.queriesStartInColliders = false; 
    }

    // Check if the player hit an enemy and give damage accordingly every frame 
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX > 0)
        {
            Orientation = Vector3.right;
        }
        else if (inputX < 0)
        {
            Orientation = -Vector3.right;
        }
        else if (inputY > 0)
        {
            Orientation = Vector3.up;
        }
        else if (inputY < 0)
        {
            Orientation = -Vector3.up;
        }

        if (PlayerAttacked != null && Input.GetButtonDown("Fire1"))
        { 
            hitInfo = Physics2D.Raycast(transform.position, Orientation, 0.5f);

            if (hitInfo.collider != null)
            {
				PlayerAttacked(this, new CombatEventArgs(damageValue, Vector3.zero, 0, false, 0));
                Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + Orientation * 0.5f, Color.white);
            }
        }
    }

    void HandleEnemyAttacked(object sender, CombatEventArgs e)
    {
        health -= e.Dmg;
    }
}
