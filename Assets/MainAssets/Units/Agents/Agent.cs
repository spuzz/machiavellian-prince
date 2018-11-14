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
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material meshMaterial;

    MeshRenderer renderer;
    AgentMovementController agentMovementController;
    AbilityController abilityController;
    AgentUI agentUI;
    SelectableComponent select;



    private void Awake()
    {
        agentMovementController = GetComponent<AgentMovementController>();
        select = GetComponentInChildren<SelectableComponent>();
        agentUI = FindObjectOfType<AgentUI>();
        abilityController = FindObjectOfType<AbilityController>();
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        agentMovementController.onReachedTarget += OnReachTarget;
        select.UpdateName(agentName);
        select.SetScale(0.15f);
        select.SetShown(true);

    }

    private void OnReachTarget()
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
        agentMovementController.SetTarget(system.transform.position);
        if (targetSystem)
        {
            targetSystem.RemoveAgent(this);
        }
        
        targetSystem = system;
        systemLocation = null;
    }

    public void SetTarget(Vector3 position)
    {
        agentMovementController.SetTarget(position);
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


    public void SelectAgent(bool select)
    {
        List<Material> materials = new List<Material>();
        renderer.GetMaterials(materials);
        if (select)
        {
            materials.Add(meshMaterial);
        }
        else
        {
            if(materials.Count > 1)
            {
                materials.Remove(materials[1]);
            }
            
        }
        renderer.materials = materials.ToArray();
    }
    public void AddAbility(AbilityConfig ability)
    {
        abilityController.AddAbilityConfig(ability);
        abilities.Add(ability);
    }
    public IEnumerable<AbilityConfig> GetAbilities()
    {
        return abilities;
    }
}

