using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AttackedHandler(object sender, CombatEventArgs e);

public class PlayerCombat : MonoBehaviour
{
    public float health;
    public float damageValue;
    RaycastHit2D hitInfo;

    public event AttackedHandler PlayerAttacked;

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<EnemyCombat>().EnemyAttacked += HandleEnemyAttacked;
        Physics2D.queriesStartInColliders = false; 
    }

    // Check if the player hit an enemy and give damage accordingly every frame 
    void Update()
    {
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
        }
    }

    void HandleEnemyAttacked(object sender, CombatEventArgs e)
    {
        health -= e.Dmg;
    }
}
