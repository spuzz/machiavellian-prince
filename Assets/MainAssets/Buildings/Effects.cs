using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour {

    public float goldPerc = 0;
    public float gold = 0;
    public float playerGoldPerc = 0;
    public float playerGold = 0;
    public float defencePerc = 0;
    public float defence = 0;
    public float buildingCostPerc = 0;
    public float buildingCost = 0;
    public float unitCostPerc = 0;
    public float unitCost = 0;
    public float abilityCostPerc = 0;
    public float abilityCost = 0;
    public float abilitySuccessPerc = 0;

    public void AddEffects(BuildingEffectConfig buildingEffectConfig)
    {
        goldPerc += buildingEffectConfig.GetGoldPerc();
        gold += buildingEffectConfig.GetGold();
        playerGoldPerc += buildingEffectConfig.GetPlayerGoldPerc();
        playerGold += buildingEffectConfig.GetPlayerGold();
        defencePerc += buildingEffectConfig.GetDefencePerc();
        defence += buildingEffectConfig.GetDefence();
        buildingCostPerc += buildingEffectConfig.GetBuildingCostPerc();
        buildingCost += buildingEffectConfig.GetBuildingCost();
        unitCostPerc += buildingEffectConfig.GetUnitCostPerc();
        unitCost += buildingEffectConfig.GetUnitCost();
        abilityCostPerc += buildingEffectConfig.GetAbilityCostPerc();
        abilityCost += buildingEffectConfig.GetAbilityCost();
        abilitySuccessPerc += buildingEffectConfig.GetAbilitySuccessPerc();
    }

    public void RemoveEffects(BuildingEffectConfig buildingEffectConfig)
    {
        goldPerc -= buildingEffectConfig.GetGoldPerc();
        gold -= buildingEffectConfig.GetGold();
        playerGoldPerc -= buildingEffectConfig.GetPlayerGoldPerc();
        playerGold -= buildingEffectConfig.GetPlayerGold();
        defencePerc -= buildingEffectConfig.GetDefencePerc();
        defence -= buildingEffectConfig.GetDefence();
        buildingCostPerc -= buildingEffectConfig.GetBuildingCostPerc();
        buildingCost -= buildingEffectConfig.GetBuildingCost();
        unitCostPerc -= buildingEffectConfig.GetUnitCostPerc();
        unitCost -= buildingEffectConfig.GetUnitCost();
        abilityCostPerc -= buildingEffectConfig.GetAbilityCostPerc();
        abilityCost -= buildingEffectConfig.GetAbilityCost();
        abilitySuccessPerc -= buildingEffectConfig.GetAbilityCostPerc();
    }
}
