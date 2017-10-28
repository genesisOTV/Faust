using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEventArgs : EventArgs
{
	public CombatEventArgs(float dmg, Vector3 force, int combatLvl, bool torchMod, float torchDmg)
    {
        this.Dmg = dmg; 
		this.Force = force;
		this.CombatLvl = combatLvl;
		this.TorchMod = torchMod;
		this.TorchDmg = torchDmg;
    }

    public float Dmg { get; set; } 
	public Vector3 Force{ get; set; }
	public int CombatLvl{ get; set; }
	public bool TorchMod{ get; set; }
	public float TorchDmg{ get; set;}
}
