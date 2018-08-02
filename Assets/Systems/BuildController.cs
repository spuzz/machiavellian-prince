using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {

    SolarSystem localSystem;
    Universe universe;
    UnitConfig unitToBuild;
    int daysLeftOnBuild;
    Army armyToJoin;

    public UnitConfig GetUnitBuilding()
    {
        return unitToBuild;
    }

	void Start () {
        universe = FindObjectOfType<Universe>();
        universe.onDayChanged += OnDayChange;

        localSystem = GetComponent<SolarSystem>();
	}

    public bool IsBuilding()
    {
        if (unitToBuild)
        {
            return true;
        }
        return false;
    }

    public void BuildUnit(UnitConfig unitConfig, Army armyToJoin)
    {
        if (!IsBuilding() && localSystem)
        {
            unitToBuild = unitConfig;
            this.armyToJoin = armyToJoin;
            daysLeftOnBuild = unitToBuild.GetBuildTime();
        }
        
    }

    public void OnDayChange(int days)
    {
        if(IsBuilding())
        {
            daysLeftOnBuild -= days;
            if (daysLeftOnBuild <= 0)
            {
                if (unitToBuild.WillJoinArmy())
                {
                    armyToJoin.Addunit(unitToBuild);
                }
                else
                {
                    Empire empire = localSystem.GetEmpire();
                    ColonyShip ship = Instantiate(unitToBuild.GetUnitPrefab(), empire.transform.Find("ColonyShips").transform).GetComponent<ColonyShip>();
                    ship.GetComponent<MovementController>().SetLocation(localSystem);
                    empire.AddColonyShip(ship);
                }
                unitToBuild = null;
            }
        }

    }
}
