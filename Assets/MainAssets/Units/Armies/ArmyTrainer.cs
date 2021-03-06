﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyTrainer : MonoBehaviour {

    Army army;
    Universe universe;
    UnitConfig unitToBuild;
    int daysLeftOnBuild;

    private void Awake()
    {
        army = GetComponent<Army>();
    }
    void Start () {
        universe = FindObjectOfType<Universe>();
        universe.onDayChanged += OnDayChange;
        
	}

    private void OnDestroy()
    {
        universe.onDayChanged -= OnDayChange;
    }

    public UnitConfig GetUnitBuilding()
    {
        return unitToBuild;
    }
    public bool IsBuilding()
    {
        if (unitToBuild)
        {
            return true;
        }
        return false;
    }
    public int GetDaysLeftToBuild()
    {
        return daysLeftOnBuild;
    }

    public bool TrainUnit(UnitConfig unitConfig)
    {
        if(!army)
        {
            return false;
        }

        if (!IsBuilding() && army.GetEmpire().UseGold(unitConfig.GetCost()))
        {
            unitToBuild = unitConfig;
            daysLeftOnBuild = unitToBuild.GetBuildTime();
            army.SetArmyStatus(Army.ArmyStatus.Training);
            return true;
        }
        return false;

    }
    public void OnDayChange(int days)
    {
        if (IsBuilding())
        {

            daysLeftOnBuild -= days;
            if (daysLeftOnBuild <= 0)
            {
                army.Addunit(unitToBuild);
                unitToBuild = null;
                army.SetArmyStatus(Army.ArmyStatus.Idle);
            }
        }

    }

    internal void CancelAllTraining()
    {
        if(IsBuilding())
        {
            unitToBuild = null;
            army.SetArmyStatus(Army.ArmyStatus.Idle);
        }
    }
}
