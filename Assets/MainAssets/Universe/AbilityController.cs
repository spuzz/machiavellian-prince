using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour {

    List<AbilityConfig> abilityConfigs = new List<AbilityConfig>();

    public void AddAbilityConfig(AbilityConfig abilityConfig)
    {
        if (!abilityConfigs.Contains(abilityConfig))
        {
            abilityConfigs.Add(abilityConfig);
            abilityConfig.AddComponent(gameObject);
        }
    }
}
