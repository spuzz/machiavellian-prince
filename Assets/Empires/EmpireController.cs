using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmpireController : MonoBehaviour {

    List<SolarSystem> neutralBorderSystems = new List<SolarSystem>();
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
            
            GuardBorders();
            ColonisePlanets();
            yield return new WaitForFixedUpdate();
        }

    }
    public IEnumerator ExpandEmpire()
    {
        while (true)
        {

            GuardBorders();
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
        List<Empire> enemyEmpires = diplomacy.GetEmpiresAtWar();
        
        foreach (Army army in empire.GetArmies())
        {
            MovementController armyMove = army.GetComponent<MovementController>();
            SolarSystem system  = armyMove.GetNearestSystem(enemyEmpires);
            if(system.GetDefence() < army.GetAttackValue())
            {
                armyMove.MoveTo(system);
            }
            else
            {
                List<Empire> safeEmpires = new List<Empire>();
                safeEmpires.Add(empire);
                SolarSystem nearestSafeSystem = armyMove.GetNearestSystem(safeEmpires, system);
                armyMove.MoveTo(nearestSafeSystem);
            }
        }

        foreach(SolarSystem system in empire.GetSystems())
        {
            system.MergeArmies();
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
        throw new NotImplementedException();
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
                colonyShip.GetComponent<MovementController>().MoveTo(neutralBorderSystems[border]);
                border++;
            }
        }
    }

    private void GuardBorders()
    {
        FindEnemyBorderSystems();
        int border = 0;
        foreach(Army army in empire.GetArmies())
        {
            if(enemyBorderSystems.Count == 0)
            {
                army.GetComponent<MovementController>().MoveTo(empire.GetSystems()[0]);
            }
            else
            {
                if(border >= enemyBorderSystems.Count)
                {
                    border = 0;
                }
                army.GetComponent<MovementController>().MoveTo(enemyBorderSystems.ElementAt(border).Key);
                border++;
            }
        }
    }

    private void FindEnemyBorderSystems()
    {
        enemyBorderSystems.Clear();
        foreach (SolarSystem system in empire.GetSystems())
        {
            foreach (SolarSystem neighbour in system.GetNearbySystems().Keys)
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
            foreach (SolarSystem neighbour in system.GetNearbySystems().Keys)
            {
                if (!neighbour.GetEmpire())
                {
                    neutralBorderSystems.Add(neighbour);
                    break;
                }
            }
        }
    }

    private void Build()
    {
        UnitConfig.BuildType buildType = UnitConfig.BuildType.Expand;
        UnitConfig tryToBuild = null;
        if(currentState == MissionState.Attack)
        {
            buildType = UnitConfig.BuildType.Attack;
        }
        foreach(UnitConfig unitConfig in buildableUnits)
        {
            if(unitConfig.GetBuildType() == buildType)
            {
                tryToBuild = unitConfig;
            }
        }
        if(tryToBuild)
        {
            if (empire.GetGold() > tryToBuild.GetCost())
            {
                empire.GetSystems()[0].GetComponent<BuildController>().BuildUnit(tryToBuild);
                empire.UseGold(tryToBuild.GetCost());
            }
        }

        
    }
}
