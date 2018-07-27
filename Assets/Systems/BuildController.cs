using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {

    SolarSystem localSystem;
    Universe universe;
    UnitConfig unitToBuild;
    int daysLeftOnBuild;
    bool IsBuilding()
    {
        if(unitToBuild)
        {
            return true;
        }
        return false;
    }
    public void BuildUnit(UnitConfig unitConfig)
    {
        if(!IsBuilding() && localSystem)
        {
            unitToBuild = unitConfig;
        }
        daysLeftOnBuild = unitToBuild.GetBuildTime();
    }
	// Use this for initialization
	void Start () {
        universe = FindObjectOfType<Universe>();
        universe.onDayChanged += OnDayChange;

        localSystem = GetComponent<SolarSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSystem()
    {

    }
    public void OnDayChange(int days)
    {
        if(IsBuilding())
        {
            daysLeftOnBuild -= daysLeftOnBuild;
            if (daysLeftOnBuild <= 0)
            {
                if (unitToBuild.WillJoinArmy())
                {
                    if (localSystem.HasArmy())
                    {
                        localSystem.AddToArmy(0, unitToBuild);
                    }
                    else
                    {
                        Empire empire = localSystem.GetEmpire();
                        Army army = Instantiate(unitToBuild.GetUnitPrefab(), empire.transform.Find("Armies").transform).GetComponent<Army>();
                        army.Addunit(unitToBuild);
                        army.GetComponent<MovementController>().SetLocation(localSystem);
                        empire.AddArmy(army);
                    }
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
