using System;
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
    List<Empire> empires = new List<Empire>();
    bool alive = true;

    private void Awake()
    {
        universe = FindObjectOfType<Universe>();
        empiresControlled = 0;
    }

    private void Start()
    {
        universe.onSystemOwnerChanged += OnSystemChange;
        universe.onLeaderLoyaltyChanged += OnLeaderLoyaltyChange;
        universe.onLeaderDeath += OnLeaderDeath;
        universe.onEmpireLeaderChange += OnEmpireLeaderChange;

    }

    private void Update()
    {
        if(alive && empires.Count == 0)
        {
            PlayerLost();
        }
    }

    public bool IsAlive()
    {
        return alive;
    }
    public void TakeControlOfEmpire(Empire empire)
    {
        foreach (Leader leader in empire.GetPotentialLeaders())
        {
            leader.IncreaseInfluence(this, 1000);
            OnLeaderLoyaltyChange(leader);
        }
        UpdateStats();
    }

    public void HireInitialAgents()
    {
        HireAgent(AgentTypes[0]);
    }

    private void PlayerLost()
    {
        if(IsHumanPlayer())
        {
            universe.GameOver();
        }
        else
        {
            alive = false;
            foreach(GameObject agent in agents)
            {
                Destroy(agent);
            }
            agents.Clear();
            universe.CheckEndGame();
        }
    }

    public bool IsHumanPlayer()
    {
        if(GetComponent<HumanController>())
        {
            return true;
        }
        return false;
    }



    public int GetPlayerNumber() { return playerNumber;  }
    public string GetPlayerName() { return playerName; }
    public Color GetPlayerColor() { return playerColor; }

    public long GetGold()
    {
        return gold;
    }

    public bool UseGold(int cost)
    {
        if(gold < cost)
        {
            return false;
        }
        gold -= cost;
        return true;
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
            GameObject agent = Instantiate(agentConfig.GetAgentPrefab(),transform.Find("Agents"));
            Agent agentComponent = agent.GetComponent<Agent>();
            agentComponent.SetPlayer(this);
            agent.transform.position = empires[0].GetSystems()[0].transform.position;
            agentComponent.SetTargetSystem(empires[0].GetSystems()[0]);
            agentComponent.SetAgentName("Agent Smith");
            foreach(AbilityConfig ability in agentConfig.GetAbilities())
            {
                agentComponent.AddAbility(ability);
            }
            agentComponent.SetPortrait(agentConfig.GetRandomPortrait());
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
        empires.Clear();
        foreach (Leader leader in leadersControlled)
        {
            if (leader.IsInControl())
            {
                Empire empire = leader.GetEmpire();
                empiresControlled += 1;
                empires.Add(empire);
                systemsControlled += empire.GetTotalSystemsControlled();
            }

        }
    }

    private void OnSystemChange(SolarSystem system)
    {
    
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
        UpdateStats();
    }

    private void OnLeaderDeath(Leader leader)
    {
        if(leadersControlled.Contains(leader))
        {
            leadersControlled.Remove(leader);
        }
        UpdateStats();
    }

    private void OnEmpireLeaderChange(Empire empire, Leader leader)
    {
        UpdateStats();
    }
}
