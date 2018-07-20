using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empire : MonoBehaviour {

    [SerializeField] string EmpireName;
    Leader m_currentLeader;
    List<Leader> m_allPotentialLeaders;
    List<Planet> m_ownedPlanets = new List<Planet>();
	// Use this for initialization

    public Leader GetLeader()
    {
        return m_currentLeader;
    }

    public void GivePlanet(Planet planet)
    {
        if(!m_ownedPlanets.Find(c => c.GetName() == planet.GetName()))
        {
            m_ownedPlanets.Add(planet);
        }
        
    }

    public void TakePlanet(Planet planet)
    {
        if (m_ownedPlanets.Find(c => c.GetName() == planet.GetName()))
        {
            m_ownedPlanets.Remove(planet);
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
