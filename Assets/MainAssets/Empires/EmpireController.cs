using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmpireController : MonoBehaviour {

    List<SolarSystem> neutralBorderSystems = new List<SolarSystem>();
    List<SolarSystem> enemyBorderSystems = new List<SolarSystem>();
    List<SolarSystem> empireEnemyBorderSystems = new List<SolarSystem>();
    [SerializeField] Universe m_universe;

    bool systemChangeUpdate = true;

    Empire empire;
    DiplomacyController diplomacy;

    void Start () {
        m_universe = FindObjectOfType<Universe>();
        m_universe.onSystemOwnerChanged += onSystemChange;
        
        empire = gameObject.GetComponent<Empire>();
        diplomacy = gameObject.GetComponent<DiplomacyController>();

    }
	
    public DiplomacyController GetDiplomacyController()
    {
        return diplomacy;
    }
    public List<SolarSystem> GetEnemyBorderSystems()
    {
        return enemyBorderSystems;
    }

    public List<SolarSystem> GetNeutralBorderSystems()
    {
        return neutralBorderSystems;
    }

    public List<SolarSystem> GetEmpireEnemyBorderSystems()
    {
        return empireEnemyBorderSystems;
    }

    private void onSystemChange(SolarSystem system)
    {
        systemChangeUpdate = true;
        FindEnemyBorderSystems();
        FindNeutralBorderSystems();
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



    private void FindEnemyBorderSystems()
    {
        enemyBorderSystems.Clear();
        empireEnemyBorderSystems.Clear();
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
    

}
