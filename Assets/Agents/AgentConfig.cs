using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentConfig : ScriptableObject {

    [SerializeField] GameObject agentPrefab;
    [SerializeField] string Name;
    [SerializeField] int baseCost;
    [SerializeField] List<AbilityConfig> abilities;

    public GameObject GetAgentPrefab()
    {
        return agentPrefab;
    }
    public int GetCost()
    {
        return baseCost;
    }
}
