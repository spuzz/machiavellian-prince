using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    [SerializeField] string agentName;
    [SerializeField] Player player;
    [SerializeField] List<AbilityConfig> abilities;
    [SerializeField] Planet targetPlanet;
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

    public Planet GetTargetPlanet()
    {
        return targetPlanet;
    }

    public void SetTargetPlanet(Planet planet)
    {
        // TODO Travel time
        if(targetPlanet)
        {
            targetPlanet.RemoveAgent(this);
        }
        planet.AddAgent(this);
        targetPlanet = planet;
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

