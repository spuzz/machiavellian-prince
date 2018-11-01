using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Agent : MonoBehaviour
{
    [SerializeField] string agentName;
    [SerializeField] Player player;
    [SerializeField] List<AbilityConfig> abilities;
    [SerializeField] SolarSystem targetSystem;

    AICharacterControl aICharacterControl;

    private void Start()
    {
        SelectableComponent select = GetComponentInChildren<SelectableComponent>();
        select.UpdateName(agentName);
        select.SetScale(0.06f);
        select.SetShown(true);
        select.SetColor(Color.black);
    }
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
        aICharacterControl.SetTarget(system.transform);
        if (targetSystem)
        {
            targetSystem.RemoveAgent(this);
        }
        system.AddAgent(this);
        targetSystem = system;
    }

    public void SetTarget(Transform transform)
    {
        aICharacterControl.SetTarget(transform);
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

