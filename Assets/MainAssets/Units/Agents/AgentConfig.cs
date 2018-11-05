using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentConfig : ScriptableObject {

    [SerializeField] GameObject agentPrefab;
    [SerializeField] string Name;
    [SerializeField] int baseCost;
    [SerializeField] List<AbilityConfig> abilities;
    [SerializeField] List<Sprite> portraits;

    public GameObject GetAgentPrefab()
    {
        return agentPrefab;
    }

    public int GetCost()
    {
        return baseCost;
    }

    public List<AbilityConfig> GetAbilities()
    {
        return abilities;
    }

    public IEnumerable<Sprite> GetPortraits()
    {
        return portraits;
    }

    public Sprite GetRandomPortrait()
    {
        return portraits[UnityEngine.Random.Range(0, portraits.Count)];
    }
}
