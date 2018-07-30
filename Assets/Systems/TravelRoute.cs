using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelRoute : MonoBehaviour {

    public SolarSystem systemOne;
    public SolarSystem systemTwo;
    
    float distance;
    List<Army> armiesTravelling;

    public float GetDistance()
    {
        return distance;
    }

    public void SetDistance(float distance)
    {
        this.distance = distance;
    }
    public bool IsBlocked(Empire empire)
    {
        List<Empire> blockedEmpires = empire.GetComponent<DiplomacyController>().GetEmpiresAtWar();
        foreach (Army army in armiesTravelling)
        {
            if (blockedEmpires.Contains(army.GetEmpire()))
            {
                return true;
            }
        }
        return false;
    }

    public void UseRoute(Army army)
    {
        armiesTravelling.Add(army);
    }

    public void finishUsingRoute(Army army)
    {
        armiesTravelling.Remove(army);
    }
    void Start () {
        armiesTravelling = new List<Army>();
    }
	
    public SolarSystem GetDestination(SolarSystem system)
    {
        if(systemOne == system)
        {
            return systemTwo;
        }
        else if(systemTwo == system)
        {
            return systemOne;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    public bool ContainsSystem(SolarSystem system)
    {
        if(systemOne == system || systemTwo == system)
        {
            return true;
        }
        return false;
    }

}
