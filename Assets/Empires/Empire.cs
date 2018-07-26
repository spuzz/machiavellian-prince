using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour {

    [SerializeField] string EmpireName;
    [SerializeField] Color empireColour;
    [SerializeField] List<UnitConfig> buildableUnits;
    Leader m_currentLeader;
    List<Leader> m_allPotentialLeaders;
    List<Army> armies = new List<Army>();
    List<ColonyShip> colonyShips = new List<ColonyShip>();
    List<SolarSystem> m_ownedSystems = new List<SolarSystem>();

    // Resources
    [SerializeField] int gold;


	// Use this for initialization
    public void AddGold(int gold)
    {
        this.gold += gold;
    }

    public bool UseGold(int gold)
    {
        if(this.gold > gold)
        {
            this.gold -= gold;
            return true;
        }
        return false;
        
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
    public List<ColonyShip> GetColonyShips()
    {
        return colonyShips;
    }


    public void RemoveColonyShip(ColonyShip colonyShip)
    {
        colonyShips.Remove(colonyShip);
    }

    public List<Leader> GetPotentialLeaders()
    {
        return m_allPotentialLeaders;
    }

    public void AddArmy(Army army)
    {
        army.SetEmpire(this);
        armies.Add(army);
    }
	void Start () {
        m_allPotentialLeaders = new List<Leader>(GetComponentsInChildren<Leader>());
        if(m_allPotentialLeaders.Count > 0)
        {
            m_currentLeader = m_allPotentialLeaders[0];
        }

        armies = new List<Army>(GetComponentsInChildren<Army>());
        foreach(Army army in armies)
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
            if(!colonyShip.GetComponent<MovementController>().GetSystemLocation())
            {
                colonyShip.GetComponent<MovementController>().SetLocation(m_ownedSystems[0]);
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
        m_ownedSystems[0].GetComponent<BuildController>().BuildUnit(buildableUnits[1]);

    }
}
