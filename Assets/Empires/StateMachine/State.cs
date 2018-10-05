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

    protected int CheckMinDefence(Empire empire)
    {

        int total = 0;
        foreach (UnitConfig config in empire.GetTrainingInProgress())
        {
            total += config.GetDefenceStrength();
        }
        if (empire.GetTotalDefence() + total < (minDefencePerSystem * empire.GetSystems().Count))
        {
            List<Army> armies = empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Defensive && c.GetArmyStatus() == Army.ArmyStatus.Idle);
            if(armies.Count > 0)
            {
                armies.OrderBy(c => c.GetDefenceValue());
                if (armies[0].GetComponent<ArmyTrainer>().TrainUnit(empire.GetDefenceConfig()))
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

    protected int CheckMinOffence(Empire empire)
    {

        int total = 0;
        foreach (UnitConfig config in empire.GetTrainingInProgress())
        {
            total += config.GetAttackStrength();
        }
        if (empire.GetTotalOffence() + total < (minOffencePerSystem * empire.GetSystems().Count))
        {
            List<Army> armies = empire.GetArmies().FindAll(c => c.GetArmyType() == Army.ArmyType.Defensive && c.GetArmyStatus() == Army.ArmyStatus.Idle);
            if (armies.Count > 0)
            {
                armies.OrderBy(c => c.GetAttackValue());
                if (armies[0].GetComponent<ArmyTrainer>().TrainUnit(empire.GetAttackConfig()))
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

            if (army.GetComponent<MovementController>().IsMoving() == false)
            {
                float armyValue;
                if (army.GetArmyType() == Army.ArmyType.Offensive)
                {
                    armyValue = army.GetAttackValue();
                }
                else
                {
                    armyValue = army.GetDefenceValue();
                }
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

    protected void DefendBorders(Empire empire, List<Army> armies, List<SolarSystem> enemyBorderSystems)
    {
        
        foreach (SolarSystem system in enemyBorderSystems)
        {
            SolarSystem nearestSystem = Navigation.GetNearestSystem(new List<Empire>() { empire }, system);
            int defenceRouted = 0;
            while (nearestSystem.GetDefence() + defenceRouted < system.GetOffence() && armies.Count > 0)
            {
                Army closestArmy = FindClosestArmy(armies, system);
                defenceRouted += closestArmy.GetDefenceValue();
                armies.Remove(closestArmy);
            }
        }
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
