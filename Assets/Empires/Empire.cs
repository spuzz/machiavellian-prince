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
    [SerializeField] GameObject colonyShipPrefab;
    [SerializeField] BuildingConfig colonyShipConfig;
    [SerializeField] UnitConfig attackUnit;
    [SerializeField] UnitConfig defenceUnit;
    [SerializeField] GameObject fleetPrefab;
    Universe universe;

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

    public string GetName()
    {
        return EmpireName;
    }
    public GameObject GetColonyShipPrefab()
    {
        return colonyShipPrefab;
    }

    public BuildingConfig GetColonyShipConfig()
    {
        return colonyShipConfig;
    }

    public UnitConfig GetDefenceConfig()
    {
        return defenceUnit;
    }


    public UnitConfig GetAttackConfig()
    {
        return attackUnit;
    }

    public GameObject GetFleetPrefab()
    {
        return fleetPrefab;
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

    public int GetPredictedNetIncome()
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

    public int GetPredictedGrossIncome()
    {
        int netIncome = 0;
        foreach (SolarSystem system in GetSystems())
        {
            netIncome += system.GetNetIncome();
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

    public float GetTotalArmyStrength()
    {
        float total = 0;
        foreach(Army army in armies)
        {
            total += army.GetArmyStrength();
        }
        return total;
    }

    public List<BuildingConfig> GetBuildingInProgress()
    {
        List <BuildingConfig> buildList = new List<BuildingConfig>();
        foreach (SolarSystem system in m_ownedSystems)
        {
            if(system.GetComponent<BuildController>().IsBuilding())
            {
                buildList.Add(system.GetComponent<BuildController>().GetBuildingInConstruction());
            }
        }
        return buildList;
    }

    public List<UnitConfig> GetTrainingInProgress()
    {
        List<UnitConfig> buildList = new List<UnitConfig>();
        foreach (Army army in armies)
        {
            if (army.GetComponent<ArmyTrainer>().IsBuilding())
            {
                buildList.Add(army.GetComponent<ArmyTrainer>().GetUnitBuilding());
            }
        }
        return buildList;
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
        universe = FindObjectOfType<Universe>();
        universe.onDayChanged += ProcessDayChange;
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

    private void ProcessDayChange(int days)
    {
        if(IsAlive())
        {
            m_currentLeader.LeadEmpire(this, GetComponent<EmpireController>());
        }

        int goldChange = 0;
        foreach(SolarSystem system in m_ownedSystems)
        {
            goldChange += system.GetNetIncome();
        }
        foreach(Army army in armies)
        {
            goldChange -= army.GetMaintenance();
        }
        AddGold(goldChange);
    }


    public void CreateArmy(Army.ArmyType type, SolarSystem system)
    {
        Army army = Instantiate(armyPrefab, transform.Find("Armies").transform).GetComponent<Army>();
        army.GetComponent<MovementController>().SetLocation(system);
        army.SetArmyType(type);
        army.SetEmpire(this);
        armies.Add(army);
        system.AddArmy(army);
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
            if (army.GetArmyType() == Army.ArmyType.Offensive)
            {
                total += 1;
            }
        }
        return total;
    }

    public bool HasOrBuildingColonyShip()
    {
        if(GetColonyShips().Count > 0)
        {
            return true;
        }
        foreach(SolarSystem system in m_ownedSystems)
        {
            if(system.GetComponent<BuildController>().IsBuilding() && system.GetComponent<BuildController>().GetBuildingInConstruction().GetName() == "ColonyShip")
            {
                return true;
            }
        }
        return false;
    }

    // TODO Add some checks to see if empire is in a good state ( and therefore could expand )
    public bool CheckSystemHealth()
    {
        return true;
    }


}
