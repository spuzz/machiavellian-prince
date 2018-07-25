using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour {

    [SerializeField] string EmpireName;
    [SerializeField] Color empireColour;
    Leader m_currentLeader;
    List<Leader> m_allPotentialLeaders;
    List<Army> armies;
    List<SolarSystem> m_ownedSystems = new List<SolarSystem>();

    // Resources
    [SerializeField] int gold;
    [SerializeField] int buildingMaterial;
    [SerializeField] int spaceshipMaterial;


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

    public void AddBuildingMaterial(int buildingMaterial)
    {
        this.buildingMaterial += buildingMaterial;
    }

    public bool UseBuildingMaterial(int buildingMaterial)
    {
        if (this.buildingMaterial > buildingMaterial)
        {
            this.buildingMaterial -= buildingMaterial;
            return true;
        }
        return false;

    }

    public void AddSpaceshipMaterial(int spaceshipMaterial)
    {
        this.spaceshipMaterial += spaceshipMaterial;
    }

    public bool UseSpaceshipMaterial(int spaceshipMaterial)
    {
        if (this.spaceshipMaterial > spaceshipMaterial)
        {
            this.spaceshipMaterial -= spaceshipMaterial;
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

    public void RemoveArmy(Army army)
    {
        armies.Remove(army);
    }
    public List<Leader> GetPotentialLeaders()
    {
        return m_allPotentialLeaders;
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
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
