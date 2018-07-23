using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour {

    [SerializeField] string EmpireName;
    [SerializeField] Color empireColour;
    Leader m_currentLeader;
    List<Leader> m_allPotentialLeaders;
    List<SolarSystem> m_ownedSystems = new List<SolarSystem>();

	// Use this for initialization

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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
