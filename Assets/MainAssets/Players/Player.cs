using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] string playerName;
    [SerializeField] int playerNumber;
    [SerializeField] Color playerColor;
    [SerializeField] List<GameObject> agents;
    [SerializeField] List<AgentConfig> AgentTypes;
    [SerializeField] long gold;

    Universe universe;
    List<Leader> leadersControlled = new List<Leader>();
    int systemsControlled;
    int empiresControlled;
    private void Start()
    {
        universe = FindObjectOfType<Universe>();
        HireAgent(AgentTypes[0]);

        universe.onSystemOwnerChanged += OnSystemChange;
        universe.onLeaderLoyaltyChanged += OnLeaderLoyaltyChange;
        universe.onLeaderDeath += OnLeaderDeath;
        universe.onEmpireLeaderChange += OnEmpireLeaderChange;
    }

    public int GetPlayerNumber() { return playerNumber;  }
    public string GetPlayerName() { return playerName; }
    public Color GetPlayerColor() { return playerColor; }

    public long GetGold()
    {
        return gold;
    }

    public int GetTotalAgents()
    {
        return agents.Count;
    }

    public IEnumerable<GameObject> GetAgents()
    {
        return agents;
    }

    public GameObject GetAgent(int number)
    {
        if(number < 0 || number >= agents.Count)
        {
            return null;
        }
        return agents[number];
    }


    public int GetTotalLeadersControlled()
    {
        return leadersControlled.Count;
    }

    public int GetSystemsControlled()
    {
        return systemsControlled;
    }

    public int GetEmpiresControlled()
    {
        return empiresControlled;
    }

    public bool HireAgent(AgentConfig agentConfig)
    {
        if(gold >= agentConfig.GetCost())
        {
            GameObject agent = Instantiate(agentConfig.GetAgentPrefab());
            agent.GetComponent<Agent>().SetPlayer(this);
            //agent.GetComponent<Agent>().SetTargetSystem(system);
            agent.GetComponent<Agent>().SetAgentName("Agent Smith");
            foreach(AbilityConfig ability in agentConfig.GetAbilities())
            {
                agent.GetComponent<Agent>().AddAbility(ability);
            }
            agent.GetComponent<Agent>().SetPortrait(agentConfig.GetRandomPortrait());
            agents.Add(agent);
            gold -= agentConfig.GetCost();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateStats()
    {
        empiresControlled = 0;
        systemsControlled = 0;
        foreach (Leader leader in leadersControlled)
        {
            if (leader.IsInControl())
            {
                Empire empire = leader.GetEmpire();
                empiresControlled += 1;
                systemsControlled += empire.GetTotalSystemsControlled();
            }

        }
    }

    private void OnSystemChange(SolarSystem system)
    {
        systemsControlled = 0;
        UpdateStats();
    }


    private void OnLeaderLoyaltyChange(Leader leader)
    {
        if(leadersControlled.Contains(leader))
        {
            leadersControlled.Remove(leader);
        }

        if(leader.ControlledBy() == this)
        {
            leadersControlled.Add(leader);
        }
    }

    private void OnLeaderDeath(Leader leader)
    {
        if(leadersControlled.Contains(leader))
        {
            leadersControlled.Remove(leader);
        }
    }

    private void OnEmpireLeaderChange(Empire empire, Leader leader)
    {
        UpdateStats();
    }
}
