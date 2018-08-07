using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {

    [SerializeField] GameObject colonyShip;

    SolarSystem localSystem;
    Universe universe;
    bool isBuilding;
    BuildingConfig buildingUnderConstruction;
    int daysLeftOnBuild;



	void Start () {
        universe = FindObjectOfType<Universe>();
        universe.onDayChanged += OnDayChange;

        localSystem = GetComponent<SolarSystem>();
        isBuilding = false;

    }

    public BuildingConfig GetBuildingInConstruction()
    {
        return buildingUnderConstruction;
    }
    public bool BuildBuilding(BuildingConfig config)
    {
        if (!IsBuilding() && localSystem.GetEmpire().UseGold(config.GetCost()))
        {
            buildingUnderConstruction = config;
            daysLeftOnBuild = config.GetBuildTime();
            isBuilding = true;
            return true;
        }
        return false;
    }
    public bool IsBuilding()
    {
        return isBuilding;
    }

    private void InstantiateColonyShip()
    {
        Empire empire = localSystem.GetEmpire();
        ColonyShip ship = Instantiate(colonyShip, empire.transform.Find("ColonyShips").transform).GetComponent<ColonyShip>();
        ship.GetComponent<MovementController>().SetLocation(localSystem);
        empire.AddColonyShip(ship);
    }

    public void OnDayChange(int days)
    {
        if (IsBuilding())
        {
            daysLeftOnBuild -= days;
            if (daysLeftOnBuild <= 0)
            {
                isBuilding = false;
                if (buildingUnderConstruction.GetName() == "ColonyShip")
                {
                    InstantiateColonyShip();
                }
                else
                {
                    // TODO solarsystem buildings
                }
            }
        }

    }



}
