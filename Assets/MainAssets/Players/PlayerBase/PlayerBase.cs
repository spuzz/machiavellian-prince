using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {

    Player player;
    List<BaseModule> baseModules;
    [SerializeField] GameObject baseModulePrefab;
    [SerializeField] List<BaseModuleConfig> availableModules;
    [SerializeField] List<AgentConfig> availableAgentTypes;
    [SerializeField] int range = 100;

    BuildTracker buildTracker;

    BaseModuleConfig baseModuleInConstruction = null;
    long trackerID;
    private void Awake()
    {
        baseModules = new List<BaseModule>();
        buildTracker = GetComponent<BuildTracker>();
        buildTracker.onBuildComplete += onModuleFinished;
    }

    private void Start()
    {
        SelectableComponent selectableComponent = GetComponentInChildren<SelectableComponent>();
        selectableComponent.UpdateName("PlayerBase");
        selectableComponent.SetScale(0.06f);
        selectableComponent.SetShown(true);
    }
    private void Update()
    {
        if(baseModuleInConstruction)
        {

        }
    }

    public AgentConfig GetDefaultConfig()
    {
        return availableAgentTypes[0];
    }

    public IEnumerable<AgentConfig> GetAgentConfigs()
    {
        return availableAgentTypes;
    }

    public bool BuildModule(BaseModuleConfig baseModuleConfig)
    {
        if(baseModuleInConstruction)
        {
            return false;
        }
        baseModuleInConstruction = baseModuleConfig;
        trackerID = buildTracker.StartBuild(5);
        return true;
    }

    public void onModuleFinished(long id)
    {
        if(id == trackerID)
        {
            AddModule(baseModuleInConstruction);
        }
        baseModuleInConstruction = null;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public int GetRange()
    {
        return range;
    }

    public void SetRange(int range)
    {
        this.range = range;
    }

    public void AddModule(BaseModuleConfig baseModuleConfig)
    {
        GameObject gameObject = Instantiate(baseModulePrefab, this.transform.Find("BaseModules"));
        BaseModule baseModule = gameObject.GetComponent<BaseModule>();
        foreach(AgentConfig agentConfig in baseModuleConfig.GetAgentConfigs())
        {
            baseModule.AddAgentConfig(agentConfig);
            availableAgentTypes.Add(agentConfig);
        }
        baseModules.Add(baseModule);
    }

    public void RemoveModule(BaseModule baseModule)
    {
        if(baseModules.Contains(baseModule))
        {
            baseModules.Remove(baseModule);
            foreach(AgentConfig agentConfig in baseModule.GetAgentConfigs())
            {
                availableAgentTypes.Remove(agentConfig);
            }
        }
    }

    public bool HireAgent(AgentConfig agentConfig)
    {
        
        if (player.UseGold(agentConfig.GetCost()))
        {
            GameObject agent = Instantiate(agentConfig.GetAgentPrefab(), player.transform.Find("Agents"));
            Agent agentComponent = agent.GetComponent<Agent>();
            agentComponent.SetPlayer(player);
            
            agent.transform.position = this.transform.position + RandomDirection();
            agentComponent.SetAgentName("Agent Smith");
            foreach (AbilityConfig ability in agentConfig.GetAbilities())
            {
                agentComponent.AddAbility(ability);
            }
            agentComponent.SetPortrait(agentConfig.GetRandomPortrait());
            player.AddAgent(agent);

            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 RandomDirection()
    {
        var x = UnityEngine.Random.Range(-1f, 1f);
        var z = UnityEngine.Random.Range(-1f, 1f);
        var distance = UnityEngine.Random.Range(1f, 3f);
        var direction = new Vector3(x, 0f, z);
        //if you need the vector to have a specific length:
        direction = direction.normalized * distance;
        return direction;
    }
}
