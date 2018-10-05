using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {

    SelectableComponent selectableComponent;
    public enum ArmyStatus
    {
        Idle,
        Training,
        Attacking,
        Moving,
        
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
    [SerializeField] string name = "Army";

    ArmyStatus armyStatus;
    ArmyType armyType;

    private void Awake()
    {
        selectableComponent = GetComponentInChildren<SelectableComponent>();
        selectableComponent.UpdateName(name);
        selectableComponent.SetScale(0.04f);
    }

    void Start()
    {
        movementController = GetComponent<MovementController>();
        movementController.onReachedSystem += OnReachedSystem;
        movementController.onLeaveSystem += OnLeaveSystem;

        if (movementController.GetSystemLocation())
        {
            movementController.GetSystemLocation().AddArmy(this);
        }
        movementController.SetBlocking(true);
        SetArmyStatus(ArmyStatus.Idle);


    }

    public void Update()
    {

    }

    public string GetName()
    {
        return name;
    }

    public void SetArmyType(ArmyType type)
    {
        armyType = type;
        if(type == ArmyType.Offensive)
        {
            transform.Find("ArmyMesh").GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            transform.Find("ArmyMesh").GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void SetArmyStatus(ArmyStatus status)
    {
        armyStatus = status;
        if(armyStatus == ArmyStatus.Idle || armyStatus == ArmyStatus.Training)
        {
            ShowOnMap(false);
        }
        else
        {
            ShowOnMap(true);
        }
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
        movementController.GetSystemLocation().RemoveArmy(this);
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

    public void Addunit(UnitConfig unitConfig)
    {
        attackValue += unitConfig.GetAttackStrength();
        defenceValue += unitConfig.GetDefenceStrength();
    }

    public void MoveToNearestEnemy()
    {
        List<Empire> enemyEmpires = empire.GetComponent<DiplomacyController>().GetEmpiresAtWar();
        if (enemyEmpires.Count == 0)
        {
            return;
        }
        MovementController armyMove = GetComponent<MovementController>();
        SolarSystem systemLoc = armyMove.GetSystemLocation();
        if (!systemLoc)
        {
            systemLoc = armyMove.GetSystemNextDesination();
        }
        SolarSystem system = Navigation.GetNearestSystem(enemyEmpires, systemLoc);


        List<Empire> safeEmpires = new List<Empire>();
        safeEmpires.Add(empire);
        SolarSystem nearestSafeSystem = Navigation.GetNearestSystem(safeEmpires, system);
        if (movementController.GetSystemLocation() != nearestSafeSystem)
        {
            armyMove.MoveTo(nearestSafeSystem);
        }

    }


    public void MoveTo(SolarSystem system)
    {
        if(movementController.MoveTo(system))
        {
            SetArmyStatus(ArmyStatus.Moving);
        }
    }

    public void ResetPosition(SolarSystem system)
    {
        movementController.SetLocation(system);
        movementController.MoveTo(system);
        SetArmyStatus(ArmyStatus.Idle);
    }

    public void Defend(SolarSystem system)
    {
        system.AddArmy(this);
        SetArmyStatus(ArmyStatus.Idle);
    }

    private void OnReachedSystem(SolarSystem system)
    {
        if(!system.GetEmpire() || system.GetEmpire() == empire)
        {
            Defend(system);
        }
    }

    private void OnLeaveSystem(SolarSystem system)
    {
        system.RemoveArmy(this);
        SetArmyStatus(ArmyStatus.Moving);
    }



    private void ShowOnMap(bool show)
    {
        GetComponentInChildren<MeshRenderer>().enabled = show;
        GetComponentInChildren<CapsuleCollider>().enabled = show;
        selectableComponent.enabled = show;
    }

}
