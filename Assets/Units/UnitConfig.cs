using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Unit"))]
public class UnitConfig : ScriptableObject
{
    public enum BuildType
    {
        Expand,
        Defend,
        Attack,
    }
    [SerializeField] GameObject unitPrefab;
    [SerializeField] string Name;
    [SerializeField] int baseCost;
    [SerializeField] bool joinArmy;
    [SerializeField] BuildType buildType;
    [SerializeField] int attackStrength;
    [SerializeField] int defenceStrength;
    [SerializeField] int buildTime;

    public int GetBuildTime()
    {
        return buildTime;
    }
    public int GetAttackStrength()
    {
        return attackStrength;
    }
    public int GetDefenceStrength()
    {
        return defenceStrength;
    }

    public GameObject GetUnitPrefab()
    {
        return unitPrefab;
    }

    public BuildType GetBuildType()
    {
        return buildType;
    }
    public int GetCost()
    {
        return baseCost;
    }

    public bool WillJoinArmy()
    {
        return joinArmy;
    }
}
