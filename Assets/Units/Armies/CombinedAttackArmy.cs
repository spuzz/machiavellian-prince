using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedAttackArmy : MonoBehaviour
{

    List<Army> armies = new List<Army>();
    MovementController movementController;
    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    public void SetLocation(SolarSystem system)
    {
        movementController.SetLocation(system);
    }

    public SolarSystem GetLocation()
    {
        return movementController.GetSystemLocation();
    }

    public bool AddArmy(Army army)
    {
        if (GetLocation() != army.GetComponent<MovementController>().GetSystemLocation())
        {
            return false;
        }
        Transform armyMesh = army.transform.Find("ArmyMesh");
        armyMesh.GetComponent<MeshRenderer>().enabled = false;
        armyMesh.GetComponent<CapsuleCollider>().enabled = false;
        armies.Add(army);
        return true;
    }
}

