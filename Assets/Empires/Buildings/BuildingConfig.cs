using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Building/StandardBuilding"))]
public class BuildingConfig : ScriptableObject {

    [SerializeField] string Name;
    [SerializeField] int baseCost;
    [SerializeField] int buildTime;


    public string GetName()
    {
        return Name;
    }
    public int GetBuildTime()
    {
        return buildTime;
    }
    public int GetCost()
    {
        return baseCost;
    }
}
