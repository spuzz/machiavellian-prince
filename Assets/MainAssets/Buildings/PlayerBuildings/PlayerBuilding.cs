using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour {

    PlayerBuildingConfig playerBuildingConfig;
    Universe universe;
    SolarSystem system;
    bool inConstruction;
    int buildingNumber;
    int daysLeftOnBuild;

    private void Awake()
    {
        universe = FindObjectOfType<Universe>();
    }
    
    public int GetBuildingNumber()
    {
        return buildingNumber;
    }

    public bool IsInConstrution()
    {
        return inConstruction;
    }

    public void SetConfig(PlayerBuildingConfig playerBuildingConfig)
    {
        this.playerBuildingConfig = playerBuildingConfig;
    }

    public void SetSystem(SolarSystem system)
    {
        this.system = system;
    }

    public Sprite GetImage()
    {
        if(IsInConstrution())
        {
            return playerBuildingConfig.GetConstructionImage();
        }
        else
        {
            return playerBuildingConfig.GetBuildingImage();
        }
    }

    public void Build(int buildingNumber)
    {
        this.buildingNumber = buildingNumber;
        if(!universe)
        {
            universe = FindObjectOfType<Universe>();
        }
        if(playerBuildingConfig)
        {
            inConstruction = true;
            daysLeftOnBuild = playerBuildingConfig.GetBuildTime();
            universe.onDayChanged += OnDayChange;
        }
    }

    public void Raze()
    {
        if (playerBuildingConfig)
        {
            foreach (BuildingEffectConfig buildingEffectConfig in playerBuildingConfig.GetEffects())
            {
                system.GetComponent<Effects>().RemoveEffects(buildingEffectConfig);
            }
            playerBuildingConfig = null;
        }
    }
    private void OnDestroy()
    {
        if (playerBuildingConfig)
        {
            foreach (BuildingEffectConfig buildingEffectConfig in playerBuildingConfig.GetEffects())
            {
                if(!buildingEffectConfig.isEmpireWide())
                {
                    system.GetComponent<Effects>().RemoveEffects(buildingEffectConfig);
                }
                else
                {
                    Empire empire = system.GetEmpire();
                    if(empire)
                    {
                        empire.GetComponent<Effects>().RemoveEffects(buildingEffectConfig);
                    }
                }
                    
            }
            playerBuildingConfig = null;
        }
    }

    public IEnumerable<BuildingEffectConfig> GetEffects()
    {
        return playerBuildingConfig.GetEffects();
    }

    public void OnDayChange(int days)
    {
        if (IsInConstrution())
        {
            daysLeftOnBuild -= days;
            if (daysLeftOnBuild <= 0)
            {

                inConstruction = false;
                universe.onDayChanged -= OnDayChange;
                foreach (BuildingEffectConfig buildingEffectConfig in playerBuildingConfig.GetEffects())
                {
                    if (!buildingEffectConfig.isEmpireWide())
                    {
                        system.GetComponent<Effects>().AddEffects(buildingEffectConfig);
                    }
                    else
                    {
                        Empire empire = system.GetEmpire();
                        if (empire)
                        {
                            empire.GetComponent<Effects>().AddEffects(buildingEffectConfig);
                        }
                    }
                }
                
            }
        }

    }
}
