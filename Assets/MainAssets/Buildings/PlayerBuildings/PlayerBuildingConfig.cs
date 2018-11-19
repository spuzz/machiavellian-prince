using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Building/PlayerBuilding"))]
public class PlayerBuildingConfig : ScriptableObject {

    [SerializeField] float upgradeCost;
    [SerializeField] List<BuildingEffectConfig> buildingEffectConfigs;
    [SerializeField] int buildTime;
    [SerializeField] Sprite underConstructionImage;
    [SerializeField] Sprite buildingImage;

    public float GetUpgradeCost()
    {
        return upgradeCost;
    }

    public IEnumerable<BuildingEffectConfig> GetEffects()
    {
        return buildingEffectConfigs;
    }

    public int GetBuildTime()
    {
        return buildTime;
    }

    public Sprite GetConstructionImage()
    {
        return underConstructionImage;
    }

    public Sprite GetBuildingImage()
    {
        return buildingImage;
    }
}
