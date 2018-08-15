using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedAttackArmy : MonoBehaviour {

    List<Army> armies = new List<Army>();
    MovementController movementController;
    LineRenderer line;
    private void Awake()
    {
        line = tranGetComponent<LineRenderer>();
        movementController = GetComponent<MovementController>();
        line.SetVertexCount(360);
        line.useWorldSpace = false;
        DrawCircle();
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
        if(GetLocation() != army.GetComponent<MovementController>().GetSystemLocation())
        {
            return false;
        }
        Transform armyMesh = army.transform.Find("ArmyMesh");
        armyMesh.GetComponent<MeshRenderer>().enabled = false;
        armyMesh.GetComponent<CapsuleCollider>().enabled = false;
        armies.Add(army);
        return true;
    }

    private void DrawCircle()
    {

        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (360); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * 1;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * 1;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / 360);
        }
        line.transform.LookAt(Camera.main.transform);
        line.transform.Translate(Vector3.forward);
    }
}
