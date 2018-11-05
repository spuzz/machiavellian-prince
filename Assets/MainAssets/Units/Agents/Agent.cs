using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Agent : MonoBehaviour
{
    [SerializeField] string agentName;
    [SerializeField] Player player;
    [SerializeField] List<AbilityConfig> abilities;
    [SerializeField] SolarSystem targetSystem;
    [SerializeField] Sprite portrait;
    [SerializeField] float stoppingDistance = 2.0f;
    [SerializeField] SolarSystem systemLocation;

    AICharacterControl aICharacterControl;
    AgentUI agentUI;

    private void Start()
    {
        SelectableComponent select = GetComponentInChildren<SelectableComponent>();
        aICharacterControl = GetComponent<AICharacterControl>();
        agentUI = FindObjectOfType<AgentUI>();
        select.UpdateName(agentName);
        select.SetScale(0.06f);
        select.SetShown(true);
        select.SetColor(Color.black);
    }

    private void Update()
    {
        if(targetSystem && Vector3.Distance(transform.position,targetSystem.transform.position) <= stoppingDistance)
        {
            OnReachTargetSystem();
        }
    }
    private void OnReachTargetSystem()
    {
        if(targetSystem)
        {
            systemLocation = targetSystem;
            systemLocation.AddAgent(this);
        }
        targetSystem = null;
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

    public SolarSystem GetCurrentSystem()
    {
        return systemLocation;
    }

    public void SetTargetSystem(SolarSystem system)
    {
        aICharacterControl.SetTarget(system.transform.position);
        if (targetSystem)
        {
            targetSystem.RemoveAgent(this);
        }
        
        targetSystem = system;
        systemLocation = null;
    }

    public void SetTarget(Vector3 position)
    {
        aICharacterControl.SetTarget(position);
        if (targetSystem)
        {
            targetSystem.RemoveAgent(this);
        }
        
        targetSystem = null;
        systemLocation = null;
    }

    public Sprite GetPortrait()
    {
        return portrait;
    }

    public void SetPortrait(Sprite sprite)
    {
        portrait = sprite;
    }

    public void AddAbility(AbilityConfig ability)
    {
        abilities.Add(ability);
    }
    public IEnumerable<AbilityConfig> GetAbilities()
    {
        return abilities;
    }
}

