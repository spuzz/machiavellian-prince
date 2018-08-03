using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour {

    [SerializeField] string EmpireName;
    [SerializeField] Color empireColour;
    Leader m_currentLeader;
    List<Leader> m_allPotentialLeaders;
    List<Army> armies = new List<Army>();
    List<ColonyShip> colonyShips = new List<ColonyShip>();
    List<SolarSystem> m_ownedSystems = new List<SolarSystem>();
    [SerializeField] GameObject armyPrefab;

    bool isAlive = true;

    // Resources
    [SerializeField] int gold;

    public void DestroyEmpire()
    {
        isAlive = false;
        while(armies.Count !=0)
        {
            armies[0].DestroyArmy();
        }
        while (colonyShips.Count != 0)
        {
            colonyShips[0].DestroyColonyShip();
        }

        FindObjectOfType<Universe>().CheckEndGame();
    }

    public bool IsAlive()
    {
        return isAlive;
    }
    public int GetGold()
    {
        return gold;

    }
    // Use this for initialization
    public void AddGold(int gold)
    {
        this.gold += gold;
    }

    public bool UseGold(int gold)
    {
        if(this.gold >= gold)
        {
            this.gold -= gold;
            return true;
        }
        return false;
        
    }

    public int GetPredictedIncome()
    {
        int netIncome = 0;
        foreach(SolarSystem system in GetSystems())
        {
            netIncome += system.GetNetIncome();
        }
        foreach(Army army in armies)
        {
            netIncome -= 50;
        }
        return netIncome;
    }


    public Color GetColor()
    {
        return empireColour;
    }
    public Leader GetLeader()
    {
        return m_currentLeader;
    }

    public void GiveSystem(SolarSystem system)
    {
        if(!m_ownedSystems.Find(c => c.GetName() == system.GetName()))
        {
            m_ownedSystems.Add(system);
        }
        
    }

    public void TakeSystem(SolarSystem system)
    {
        if (m_ownedSystems.Find(c => c.GetName() == system.GetName()))
        {
            m_ownedSystems.Remove(system);
        }
        if(m_ownedSystems.Count == 0)
        {
            DestroyEmpire();
        }

    }

    public List<SolarSystem> GetSystems()
    {
        return m_ownedSystems;
    }

    public List<Army> GetArmies()
    {
        return armies;
    }


    public void RemoveArmy(Army army)
    {
        armies.Remove(army);
    }

    public float GetTotalDefence()
    {
        float total = 0;
        foreach(Army army in armies)
        {
            total += army.GetDefenceValue();
        }
        return total;
    }

    public List<UnitConfig> GetBuildingInProgress()
    {
        List < UnitConfig > buildList = new List<UnitConfig>();
        foreach (SolarSystem system in m_ownedSystems)
        {
            if(system.GetComponent<BuildController>().IsBuilding())
            {
                buildList.Add(system.GetComponent<BuildController>().GetUnitBuilding());
            }
        }
        return buildList;
    }

    public float GetTotalOffence()
    {
        float total = 0;
        foreach (Army army in armies)
        {
            total += army.GetAttackValue();
        }
        return total;
    }

    public List<ColonyShip> GetColonyShips()
    {
        return colonyShips;
    }

    public void AddColonyShip(ColonyShip ship)
    {
        ship.SetEmpire(this);
        colonyShips.Add(ship);
    }
    public void RemoveColonyShip(ColonyShip colonyShip)
    {
        colonyShips.Remove(colonyShip);
    }

    public List<Leader> GetPotentialLeaders()
    {
        return m_allPotentialLeaders;
    }

    void Start()
    {

        m_allPotentialLeaders = new List<Leader>(GetComponentsInChildren<Leader>());
        if (m_allPotentialLeaders.Count > 0)
        {
            m_currentLeader = m_allPotentialLeaders[0];
        }

        armies = new List<Army>(GetComponentsInChildren<Army>());
        foreach (Army army in armies)
        {
            army.SetEmpire(this);
            if (!army.GetComponent<MovementController>().GetSystemLocation())
            {
                army.GetComponent<MovementController>().SetLocation(m_ownedSystems[0]);
            }
        }

        colonyShips = new List<ColonyShip>(GetComponentsInChildren<ColonyShip>());
        foreach (ColonyShip colonyShip in colonyShips)
        {
            colonyShip.SetEmpire(this);
            if (!colonyShip.GetComponent<MovementController>().GetSystemLocation())
            {
                colonyShip.GetComponent<MovementController>().SetLocation(m_ownedSystems[0]);
            }
        }

    }



    // Update is called once per frame
    void Update()
    {


    }

    public void CreateArmy(Army.ArmyType type, SolarSystem system)
    {
        Army army = Instantiate(armyPrefab, transform.Find("Armies").transform).GetComponent<Army>();
        army.GetComponent<MovementController>().SetLocation(system);
        army.SetArmyType(type);
        army.SetEmpire(this);
        armies.Add(army);
    }

    public int GetDefensiveArmies()
    {
        int total = 0;
        foreach (Army army in GetArmies())
        {
            if (army.GetArmyType() == Army.ArmyType.Defensive)
            {
                total += 1;
            }
        }
        return total;
    }

    public int GetOffensiveArmies()
    {
        int total = 0;
        foreach (Army army in GetArmies())
        {
            if (army.GetArmyType() == Army.ArmyType.Defensive)
            {
                total += 1;
            }
        }
        return total;
    }
    
}
