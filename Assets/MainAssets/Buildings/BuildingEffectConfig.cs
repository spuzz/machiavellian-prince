using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Building/BuildingEffect"))]
public class BuildingEffectConfig : ScriptableObject {
    [SerializeField] [Range(-100, 100)] float goldPerc;
    [SerializeField] float gold;
    [SerializeField] [Range(-100, 100)] float playerGoldPerc;
    [SerializeField] float playerGold;
    [SerializeField] [Range(-100, 100)] float defencePerc;
    [SerializeField] float defence;
    [SerializeField] [Range(-100, 100)] float buildingCostPerc;
    [SerializeField] float buildingCost;
    [SerializeField] [Range(-100, 100)] float unitCostPerc;
    [SerializeField] float unitCost;
    [SerializeField] [Range(-100, 100)] float abilityCostPerc;
    [SerializeField] float abilityCost;
    [SerializeField] [Range(-100, 100)] float abilitySuccessPerc;

    [SerializeField] bool empireWide;

    public float GetGoldPerc(){ return goldPerc; }
    public float GetGold() { return gold; }
    public float GetPlayerGoldPerc() { return playerGoldPerc; }
    public float GetPlayerGold() { return playerGold; }
    public float GetDefencePerc() { return defencePerc; }
    public float GetDefence() { return defence; }
    public float GetBuildingCostPerc() { return buildingCostPerc; }
    public float GetBuildingCost() { return buildingCost; }
    public float GetUnitCostPerc() { return unitCostPerc; }
    public float GetUnitCost() { return unitCost; }
    public float GetAbilityCostPerc() { return abilityCostPerc; }
    public float GetAbilityCost() { return abilityCost; }
    public float GetAbilitySuccessPerc() { return abilitySuccessPerc; }

    public bool isEmpireWide()
    {
        return empireWide;
    }
}
