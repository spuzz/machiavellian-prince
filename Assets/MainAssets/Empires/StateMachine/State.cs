using System;
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
    
    protected bool isInDefaultBuild = false;
    protected delegate int BuildFunction(Empire empire);
    protected List<BuildFunction> buildFunctions;
    protected List<BuildFunction> defaultBuildFunctions;

    public State()
    {
        buildFunctions = new List<BuildFunction>();
        defaultBuildFunctions = new List<BuildFunction>();
    }
    public bool GetIsInDefaultBuildMode()
    {
        return isInDefaultBuild;
    }
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

    protected bool Build(Empire empire)
    {
        isInDefaultBuild = false;
        if ((float)empire.GetPredictedNetIncome() / (float)empire.GetPredictedGrossIncome() > GetEconomyAvailable())
        {
            return false;
        }

        foreach (BuildFunction function in buildFunctions)
        {
            int status = function(empire);
            if (status == 1)
            {
                return false;
            }
            else if (status == 2)
            {
                return true;
            }
        }
        isInDefaultBuild = true;

        foreach (BuildFunction function in defaultBuildFunctions)
        {
            int status = function(empire);
            if (status == 1)
            {
                return false;
            }
            else if (status == 2)
            {
                return true;
            }
        }

        return false;
    }


    protected int CheckArmies(Empire empire)
    {
        // TODO Change Hardcode of when to create extra armies

        if (empire.GetDefensiveArmies() < (Mathf.Ceil((float)empire.GetSystems().Count / 3.0f)))
        {
            empire.CreateArmy(Army.ArmyType.Defensive, empire.GetSystems()[0]);
            return 2;
        }


        if (empire.GetOffensiveArmies() < (Mathf.Ceil((float)empire.GetSystems().Count / 5.0f)))
        {
            empire.CreateArmy(Army.ArmyType.Offensive, empire.GetSystems()[0]);
            return 2;
        }
        return 0;
    }


    protected int BuildInfrastructure(Empire empire)
    {
        return 0;
    }
    protected int BuildColonyShip(Empire empire)
    {

        foreach (SolarSystem system in empire.GetSystems())
        {
            BuildController builder = system.GetComponent<BuildController>();
            if (builder.IsBuilding() == false)
            {
                if (builder.BuildBuilding(empire.GetColonyShipConfig()))
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    protected int TrainArmy(Empire empire)
    {
        Army smallestArmy = null;
        float smallestArmyValue = -1;
        foreach (Army army in empire.GetArmies())
        {

            if (army.GetArmyStatus() == Army.ArmyStatus.Idle)
            {
                float armyValue;
                armyValue = army.GetArmyStrength();
                if (smallestArmyValue == -1 || armyValue < smallestArmyValue)
                {
                    smallestArmy = army;
                    smallestArmyValue = armyValue;
                }
            }
        }
        if (smallestArmy)
        {
            if (smallestArmy.GetArmyType() == Army.ArmyType.Offensive)
            {

                if (smallestArmy.GetComponent<ArmyTrainer>().TrainUnit(empire.GetAttackConfig()))
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (smallestArmy.GetComponent<ArmyTrainer>().TrainUnit(empire.GetDefenceConfig()))
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }

        }

        return 0;
    }

    protected void DefendBorders(Empire empire, EmpireController empireController)
    {
        List<SolarSystem> enemyBorderSystems = empireController.GetEmpireEnemyBorderSystems();
        List<Army> defensiveArmies = empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Defensive);

        List<Army> availableArmies;
        availableArmies = empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Defensive
               && c.GetArmyStatus() != Army.ArmyStatus.Training
               && c.GetArmyStatus() != Army.ArmyStatus.Attacking);

        foreach(Army army in availableArmies)
        {
            RouteDefensiveArmy(army, enemyBorderSystems, defensiveArmies, empire);
        }
    }

    private void RouteDefensiveArmy(Army army, List<SolarSystem> enemyBorderSystems, List<Army> defensiveArmies, Empire empire)
    {
        // order by distance to this army

        List<SolarSystem> options;
        if (enemyBorderSystems.Count == 0)
        {
            options = empire.GetSystems().OrderBy(x => Vector2.Distance(army.transform.position, x.transform.position)).ToList();
        }
        else
        {
            options = enemyBorderSystems.OrderBy(x => Vector2.Distance(army.transform.position, x.transform.position)).ToList();
        }

        // Try to find closest border system with no defensive army in route
        foreach (SolarSystem system in options)
        {
            if(!defensiveArmies.Find( x => x != army && x.GetComponent<MovementController>().GetSystemTarget() == system))
            {
                army.MoveTo(system);
                return;
            }
        }

        // No Unoccupied systems to go to in list to move to closest
        army.MoveTo(options[0]);
        return;

    }

    private Army FindClosestArmy(List<Army> armies, SolarSystem system)
    {
        Army closestArmy = null;
        int distance = -1;
        foreach(Army army in armies)
        {
            if(distance == -1 || Vector3.Distance(army.transform.position, system.transform.position) < distance)
            {
                closestArmy = army;
            }
        }
        return closestArmy;
    }


    protected void ColonisePlanets(Empire empire, EmpireController empireController)
    {

        int border = 0;
        foreach (ColonyShip colonyShip in empire.GetColonyShips())
        {
            if (empireController.GetNeutralBorderSystems().Count == 0)
            {
                colonyShip.MoveTo(empire.GetSystems()[0]);
            }
            else
            {
                if (border >= empireController.GetNeutralBorderSystems().Count)
                {
                    border = 0;
                }
                colonyShip.MoveTo(empireController.GetNeutralBorderSystems().ElementAt(border));
                border++;
            }
        }
    }
}
