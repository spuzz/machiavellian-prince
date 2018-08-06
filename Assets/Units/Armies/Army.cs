using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {

    public enum ArmyStatus
    {
        Idle,
        Training,
        Attacking,
        Defending,
        
    }

    public enum ArmyType
    {
        Defensive,
        Offensive
    }

    Empire empire;
    MovementController movementController;
    [SerializeField] int attackValue = 100;
    [SerializeField] int defenceValue = 100;


    ArmyStatus armyStatus;
    ArmyType armyType;

    public void SetArmyType(ArmyType type)
    {
        armyType = type;
        if(type == ArmyType.Offensive)
        {
            transform.Find("Capsule").GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            transform.Find("Capsule").GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void SetArmyStatus(ArmyStatus status)
    {
        armyStatus = status;
    }

    public ArmyStatus GetArmyStatus()
    {
        return armyStatus;
    }
    
    public ArmyType GetArmyType()
    {
        return armyType;
    }

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
        movementController.Remove();
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
        armyStatus = ArmyStatus.Defending;


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

    public void AttackNearestEnemy()
    {
        List<Empire> enemyEmpires = empire.GetComponent<DiplomacyController>().GetEmpiresAtWar();
        MovementController armyMove = GetComponent<MovementController>();
        SolarSystem system = armyMove.GetNearestSystem(enemyEmpires);
        if (system.GetDefence() < GetAttackValue())
        {
            SetArmyStatus(Army.ArmyStatus.Attacking);
            armyMove.MoveTo(system);
        }
        else
        {
            SetArmyStatus(Army.ArmyStatus.Idle);
            List<Empire> safeEmpires = new List<Empire>();
            safeEmpires.Add(empire);
            SolarSystem nearestSafeSystem = armyMove.GetNearestSystem(safeEmpires, system);
            armyMove.MoveTo(nearestSafeSystem);
        }
    }

}
