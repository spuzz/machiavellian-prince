using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet : MonoBehaviour
{
    SelectableComponent selectableComponent;
    List<Army> armies = new List<Army>();
    Empire empire;
    MovementController movementController;
    private void Awake()
    {
        movementController = GetComponent<MovementController>();
        movementController.onReachedSystem += OnReachedSystem;
        movementController.onLeaveSystem += OnLeaveSystem;
        selectableComponent = GetComponentInChildren<SelectableComponent>();
        selectableComponent.UpdateName("Fleet");
        selectableComponent.SetScale(0.04f);
    }

    private void OnLeaveSystem(SolarSystem system)
    {
        foreach (Army army in armies)
        {
            army.SetArmyStatus(Army.ArmyStatus.Attacking);
        }
    }

    private void OnReachedSystem(SolarSystem system)
    {
        if (system.GetEmpire() == empire)
        {
            DisbandFleet(system);
        }
        else
        {
            Attack(system);
        }
    }

    private void DisbandFleet(SolarSystem system)
    {
        foreach(Army army in armies)
        {
            army.ResetPosition(system);
        }
        Destroy(this.gameObject);
    }

    private void DestroyFleet()
    {
        while (armies.Count > 0)
        {
            armies[0].DestroyArmy();
        }
        Destroy(this.gameObject);
    }


    public void SetLocation(SolarSystem system)
    {
        movementController.SetLocation(system);

    }

    public SolarSystem GetLocation()
    {
        return movementController.GetSystemLocation();
    }

    public void SetTarget(SolarSystem system)
    {
        movementController.MoveTo(system);
    }

    public void SetEmpire(Empire empire)
    {
        selectableComponent.SetColor(empire.GetColor());
        selectableComponent.UpdateName(empire.GetName() + " Fleet");
        selectableComponent.SetShown(true);
        this.empire = empire;
    }

    public bool AddArmy(Army army)
    {
        if (GetLocation() != army.GetComponent<MovementController>().GetSystemLocation())
        {
            return false;
        }
        armies.Add(army);
        army.SetFleet(this);
        return true;
    }

    public void RemoveArmy(Army army)
    {
        armies.Remove(army);
        if(armies.Count == 0)
        {
            DestroyFleet();
        }
    }

    public int GetAttackValue()
    {
        int attackValue = 0;
        foreach(Army army in armies)
        {
            attackValue += army.GetAttackValue();
        }
        return attackValue;
    }
    private void Attack(SolarSystem system)
    {
        float defence = system.GetDefence();
        if (GetAttackValue() <= 0)
        {
            DestroyFleet();
            return;
        }
        else
        {
            if (FightBattle(system))
            {
                system.SetEmpire(empire);
                DisbandFleet(system);
            }
            else
            {
                DestroyFleet();
            }
        }

        

    }


    private bool FightBattle(SolarSystem enemySystem)
    {
        int totalDefence = enemySystem.GetDefence();


        if (GetAttackValue() > totalDefence)
        {
            int attackLeft = GetAttackValue() - totalDefence;
            float percLost = ((float)attackLeft / (float)GetAttackValue());
            foreach (Army army in armies)
            {
                army.DepleteArmy(percLost);
            }

            SetEmpire(empire);
            return true;
        }
        else
        {
            int defenceLeft = totalDefence - GetAttackValue();
            if (defenceLeft == 0)
            {
                defenceLeft = 1;
            }
            float percLost = ((float)defenceLeft / (float)totalDefence);

            enemySystem.DepleteArmies(percLost);
            return false;
        }
    }
}

