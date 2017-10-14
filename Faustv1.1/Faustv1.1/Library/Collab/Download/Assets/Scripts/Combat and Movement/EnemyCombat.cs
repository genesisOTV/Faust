using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {

    public float health;
    public float damageValue;

    public event AttackedHandler EnemyAttacked;

    // Use this for initialization
	void Start () {
       FindObjectOfType<PlayerCombat>().PlayerAttacked += HandlePlayerAttacked;
	}
	
	// Update is called once per frame
	void Update () {
        if(health == 0)
        {
            gameObject.SetActive(false);
            FindObjectOfType<PlayerCombat>().PlayerAttacked -= HandlePlayerAttacked;
        }
		
	}

    void HandlePlayerAttacked(object sender, CombatEventArgs e)
    {
        health -= e.Dmg; 
    }
}
