using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Relationship
{
    War,
    Peace,
    Alliance
}


public class RelationshipStatus
{

    public Relationship relationship;
    public Dictionary<string, int> positiveModifiers;
    public Dictionary<string, int> negativeModifiers;
}

public class DiplomacyController : MonoBehaviour {


    Empire empire;
    Dictionary<Empire, RelationshipStatus> otherEmpires = new Dictionary<Empire, RelationshipStatus>();
	// Use this for initialization
	void Start () {
        empire = GetComponent<Empire>();
        foreach(Empire otherEmpire in FindObjectsOfType<Empire>())
        {
            if(otherEmpire != empire)
            {
                otherEmpires.Add(otherEmpire, new RelationshipStatus());
                otherEmpires[otherEmpire].relationship = Relationship.Peace;
            }
        }
	}
	
    public RelationshipStatus GetDiplomacy(Empire empire)
    {
        return otherEmpires[empire];
    }

    public void DeclareWar(Empire empire, Empire enemyEmpire)
    {
        otherEmpires[enemyEmpire].relationship = Relationship.War;
        enemyEmpire.GetComponent<DiplomacyController>().WarDeclaredBy(empire);
    }

    public void WarDeclaredBy(Empire enemyEmpire)
    {

        otherEmpires[enemyEmpire].relationship = Relationship.War;
    }

    public List<Empire> GetEmpiresAtWar()
    {
        List<Empire> empires = new List<Empire>();
        foreach(Empire empire in otherEmpires.Keys)
        {
            if(otherEmpires[empire].relationship == Relationship.War)
            {
                empires.Add(empire);
            }
        }
        return empires;
    }

    public void DestroyEmpire()
    {
        foreach (Empire enemyEmpire in otherEmpires.Keys)
        {
            enemyEmpire.GetComponent<DiplomacyController>().EmpireDestroyed(empire);
        }
    }
    public void EmpireDestroyed(Empire empire)
    {
        otherEmpires.Remove(empire);
    }
}
