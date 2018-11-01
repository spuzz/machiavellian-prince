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
        Moving,

    }

    public enum ArmyType
    {
        Defensive,
        Offensive
    }


    SelectableComponent selectableComponent;
    MeshRenderer meshRenderer;
    CapsuleCollider capsuleCollider;

    Empire empire;
    MovementController movementController;
    [SerializeField] int attackValue = 100;
    [SerializeField] int defenceValue = 100;
    [SerializeField] string armyName = "Army";
    List<Unit> units;
    [SerializeField] int maximumUnits = 20;
    [SerializeField] int maintenance = 50;
    [SerializeField] int armyBaseStrength;
    ArmyStatus armyStatus;
    ArmyType armyType;
    Fleet fleet;

    private void Awake()
    {
        units = new List<Unit>();
        selectableComponent = GetComponentInChildren<SelectableComponent>();
        selectableComponent.UpdateName(armyName);
        selectableComponent.SetScale(0.04f);
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        movementController = GetComponent<MovementController>();
        movementController.onReachedSystem += OnReachedSystem;
        movementController.onLeaveSystem += OnLeaveSystem;

        if (movementController.GetSystemLocation())
        {
            movementController.GetSystemLocation().AddArmy(this);
        }
        movementController.SetBlocking(true);
        SetArmyStatus(armyStatus);
    }

    public int GetMaintenance()
    {
        int maint = 0;
        maint += maintenance;
        foreach(Unit unit in units)
        {
            maint += unit.GetMaintenance();
        }
        return maint;
    }

    public IEnumerable<Unit> GetUnits()
    {
        return units;
    }

    public int GetMaxUnits()
    {
        return maximumUnits;
    }

    public Fleet GetFleet()
    {
        return fleet;
    }

    public void SetFleet(Fleet fleet)
    {
        this.fleet = fleet;
    }


    public string GetName()
    {
        return armyName;
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
        if(armyStatus == ArmyStatus.Moving)
        {
            ShowOnMap(true);
        }
        else
        {
            ShowOnMap(false);
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
        selectableComponent.SetColor(empire.GetColor());
    }
    public Empire GetEmpire()
    {
        return empire;
    }
    public void DestroyArmy()
    {
        movementController.GetSystemLocation().RemoveArmy(this);
        empire.RemoveArmy(this);
        if(fleet)
        {
            fleet.RemoveArmy(this);
        }
        Destroy(gameObject);
    }

    public bool Merge(Army army)
    {
        if(units.Count + army.GetUnitCount() > maximumUnits)
        {
            return false;
        }
        foreach(Unit unit in army.GetUnits())
        {
            unit.transform.SetParent(transform);
            units.Add(unit);
        }
        army.DestroyArmy();

        return true;
    }

    public int GetUnitCount()
    {
        return units.Count;
    }

    public void DepleteArmy(float perc)
    {
        if(perc > 1)
        {
            perc = 1;
        }
        if(perc <= 0)
        {
            throw new ArgumentException();
        }

        if (units.Count == 0)
        {
            return;
        }

        float totalUnits = units.Count;
        int numberDestroyed = Convert.ToInt32(totalUnits * perc);

        if(numberDestroyed == 0)
        {
            return;
        }
        units.RemoveRange(0, numberDestroyed);

        //attackValue = (int)Mathf.Ceil(attackValue * perc);
        //defenceValue = (int)Mathf.Ceil(defenceValue * perc);
    }
    //public int GetAttackValue()
    //{
    //    return attackValue;
    //}

    //public int GetDefenceValue()
    //{
    //    return defenceValue;
    //}

    public int GetArmyStrength()
    {
        int strength = 0;
        strength += armyBaseStrength;
        foreach (Unit unit in units)
        {
            strength += unit.GetPower();
        }
        return strength;
    }

    public void Addunit(UnitConfig unitConfig)
    {
        if(units.Count < maximumUnits)
        {
            Unit unit = Instantiate(unitConfig.GetUnitPrefab(), transform).GetComponent<Unit>();
            units.Add(unit);
        }
        else
        {
            throw (new InvalidOperationException());
        }
        
        //attackValue += unitConfig.GetAttackStrength();
        //defenceValue += unitConfig.GetDefenceStrength();
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
            MoveTo(nearestSafeSystem);
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
        SolarSystem currentSystem = movementController.GetSystemLocation();
        if(currentSystem)
        {
            currentSystem.RemoveArmy(this);
        }
        movementController.SetLocation(system);
        movementController.MoveTo(system);
        system.AddArmy(this);
        SetArmyStatus(ArmyStatus.Idle);
    }

    public void Defend(SolarSystem system)
    {
        system.AddArmy(this);
        if(movementController.GetSystemTarget() == system)
        {
            SetArmyStatus(ArmyStatus.Idle);
        }
        
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
        meshRenderer.enabled = show;
        capsuleCollider.enabled = show;
        selectableComponent.SetShown(show);
    }

}
