using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {

    Empire empire;
    MovementController movementController;
    [SerializeField] int attackValue = 0;
    [SerializeField] int defenceValue = 0;

    public void SetEmpire(Empire empire)
    {
        this.empire = empire;
    }
    public Empire GetEmpire()
    {
        return empire;
    }
    public void DestroyArmy()
    {
        empire.RemoveArmy(this);
        Destroy(gameObject);
    }

    public void Merge(Army army)
    {
        attackValue += army.attackValue;
        defenceValue += army.defenceValue;
        army.DestroyArmy();
    }

    public void DepleteArmy(float perc)
    {
        if(perc > 100)
        {
            perc = 100;
        }
        if(perc < 0)
        {
            perc = 0;
        }

        attackValue = (int)Mathf.Ceil(attackValue * perc);
        defenceValue = (int)Mathf.Ceil(defenceValue * perc);
    }
    public int GetAttackValue()
    {
        return attackValue;
    }

    public int GetDefenceValue()
    {
        return defenceValue;
    }

    void Start () {
        movementController = GetComponent<MovementController>();
        movementController.onReachedSystem += OnReachedSystem;
        movementController.onLeaveSystem += OnLeaveSystem;

        if (movementController.GetSystemLocation())
        {
            movementController.GetSystemLocation().AddArmy(this);
        }
        
    }

    private void OnReachedSystem(SolarSystem system)
    {
        if(!system.GetEmpire() || system.GetEmpire() == empire)
        {
            Defend(system);
        }
        else
        {
            Attack(system);
        }
    }

    private void OnLeaveSystem(SolarSystem system)
    {
        system.RemoveArmy(this);
    }

    private void Defend(SolarSystem system)
    {
        system.AddArmy(this);
    }

    private void Attack(SolarSystem system)
    {
        if(attackValue <= 0)
        {
            DestroyArmy();
        }
        else
        {
            system.Defend(this);
        }
        
    }

    public void Addunit(UnitConfig unitConfig)
    {
        attackValue += unitConfig.GetAttackStrength();
        defenceValue += unitConfig.GetDefenceStrength();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
