using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmpireController : MonoBehaviour {

    List<SolarSystem> neutralBorderSystems = new List<SolarSystem>();
    List<SolarSystem> enemyBorderSystems = new List<SolarSystem>();
    List<SolarSystem> empireEnemyBorderSystems = new List<SolarSystem>();
    [SerializeField] PersonalityConfig personality;
    Queue<UnitConfig> buildOrder = new Queue<UnitConfig>();
    [SerializeField] List<UnitConfig> buildableUnits;
    [SerializeField] Universe m_universe;
    UnitConfig colonyShipConfig;
    bool systemChangeUpdate = true;
    enum MissionState
    {
        Idle,
        Expand,
        Defend,
        Attack,
        Grow,
    }


    [SerializeField] MissionState currentState;
    Empire empire;
    DiplomacyController diplomacy;

    void Start () {
        m_universe = FindObjectOfType<Universe>();
        m_universe.onSystemOwnerChanged += onSystemChange;
        
        empire = gameObject.GetComponent<Empire>();
        diplomacy = gameObject.GetComponent<DiplomacyController>();
        foreach (UnitConfig config in buildableUnits)
        {
            if (config.name == "ColonyShip")
            {
                colonyShipConfig = config;
            }
        }
        UpdateState(currentState);

    }
	
    // Update is called once per frame
    void Update () {
        if(empire.IsAlive())
        {

            // TODO Simple code to set off war
            if (enemyBorderSystems.Count > 0)
            {
                foreach (SolarSystem system in enemyBorderSystems)
                {
                    if (diplomacy.GetDiplomacy(system.GetEmpire()).relationship != Relationship.War)
                    {
                        diplomacy.DeclareWar(empire, system.GetEmpire());
                    }
                }
                if (currentState != MissionState.Attack)
                {
                    UpdateState(MissionState.Attack);
                }
            }
            else
            {
                if (currentState == MissionState.Attack)
                {
                    UpdateState(MissionState.Grow);
                }
            }
            
        }

	}

    void UpdateState(MissionState state)
    {
        StopAllCoroutines();
        currentState = state;
        switch (currentState)
        {
            case MissionState.Grow:
                StartCoroutine("GrowEmpire");
                break;
            case MissionState.Attack:
                StartCoroutine("Attack");
                break;
        }
    }

    private void onSystemChange(SolarSystem system)
    {
        systemChangeUpdate = true;
        FindEnemyBorderSystems();
        FindNeutralBorderSystems();
    }

    public IEnumerator GrowEmpire()
    {
        while(true)
        {
            
            DefendBorders();
            ColonisePlanets();
            Build();
            yield return new WaitForFixedUpdate();
        }

    }
    public IEnumerator ExpandEmpire()
    {
        while (true)
        {
            
            DefendBorders();
            ColonisePlanets();
            Build();
            yield return new WaitForFixedUpdate();
        }

    }

    public IEnumerator Attack()
    {

        while (true)
        {
            DefendBorders();
            AttackEnemy();
            
            Build();
            yield return new WaitForFixedUpdate();
        }

    }

    private void AttackEnemy()
    {
        
        
        foreach (Army army in empire.GetArmies())
        {
            if(army.GetArmyStatus() != Army.ArmyStatus.Training)
            {
                if(army.GetArmyType() == Army.ArmyType.Offensive)
                {
                    army.AttackNearestEnemy();
                }
            }

        }

    }


    public IEnumerator Defend()
    {
        while (true)
        {

            DefendBorders();
            yield return new WaitForFixedUpdate();
        }

    }

    private void DefendBorders()
    {
        
        if(systemChangeUpdate == false)
        {
            return;
        }
        systemChangeUpdate = false;
        List<Army> defendingArmies = new List<Army>();
        foreach (Army army in empire.GetArmies())
        {
            defendingArmies = new List<Army>();
        }
        while(defendingArmies.Count > 0)
        {
            foreach (SolarSystem enemySystem in empireEnemyBorderSystems)
            {
                Army closestArmy = FindClosestArmyToSystem(defendingArmies, enemySystem);
                closestArmy.GetComponent<MovementController>().MoveTo(enemySystem);
                defendingArmies.Remove(closestArmy);
            }

        }

    }

    private Army FindClosestArmyToSystem(List<Army> armies, SolarSystem system)
    {
        float closestDistance =  -1;
        Army closestArmy = null;
        foreach(Army army in armies)
        {
            float distance = Vector3.Distance(army.transform.position, system.transform.position);
            if (closestDistance == -1 || distance < closestDistance)
            {
                closestDistance = distance;
                closestArmy = army;
            }
        }
        return closestArmy;
    }

    private void ColonisePlanets()
    {
        
        int border = 0;
        foreach (ColonyShip colonyShip in empire.GetColonyShips())
        {
            if (neutralBorderSystems.Count == 0)
            {
                colonyShip.GetComponent<MovementController>().MoveTo(empire.GetSystems()[0]);
            }
            else
            {
                if (border >= neutralBorderSystems.Count)
                {
                    border = 0;
                }
                colonyShip.GetComponent<MovementController>().MoveTo(neutralBorderSystems.ElementAt(border));
                border++;
            }
        }
    }


    private void FindEnemyBorderSystems()
    {
        enemyBorderSystems.Clear();
        foreach (SolarSystem system in empire.GetSystems())
        {
            foreach (SolarSystem neighbour in system.GetNearbySystems())
            {
                if (neighbour.GetEmpire() && neighbour.GetEmpire() != empire)
                {
                    if(!enemyBorderSystems.Contains(neighbour))
                    {
                        enemyBorderSystems.Add(neighbour);

                    }
                    if (!empireEnemyBorderSystems.Contains(system))
                    {
                        empireEnemyBorderSystems.Add(system);

                    }
                   
                    break;
                }
            }
        }
    }

    private void FindNeutralBorderSystems()
    {
        neutralBorderSystems.Clear();
        foreach (SolarSystem system in empire.GetSystems())
        {
            foreach (SolarSystem neighbour in system.GetNearbySystems())
            {
                if (!neighbour.GetEmpire())
                {
                    if(!neutralBorderSystems.Contains(neighbour))
                    {
                        neutralBorderSystems.Add(neighbour);
                    }
                    
                }
            }
        }
    }
    private void Build()
    {
        CheckArmies();
        buildOrder.Clear();
        BuildMinimumArmies();
        if (currentState == MissionState.Expand)
        {
            BuildColonyShip();
        }

        BuildUpArmies();
    }

    private void BuildMinimumArmies()
    {
        float maxEcon = personality.m
        int total = 0;
        foreach(UnitConfig config in empire.GetBuildingInProgress())
        {
            total += config.GetDefenceStrength();
        }
        if(empire.GetDefensiveArmies() + total < personality.)
    }

    private void BuildColonyShip()
    {
        if (empire.GetColonyShips().Count >= 1)
        {
            return;
        }
        foreach (SolarSystem system in empire.GetSystems())
        {
            UnitConfig unitBuilding = system.GetComponent<BuildController>().GetUnitBuilding();
            if (unitBuilding && unitBuilding == colonyShipConfig)
            {
                return;
            }
        }
        foreach (SolarSystem system in empire.GetSystems())
        {
            BuildController builder = system.GetComponent<BuildController>();
            if (builder.IsBuilding() == false)
            {
                buildOrder.Enqueue(colonyShipConfig);
                break;
            }
        }
    }



    private void CheckArmies()
    {
        // TODO Change Hardcode of when to create extra armies

        if (empire.GetDefensiveArmies() < (Mathf.Ceil((float)empire.GetSystems().Count / 3.0f)) && empire.GetPredictedIncome() > 100)
        {
            empire.CreateArmy(Army.ArmyType.Defensive, empire.GetSystems()[0]);
        }
        
        if(currentState != MissionState.Attack)
        {
            if(empire.GetOffensiveArmies() < (Mathf.Ceil((float)empire.GetSystems().Count / 5.0f)) && empire.GetPredictedIncome() > 100)
            {
                empire.CreateArmy(Army.ArmyType.Offensive, empire.GetSystems()[0]);
            }
        }
        else
        {
            if (empire.GetOffensiveArmies() < (Mathf.Ceil((float)empire.GetSystems().Count / 3.0f)) && empire.GetPredictedIncome() > 100)
            {
                empire.CreateArmy(Army.ArmyType.Offensive, empire.GetSystems()[0]);
            }
        }
    }

    private void BuildUpArmies()
    {
        Army smallestArmy = null;
        float smallestArmyValue = -1;
        if (empire.GetGold() < buildableUnits[1].GetCost())
        {
            return;
        }
        foreach (Army army in empire.GetArmies())
        {

            if(army.GetComponent<MovementController>().IsMoving() == false)
            {
                float armyValue;
                if(army.GetArmyType() == Army.ArmyType.Offensive)
                {
                    armyValue = army.GetAttackValue();
                }
                else
                {
                    armyValue = army.GetDefenceValue();
                }
                if(smallestArmyValue == -1 || armyValue < smallestArmyValue)
                {
                    smallestArmy = army;
                    smallestArmyValue = armyValue;
                }
            }
        }
        if(smallestArmy)
        {
            SolarSystem system = smallestArmy.GetComponent<MovementController>().GetSystemLocation();
            if (smallestArmy.GetArmyType() == Army.ArmyType.Offensive)
            {
  
                system.GetComponent<BuildController>().BuildUnit(buildableUnits[1],smallestArmy);
                empire.UseGold(buildableUnits[1].GetCost());
            }
            else
            {
                system.GetComponent<BuildController>().BuildUnit(buildableUnits[2], smallestArmy);
                empire.UseGold(buildableUnits[2].GetCost());
            }
            
        }
    }
    


}
