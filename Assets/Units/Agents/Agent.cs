using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    [SerializeField] string agentName;
    [SerializeField] Player player;
    [SerializeField] List<AbilityConfig> abilities;
    [SerializeField] SolarSystem targetSystem;
    public Player GetPlayer()
    {
        return player;
    }

    public void SetAgentName(string name)
    {
        agentName = name;
    }

    public string GetAgentName()
    {
        return agentName;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public SolarSystem GetTargetSystem()
    {
        return targetSystem;
    }

    public void SetTargetSystem(SolarSystem system)
    {
        // TODO Travel time
        if(targetSystem)
        {
            targetSystem.RemoveAgent(this);
        }
        system.AddAgent(this);
        targetSystem = system;
    }

    public void AddAbility(AbilityConfig ability)
    {
        abilities.Add(ability);
    }
    public List<AbilityConfig> GetAbilities()
    {
        return abilities;
    }
}

