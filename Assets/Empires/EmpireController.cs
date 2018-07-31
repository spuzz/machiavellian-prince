using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmpireController : MonoBehaviour {

    Dictionary<SolarSystem, SolarSystem> neutralBorderSystems = new Dictionary<SolarSystem, SolarSystem>();
    Dictionary<SolarSystem, SolarSystem> enemyBorderSystems = new Dictionary<SolarSystem, SolarSystem>();
    [SerializeField] List<UnitConfig> buildableUnits;
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
        empire = gameObject.GetComponent<Empire>();
        diplomacy = gameObject.GetComponent<DiplomacyController>();
        UpdateState(currentState);

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
	// Update is called once per frame
	void Update () {
        if(empire.IsAlive())
        {
            FindNeutralBorderSystems();
            FindEnemyBorderSystems();

            // TODO Simple code to set off war
            if (enemyBorderSystems.Count > 0)
            {
                foreach (SolarSystem system in enemyBorderSystems.Values)
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
            Build();
        }

	}


    public IEnumerator GrowEmpire()
    {
        while(true)
        {

            DefendBorders();
            ColonisePlanets();
            yield return new WaitForFixedUpdate();
        }

    }
    public IEnumerator ExpandEmpire()
    {
        while (true)
        {

            DefendBorders();
            ColonisePlanets();
            yield return new WaitForFixedUpdate();
        }

    }

    public IEnumerator Attack()
    {

        while (true)
        {
            AttackEnemy();
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
                else
                {
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
        FindEnemyBorderSystems();
        
        int border = 0;
        foreach (Army army in empire.GetArmies())
        {
            if (enemyBorderSystems.Count == 0)
            {
                FindNeutralBorderSystems();
                army.GetComponent<MovementController>().MoveTo(empire.GetSystems()[0]);
            }
            else
            {
                if (border >= enemyBorderSystems.Count)
                {
                    border = 0;
                }
                army.GetComponent<MovementController>().MoveTo(enemyBorderSystems.ElementAt(border).Key);
                border++;
            }
        }
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
                colonyShip.GetComponent<MovementController>().MoveTo(neutralBorderSystems.ElementAt(border).Key);
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
                    enemyBorderSystems.Add(system,neighbour);
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
                    neutralBorderSystems.Add(system,neighbour);
                    break;
                }
            }
        }
    }


    private void Build()
    {
        CheckArmies();

    }

    private void CheckArmies()
    {
        // TODO Change Hardcode when to create extra armies

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
}
