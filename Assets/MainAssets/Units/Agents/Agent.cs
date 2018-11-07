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
    SelectableComponent select;
    private void Awake()
    {
        aICharacterControl = GetComponent<AICharacterControl>();
        select = GetComponentInChildren<SelectableComponent>();
        agentUI = FindObjectOfType<AgentUI>();


    }

    private void Start()
    {
        
        select.UpdateName(agentName);
        select.SetScale(0.15f);
        select.SetShown(true);
       
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
        if(player.IsHumanPlayer())
        {
            select.SetColor(Color.white);
       
        }
        else
        {
            select.SetColor(Color.black);
        }
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
        ability.AddComponent(gameObject);
        abilities.Add(ability);
    }
    public IEnumerable<AbilityConfig> GetAbilities()
    {
        return abilities;
    }
}

