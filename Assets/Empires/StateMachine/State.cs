using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class State : ScriptableObject {

    [Header("State General")]
    [SerializeField] string stateName;
    [SerializeField] [Range(0,100)] int percOfEconomyAvailable = 100;
    [SerializeField] int minDefencePerSystem = 0;
    [SerializeField] int minOffencePerSystem = 0;

    public string GetStateName()
    {
        return stateName;
    }
    public int GetEconomyAvailable()
    {
        return percOfEconomyAvailable;
    }

    public int GetMinDefencePerSystem()
    {
        return minDefencePerSystem;
    }
    public int GetMinOffencePerSystem()
    {
        return minOffencePerSystem;
    }

    public void SetEconomyAvailable(int econ)
    {
        if (econ > 100)
        {
            econ = 100;
        }
            
        if(econ < 0)
        {
            econ = 0;
        }

        percOfEconomyAvailable = econ;
    }
    public void SetMinDefencePerSystem(int def)
    {
        minDefencePerSystem = def;
    }
    public void SetMinOffencePerSystem(int off)
    {
        minOffencePerSystem = off;
    }

    abstract public void RunArmyBehaviour(Empire empire, EmpireController empireController);
    abstract public void RunBuildBehaviour(Empire empire, EmpireController empireController);

    protected bool CheckArmies(Empire empire)
    {
        // TODO Change Hardcode of when to create extra armies

        if (empire.GetDefensiveArmies() < (Mathf.Ceil((float)empire.GetSystems().Count / 3.0f)))
        {
            empire.CreateArmy(Army.ArmyType.Defensive, empire.GetSystems()[0]);
            return true;
        }


        if (empire.GetOffensiveArmies() < (Mathf.Ceil((float)empire.GetSystems().Count / 5.0f)))
        {
            empire.CreateArmy(Army.ArmyType.Offensive, empire.GetSystems()[0]);
            return true;
        }
        return false;
    }

    protected bool CheckMinDefence(Empire empire)
    {

        int total = 0;
        foreach (UnitConfig config in empire.GetBuildingInProgress())
        {
            total += config.GetDefenceStrength();
        }
        if (empire.GetDefensiveArmies() + total < minDefencePerSystem)
        {
            List<Army> armies = empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Defensive && c.GetArmyStatus() == Army.ArmyStatus.Idle);
            if(armies.Count > 0)
            {
                armies.OrderBy(c => c.GetDefenceValue());
                //armies[0].buildUnit();
                return true;
            }
        }
        return false;

    }

    protected bool CheckMinOffence(Empire empire)
    {

        int total = 0;
        foreach (UnitConfig config in empire.GetBuildingInProgress())
        {
            total += config.GetAttackStrength();
        }
        if (empire.GetDefensiveArmies() + total < minOffencePerSystem)
        {
            List<Army> armies = empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Defensive && c.GetArmyStatus() == Army.ArmyStatus.Idle);
            if (armies.Count > 0)
            {
                armies.OrderBy(c => c.GetAttackValue());
                //armies[0].buildUnit();
                return true;
            }
        }
        return false;
    }

    protected bool BuildInfrastructure(Empire empire)
    {
        return false;
    }
    protected bool BuildColonyShip(Empire empire)
    {
        return false;
        //if (empire.GetColonyShips().Count >= 1)
        //{
        //    return;
        //}
        //foreach (SolarSystem system in empire.GetSystems())
        //{
        //    UnitConfig unitBuilding = system.GetComponent<BuildController>().GetUnitBuilding();
        //    if (unitBuilding && unitBuilding == colonyShipConfig)
        //    {
        //        return;
        //    }
        //}
        //foreach (SolarSystem system in empire.GetSystems())
        //{
        //    BuildController builder = system.GetComponent<BuildController>();
        //    if (builder.IsBuilding() == false)
        //    {
        //        buildOrder.Enqueue(colonyShipConfig);
        //        break;
        //    }
        //}
    }





    protected bool TrainArmy(Empire empire)
    {
        return false;
        //Army smallestArmy = null;
        //float smallestArmyValue = -1;
        //if (empire.GetGold() < buildableUnits[1].GetCost())
        //{
        //    return;
        //}
        //foreach (Army army in empire.GetArmies())
        //{

        //    if (army.GetComponent<MovementController>().IsMoving() == false)
        //    {
        //        float armyValue;
        //        if (army.GetArmyType() == Army.ArmyType.Offensive)
        //        {
        //            armyValue = army.GetAttackValue();
        //        }
        //        else
        //        {
        //            armyValue = army.GetDefenceValue();
        //        }
        //        if (smallestArmyValue == -1 || armyValue < smallestArmyValue)
        //        {
        //            smallestArmy = army;
        //            smallestArmyValue = armyValue;
        //        }
        //    }
        //}
        //if (smallestArmy)
        //{
        //    SolarSystem system = smallestArmy.GetComponent<MovementController>().GetSystemLocation();
        //    if (smallestArmy.GetArmyType() == Army.ArmyType.Offensive)
        //    {

        //        system.GetComponent<BuildController>().BuildUnit(buildableUnits[1], smallestArmy);
        //        empire.UseGold(buildableUnits[1].GetCost());
        //    }
        //    else
        //    {
        //        system.GetComponent<BuildController>().BuildUnit(buildableUnits[2], smallestArmy);
        //        empire.UseGold(buildableUnits[2].GetCost());
        //    }

        //}
    }

    //private void AttackEnemy()
    //{


    //    foreach (Army army in empire.GetArmies())
    //    {
    //        if (army.GetArmyStatus() != Army.ArmyStatus.Training)
    //        {
    //            if (army.GetArmyType() == Army.ArmyType.Offensive)
    //            {
    //                army.AttackNearestEnemy();
    //            }
    //        }

    //    }

    //}


    //private void DefendBorders()
    //{

    //    if (systemChangeUpdate == false)
    //    {
    //        return;
    //    }
    //    systemChangeUpdate = false;
    //    List<Army> defendingArmies = new List<Army>();
    //    foreach (Army army in empire.GetArmies())
    //    {
    //        defendingArmies = new List<Army>();
    //    }
    //    while (defendingArmies.Count > 0)
    //    {
    //        foreach (SolarSystem enemySystem in empireEnemyBorderSystems)
    //        {
    //            Army closestArmy = FindClosestArmyToSystem(defendingArmies, enemySystem);
    //            closestArmy.GetComponent<MovementController>().MoveTo(enemySystem);
    //            defendingArmies.Remove(closestArmy);
    //        }

    //    }

    //}

    //private void ColonisePlanets()
    //{

    //    int border = 0;
    //    foreach (ColonyShip colonyShip in empire.GetColonyShips())
    //    {
    //        if (neutralBorderSystems.Count == 0)
    //        {
    //            colonyShip.GetComponent<MovementController>().MoveTo(empire.GetSystems()[0]);
    //        }
    //        else
    //        {
    //            if (border >= neutralBorderSystems.Count)
    //            {
    //                border = 0;
    //            }
    //            colonyShip.GetComponent<MovementController>().MoveTo(neutralBorderSystems.ElementAt(border));
    //            border++;
    //        }
    //    }
    //}
}
