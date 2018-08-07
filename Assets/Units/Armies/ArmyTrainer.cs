using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyTrainer : MonoBehaviour {

    Army army;
    Universe universe;
    UnitConfig unitToBuild;
    int daysLeftOnBuild;

    void Start () {
        universe = FindObjectOfType<Universe>();
        universe.onDayChanged += OnDayChange;
        army = GetComponent<Army>();
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
            }
        }

    }
}
