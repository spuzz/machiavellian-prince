using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireController : MonoBehaviour {

    List<SolarSystem> enemyBorderSystems = new List<SolarSystem>();
    List<SolarSystem> neutralBorderSystems = new List<SolarSystem>();
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


    void Start () {
        empire = gameObject.GetComponent<Empire>();
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
        }
    }
	// Update is called once per frame
	void Update () {
        Build();
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

    private void ColonisePlanets()
    {
        FindNeutralBorderSystems();
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
                army.GetComponent<MovementController>().MoveTo(enemyBorderSystems[border]);
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
                    enemyBorderSystems.Add(system);
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

    }
}
