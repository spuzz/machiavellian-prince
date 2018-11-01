using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelRoute : MonoBehaviour {

    public SolarSystem systemOne;
    public SolarSystem systemTwo;
    
    float distance;
    Dictionary<Empire,int> empiresTravelling = new Dictionary<Empire, int>();

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
        DiplomacyController diplomacyController = empire.GetComponent<DiplomacyController>();
        List<Empire> blockedEmpires = diplomacyController.GetEmpiresAtWar();
        foreach (Empire empTravelling in empiresTravelling.Keys)
        {
            if (blockedEmpires.Contains(empTravelling))
            {
                return true;
            }
        }
        return false;
    }

    public void UseRoute(Empire empire)
    {
        if(empiresTravelling.ContainsKey(empire))
        {

            empiresTravelling[empire] += 1;
        }
        else
        {
            empiresTravelling.Add(empire, 1);
        }
    }

    public void finishUsingRoute(Empire empire)
    {
        if (empiresTravelling.ContainsKey(empire))
        {

            empiresTravelling[empire]-= 1;
            if(empiresTravelling[empire] <= 0)
            {
                empiresTravelling.Remove(empire);
            }
        }

    }
    void Start () {
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
